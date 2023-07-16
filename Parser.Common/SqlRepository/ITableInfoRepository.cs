using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface ITableInfoRepository
    {
        public Task<IEnumerable<TableInfo>> GetTableTop();
    }
}
