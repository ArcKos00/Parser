using Parser.Common.TableManager;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Utils.Shared;

namespace Parser.Function
{
    public class GetTableFunction
    {
        private readonly string _telegramIdentifier;
        private readonly string _chatId;
        private readonly ITableInfoManager _tableInfoRepository;

        public GetTableFunction(IOptions<FunctionSettings> options, ITableInfoManager tableRepository)
        {
            _telegramIdentifier = options.Value.TelegramIdentifier;
            _chatId = options.Value.TelegramChatIdentifier;
            _tableInfoRepository = tableRepository;
        }

        [FunctionName("GetTable")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            var message = await _tableInfoRepository.GetTableInfoInMessage();
        }
    }
}

