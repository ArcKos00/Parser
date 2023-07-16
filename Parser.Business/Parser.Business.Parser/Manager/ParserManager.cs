using Parser.Business.Parser.Wrapper;
using Parser.Common.Repos.Parser;
using Parser.Common.Parser.ParsedPages;
using Parser.Common.ParserManager;
using Parser.Common.SqlManager;
using Microsoft.Extensions.Logging;

namespace Parser.Business.Parser.Manager
{
    public class ParserManager : IParserManager
    {
        private const string _startUrl = "http://www.ilcats.ru/toyota/?function=getModels&market=EU";
        private readonly IParser _parser;
        private readonly ISqlManager _sqlManager;
        private readonly ILogger<ParserManager> _logger;
        private int _count = 0;

        public ParserManager(IParser parser, ISqlManager sql, ILogger<ParserManager> logger)
        {
            _logger = logger;
            _parser = parser;
            _sqlManager = sql;
        }

        /// <summary>
        /// початок парсингу починаючи з першої сторінки за посиланням на першу сторінку.
        /// </summary>
        /// <returns>await.</returns>
        public async Task Start(int count)
        {
            _count = count;
            using (var st = new StopWatchWrapper())
            {
                await HandleCarPage(_startUrl, 1);
            }
        }

        /// <summary>
        /// Отримує та обробляє спаршені дані з першої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public async Task HandleCarPage(string url, int previousId)
        {
            _logger.LogInformation("page1");
            var nextUrls = new List<string>();
            var firstPageQueue = new Queue<CarPage>();
            await foreach (var item in _parser.ParseCarPage(url))
            {
                foreach (var nextUrl in item.Urls)
                {
                    nextUrls.Add(nextUrl);
                }

                item.Urls = new List<string>(0);
                firstPageQueue.Enqueue(item);
            }

            var list = await _sqlManager.SendCarPage(firstPageQueue, previousId);
            await HandleOneNextPageUrl(nextUrls, list, HandleComplectationPage);
        }

        /// <summary>
        /// Отримує та обробляє спаршені дані з другої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public async Task HandleComplectationPage(string url, int previousId)
        {
            _logger.LogInformation("page2");
            var nextUrls = new List<string>();
            var secondPageQueue = new Queue<ComplectationPage>();
            await foreach (var item in _parser.ParseComplectationPage(url))
            {
                nextUrls.Add(item.Url);
                secondPageQueue.Enqueue(item);
            }

            var list = await _sqlManager.SendComplectationPage(secondPageQueue, previousId);
            await HandleOneNextPageUrl(nextUrls, list, HandleGroupPage);
        }

        /// <summary>
        /// Отримує та обробляє спаршені дані з третьої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public async Task HandleGroupPage(string url, int previousId)
        {
            _logger.LogInformation("page3");
            var nextUrls = new List<string>();
            var thirdPageQueue = new Queue<GroupPage>();
            await foreach (var item in _parser.ParseSpareGroupPage(url))
            {
                nextUrls.Add(item.NextUrl);
                thirdPageQueue.Enqueue(item);
            }

            var list = await _sqlManager.SendGroupPage(thirdPageQueue);
            await HandleOneNextPageUrl(nextUrls, list, previousId, HandleSubGroupPage);
        }

        /// <summary>
        /// Отримує та обробляє спаршені дані з четвертої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public async Task HandleSubGroupPage(string url, int previousId, int complectationId)
        {
            _logger.LogInformation("page4");
            var nextUrls = new List<string>();
            var fourthPageQueue = new Queue<SubGroupPage>();
            await foreach (var item in _parser.ParseSpareSubGroupPage(url))
            {
                nextUrls.Add(item.NextUrl);
                fourthPageQueue.Enqueue(item);
            }

            var list = await _sqlManager.SendSubGroupPage(fourthPageQueue, previousId, complectationId);
            await HandleOneNextPageUrl(nextUrls, list, HandleDetailPage);
        }

        /// <summary>
        /// Отримує та обробляє спаршені дані з пятої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public async Task HandleDetailPage(string url, int previousId)
        {
            _logger.LogInformation("page5");
            var fifthPageQueue = new Queue<DetailPage>();
            await foreach (var item in _parser.ParseDetailPage(url))
            {
                fifthPageQueue.Enqueue(item);
            }

            await _sqlManager.SendDetailPage(fifthPageQueue, previousId);
        }

        /// <summary>
        /// метод для збереження статичного html коду без javascript у поточну директорію (для тесту).
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>Task.</returns>
        public async Task SavePageAsStaticHtml(string url)
        {
            await _parser.TestSaveSaticHtmlPage(url);
        }

        private async Task HandleOneNextPageUrl(List<string> urllist, List<int> ids, int previousId, Func<string, int, int, Task> func)
        {
            for (int i = 0; i < Math.Min(urllist.Count, _count); i++)
            {
                await func(urllist[i], ids[i], previousId);
            }
        }

        private async Task HandleOneNextPageUrl(List<string> urllist, List<int> ids, Func<string, int, Task> func)
        {
            for (int i = 0; i < Math.Min(urllist.Count, _count); i++)
            {
                await func(urllist[i], ids[i]);
            }
        }
    }
}
