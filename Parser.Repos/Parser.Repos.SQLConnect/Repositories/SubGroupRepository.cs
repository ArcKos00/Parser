using Parser.Utils.SqlQuery;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Common.Shared;
using Parser.Repos.SQLConnect.Entity;
using Parser.Utils.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class SubGroupRepository : ISubGroupRepository
    {
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        public SubGroupRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// Відправляє дані про підгрупу до бд.
        /// </summary>
        /// <param name="name">назва підгрупи.</param>
        /// <returns>Обєкт з айді сутності.</returns>
        public async Task<ItemId> SendSubGroup(string name, int groupId, int complectationId, int imageId)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_subGroup_insert_page",
                "name".PairedWith(name),
                "groupId".PairedWith(groupId),
                "complectationId".PairedWith(complectationId),
                "imageId".PairedWith(imageId));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// повертає список підгруп по id груп.
        /// </summary>
        /// <param name="complectationId">id комплектації.</param>
        /// <returns>список підгруп.</returns>
        public async Task<IEnumerable<SubGroup>> GetSubGroups(int complectationId, int groupId)
        {
            var result = await SqlQuery.SendQueryAsync<SubGroupEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_subGroups_getSubGroups",
                "complectationId".PairedWith(complectationId),
                "groupId".PairedWith(groupId));

            return _mapper.Map<IEnumerable<SubGroup>>(result);
        }
    }
}
