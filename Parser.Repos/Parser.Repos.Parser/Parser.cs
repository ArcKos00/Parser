using Parser.Common.Repos.Parser;
using Parser.Common.Parser.ParsedPages;
using Parser.Repos.Parser.Models;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AutoMapper;

namespace Parser.Repos.Parser
{
    public class Parser : IParser
    {
        private const string _url = "https://www.ilcats.ru/";
        private readonly IMapper _mapper;
        private readonly IHtmlParser _parser;
        private readonly IHttpClientFactory _clientFactory;

        public Parser(IMapper mapper, IHttpClientFactory factory)
        {
            _clientFactory = factory;
            _mapper = mapper;
            _parser = new HtmlParser();
        }

        /// <summary>
        /// Розбирає усі дані з сторінки з автівками.
        /// </summary>
        /// <param name="url">Link to first Page.</param>
        /// <returns>Таск.</returns>
        public async IAsyncEnumerable<CarPage> ParseCarPage(string url)
        {
            var document = await GetHtmlDocument(url);

            foreach (var car in GetCarWithAssemblyes(GetCarList(document)))
            {
                yield return _mapper.Map<CarPage>(new CarPageEntity
                {
                    Name = car.GetElementsByClassName("Header").First().TextContent,
                    Cars = GetCarInfo(car),
                    Urls = GetUrls(car)
                });
            }
        }

        /// <summary>
        /// Розбирає усі дані на сторінці з комплектаціями.
        /// </summary>
        /// <param name="url">посилання на другу сторінку.</param>
        /// <returns>Таск.</returns>
        public async IAsyncEnumerable<ComplectationPage> ParseComplectationPage(string url)
        {
            var document = await GetHtmlDocument(url);

            var complectationTable = document.QuerySelector("table > tbody");
            var tableRows = complectationTable?.QuerySelectorAll("tr").ToList();

            foreach (var info in GetRowInfo(tableRows!))
            {
                yield return _mapper.Map<ComplectationPage>(info);
            }
        }

        /// <summary>
        /// Розбирає усі дані з сторінки з групами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>список даних з третьої сторінки.</returns>
        public async IAsyncEnumerable<GroupPage> ParseSpareGroupPage(string url)
        {
            var document = await GetHtmlDocument(url);

            var body = document.GetElementById("Body");
            var groups = body?.GetElementsByClassName("name");
            foreach (var group in groups!)
            {
                yield return _mapper.Map<GroupPage>(new SpareGroupPageEntity
                {
                    Name = group.TextContent,
                    NextUrl = GetUrl(group)
                });
            }
        }

        /// <summary>
        /// Розбирає усі дані з сторінки з підгрупами.
        /// </summary>
        /// <param name="url">посилання на третю сторінку.</param>
        /// <returns>Список даних з четвертої сторінки.</returns>
        public async IAsyncEnumerable<SubGroupPage> ParseSpareSubGroupPage(string url)
        {
            var document = await GetHtmlDocument(url);

            var subgroups = document.QuerySelectorAll("div.Tiles > div.List > div.List > div.name");

            foreach (var subgroup in subgroups)
            {
                var nextUrl = GetUrl(subgroup);
                yield return _mapper.Map<SubGroupPage>(new SpareSubGroupPageEntity
                {
                    Name = subgroup.TextContent,
                    NextUrl = nextUrl,
                    ImageUrl = await FindImageOnPage(nextUrl)
                });
            }
        }

        /// <summary>
        /// Парсить сторінку з деталями.
        /// </summary>
        /// <param name="url">посилання на пяту сторінку.</param>
        /// <returns>повертає дані з пятої сторінки.</returns>
        public async IAsyncEnumerable<DetailPage> ParseDetailPage(string url)
        {
            var document = await GetHtmlDocument(url);
            var schemaTable = document.QuerySelector("table > tbody") !;

            foreach (var detailElement in GetDetailCode_Name(document))
            {
                var code_name = GetCodeAndName(detailElement);

                yield return _mapper.Map<DetailPage>(new SpareDetailPageEntity
                {
                    DetailCode = code_name.Item1,
                    DetailName = code_name.Item2,
                    Details = GetDetailBlocks(schemaTable, code_name).ToList(),
                });
            }
        }

        /// <summary>
        /// Збарігає статичний html текст.
        /// </summary>
        /// <param name="url">url адреса з якої скачати html.</param>
        /// <returns>Таск.</returns>
        public async Task TestSaveSaticHtmlPage(string url)
        {
            var index = "index.html";
            File.Delete(index);
            var html = await _clientFactory.CreateClient().GetStringAsync(url);
            var document = await _parser.ParseDocumentAsync(html);
            using (var writer = new StreamWriter(index))
            {
                writer.Write(document.QuerySelector("body")?.OuterHtml);
            }
        }

        private async Task<string> FindImageOnPage(string url)
        {
            var document = await GetHtmlDocument(url);
            var image = GetImage(document.QuerySelector("div.Image"));
            return image!;
        }

