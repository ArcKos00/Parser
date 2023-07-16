using Parser.Utils.SqlQuery;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Repos.SQLConnect.Entity;
using Parser.Utils.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public BrandRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _connectionString = options.Value.DatabaseConnectionString;
            _mapper = mapper;
        }

        /// <summary>
        /// Повертає список брендів.
        /// </summary>
        /// <returns>список брендів.</returns>
        public async Task<IEnumerable<Brand>> GetBrands()
        {
            var result = await SqlQuery.SendQueryAsync<BrandEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_brand_getBrands");

            return _mapper.Map<IEnumerable<Brand>>(result);
        }
    }
}
