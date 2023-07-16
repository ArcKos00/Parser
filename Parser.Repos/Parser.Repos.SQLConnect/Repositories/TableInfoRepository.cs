using Parser.Utils.SqlQuery;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Repos.SQLConnect.Entity;
using AutoMapper;
using Microsoft.Extensions.Options;
using Utils.Shared;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class TableInfoRepository : ITableInfoRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public TableInfoRepository(IOptions<FunctionSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        public async Task<IEnumerable<TableInfo>> GetTableTop()
        {
            var response = await SqlQuery.SendQueryAsync<TableResponse>(
                _connectionString,
                "kpanfilenko_TableInfo.p_tables_getTableInfo");
            return _mapper.Map<IEnumerable<TableInfo>>(response);
        }
    }
}