        private async Task<IHtmlDocument> GetHtmlDocument(string url)
        {
            var html = await _clientFactory.CreateClient().GetStringAsync(url);
            return await _parser.ParseDocumentAsync(html);
        }

        private string? GetImage(IElement? page)
        {
            return "https:" + page?.QuerySelector("img")?.GetAttribute("src");
        }

        private IEnumerable<IElement> GetDetailCode_Name(IDocument document)
        {
            return document.QuerySelectorAll("th[colspan='4']");
        }

        private IEnumerable<DetailEntity> GetDetailBlocks(IElement document, (string, string) code)
        {
            var list = new List<DetailEntity>();
            var detailInfo = document.QuerySelectorAll($"tr[data-id='{code.Item1}']");

            for (int i = 1; i < detailInfo.Length; i++)
            {
                var columns = GetTableColumns(detailInfo[i], "td").ToArray();

                var dates = SetDate(columns[2]);
                var codes = GetDetailCodes(detailInfo[i]).ToList();
                var count = 0;
                if (int.TryParse(columns[1], out int result))
                {
                    count = result;
                }

                list.Add(new DetailEntity
                {
                    Codes = codes,
                    Count = count,
                    StartDate = dates.Item1,
                    EndDate = dates.Item2,
                    Usings = columns[3]
                });
            }

            return list;
        }

        private IEnumerable<string> GetDetailCodes(IElement element)
        {
            return element.QuerySelectorAll("a").Select(s => s.TextContent);
        }

        private (string, string) GetCodeAndName(IElement element)
        {
            var dsss = element.TextContent;
            var items = dsss.Split("&nbsp;", 2);
            if (items.Length <= 1)
            {
                items = element.TextContent.Split(' ', 2);
            }

            var code = items[0];
            var name = items[1];

            return (code, name);
        }

        private string GetUrl(IElement element)
        {
            return _url + element.QuerySelector("a")?.GetAttribute("href");
        }

        private (DateTime?, DateTime?) SetDate(string date)
        {
            var dates = date.Split(" - ");

            var startDate = ConvertDateStringToDate(dates[0]);
            var endDate = ConvertDateStringToDate(dates[1]);

            return (startDate, endDate);
        }

        private DateTime? ConvertDateStringToDate(string date)
        {
            if (date.Contains("..."))
            {
                return null!;
            }

            var newDate = ParseDateString(date);

            return new DateTime(newDate[1], newDate[0], 1);
        }

        private int[] ParseDateString(string dateString)
        {
            return dateString
                .Split(".")
                .Select(s => int.Parse(s))
                .ToArray();
        }

        private List<CarEntity> GetCarInfo(IElement carAssemblies)
        {
            var list = new List<CarEntity>();
            foreach (var info in carAssemblies.QuerySelectorAll("div.List > div.List > div.List > div.List"))
            {
                var dates = SetDate(info.QuerySelector("div.dateRange")?.TextContent!);
                list.Add(new CarEntity
                {
                    Code = info.QuerySelector("div.id")?.TextContent!,
                    StartDate = dates.Item1,
                    EndDate = dates.Item2,
                    Assembly = info.QuerySelector("div.modelCode")?.TextContent!,
                });
            }

            return list;
        }

        private List<string> GetUrls(IElement element)
        {
            return element.QuerySelectorAll("a")
                .Select(s => _url + s.GetAttribute("href"))
                .ToList();
        }

        private IEnumerable<IElement> GetCarWithAssemblyes(IElement carList)
        {
            return carList.QuerySelectorAll("div.Multilist > div.List");
        }

        private IElement GetCarList(IDocument document)
        {
            return document.GetElementsByClassName("List Multilist").First();
        }

        private IEnumerable<ComplectationPageEntity> GetRowInfo(List<IElement> rows)
        {
            for (int i = 1; i < rows.Count; i++)
            {
                yield return new ComplectationPageEntity
                {
                    Url = GetUrl(rows[i]),
                    Complectation = GetComplectation(rows[i]),
                    Attributes = GetAttributes(rows[0], rows[i])
                };
            }
        }

        private Dictionary<string, string> GetAttributes(IElement header, IElement row)
        {
            var attributes = new Dictionary<string, string>();
            var headers = GetTableColumns(header, "th").ToArray();
            var columns = GetTableColumns(row, "td").ToArray();
            for (int i = 2; i < headers.Count(); i++)
            {
                attributes.Add(headers[i], columns[i]);
            }

            return attributes;
        }

        private IEnumerable<string> GetTableColumns(IElement row, string selector)
        {
            return row.QuerySelectorAll(selector)
                .Select(s => s.TextContent)
                .ToList();
        }

        private ComplectationInfoEntity GetComplectation(IElement row)
        {
            var values = GetTableColumns(row, "td").ToArray();
            var date = SetDate(values[1]);

            return new ComplectationInfoEntity
            {
                Complectation = values[0],
                StartDate = date.Item1,
                EndDate = date.Item2,
            };
        }
    }
}