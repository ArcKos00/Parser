using System.Text;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Common.TableManager;

namespace Parser.Business.Parser.Manager
{
    public class TableInfoManager : ITableInfoManager
    {
        private readonly ITableInfoRepository _tableInfoRepository;
        public TableInfoManager(ITableInfoRepository tableInfoRepository)
        {
            _tableInfoRepository = tableInfoRepository;
        }

        public async Task<string> GetTableInfoInMessage()
        {
            var response = await _tableInfoRepository.GetTableTop();
            return BuildMessage(response);
        }

        private string BuildMessage(IEnumerable<TableInfo> response)
        {
            var stringBuilder = new StringBuilder();
            var counter = 0;
            foreach (var tableResponse in response)
            {
                counter++;
                stringBuilder.Append($"{counter}. {tableResponse.SchemaName}.{tableResponse.TableName} RowCount: {tableResponse.RowCount} rows  Size: {(double)tableResponse.TableSizeKB / 1_000_000} GB {Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }
    }
}
