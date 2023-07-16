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
    public class DetailRepository : IDetailRepository
    {
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        public DetailRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// Відправляє дані про деталь до бд.
        /// </summary>
        /// <param name="codeTree">код дерева деталі.</param>
        /// <param name="nameTree">назва дерева деталі.</param>
        /// <param name="previousId">айді батьківського елементу.</param>
        /// <returns>Обєкт з айді елементу.</returns>
        public async Task<ItemId> SendDetail(string codeTree, string nameTree, int previousId)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_detail_insert_page",
                "codeTree".PairedWith(codeTree),
                "nameTree".PairedWith(nameTree),
                "groupSubGroupComplectationId".PairedWith(previousId));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// Відправляє інформацію про деталі.
        /// </summary>
        /// <param name="count">кількість деталей.</param>
        /// <param name="startDate">дата початку випуску деталі.</param>
        /// <param name="endDate">дата закінчення випуску деталі.</param>
        /// <param name="info">інформація щодо деталі.</param>
        /// <returns>Обєкт з айді елементу.</returns>
        public async Task<ItemId> SendDetailInfo(int count, DateTime? startDate, DateTime? endDate, string info)
        {
            var item = await SqlQuery.SendQueryAsync<ItemId>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_detailInfo_insert_info",
                "count".PairedWith(count),
                "startDate".PairedWith(startDate!),
                "endDate".PairedWith(endDate!),
                "info".PairedWith(info));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// Відправляє повязані коди для деталей до бд.
        /// </summary>
        /// <param name="code">код деталі.</param>
        /// <param name="detailId">айді деталі.</param>
        /// <param name="infoForCodeId">айді інформації про деталь.</param>
        /// <returns>Task.</returns>
        public async Task SendDetailCode(string code, int detailId, int infoForCodeId)
        {
            await SqlQuery.SendQueryAsync(
                _connectionString,
                "kpanfilenko_Parser.p_parser_detailCode_insert_code",
                "code".PairedWith(code),
                "detailId".PairedWith(detailId),
                "infoForCodeId".PairedWith(infoForCodeId));
        }

        /// <summary>
        /// повертає список деталей по id підгрупи.
        /// </summary>
        /// <param name="complectationId">id комплектації.</param>
        /// <param name="groupId">id групи.</param>
        /// <param name="subGroupsId">id підгрупи.</param>
        /// <returns>список деталей.</returns>
        public async Task<Schema> GetSchema(int complectationId, int groupId, int subGroupsId)
        {
            var schemaItem = await SqlQuery.SendQueryAsync<SchemaEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_details_getShemaInfo",
                "complectationId".PairedWith(complectationId),
                "groupId".PairedWith(groupId),
                "subgroupId".PairedWith(subGroupsId));
            var schema = schemaItem.First();

            await GetDetails(schema);

            return _mapper.Map<Schema>(schema);
        }

        private async Task GetDetails(SchemaEntity schema)
        {
            var detailsInfo = await SqlQuery.SendQueryAsync<DetailEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_details_getDetailInfo",
                "comp_gr_subgrId".PairedWith(schema.Id));
            schema.Details = detailsInfo.ToList();
            await GetDetailCodes(schema.Details, schema.Id);
        }

        private async Task GetDetailCodes(List<DetailEntity> details, int schemaId)
        {
            var items = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_details_getDetailIds",
                "comp_gr_sugrId".PairedWith(schemaId));
            var ids = items.ToList();
            for (int i = 0; i < ids.Count; i++)
            {
                var codeItems = await SqlQuery.SendQueryAsync<StringObject>(
                    _connectionString,
                    "kpanfilenko_GetView.p_parser_details_getDetailCodes",
                    "detailId".PairedWith(ids[i].Id));
                var codes = codeItems.ToList();
                details[i].Code = codes[0].Code;
                if (codes.Count > 1)
                {
                    details[i].Codes = new List<string>(codes.Skip(1).Select(s => s.Code));
                }
            }
        }
    }
}
