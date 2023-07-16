using System.Data;
using Parser.Utils.SqlQuery;
using Parser.Common.Parser.ParsedPages;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Common.Shared;
using Parser.Repos.SQLConnect.Entity;
using Parser.Utils.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        public GroupRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// Відправляє дані про групу до бд.
        /// </summary>
        /// <param name="name">назва групи.</param>
        /// <returns>Обєкт з айді сутності.</returns>
        public async Task<ItemId> SendGroup(string name)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_group_insert_page",
                "name".PairedWith(name));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// повертає список груп по id комплектації.
        /// </summary>
        /// <param name="id">id комплектації.</param>
        /// <returns>список груп.</returns>
        public async Task<IEnumerable<Group>> GetGroups(int id)
        {
            var result = await SqlQuery.SendQueryAsync<GroupEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_groups_getGroups",
                "complectationId".PairedWith(id));

            return _mapper.Map<IEnumerable<Group>>(result);
        }
    }
}
