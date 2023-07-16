using Parser.Utils.SqlQuery;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;
using Parser.Common.Shared;
using Parser.Repos.SQLConnect.Entity;
using Parser.Utils.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;
using Parser.Repos.SQLConnect.View;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class ComplectationRepository : IComplectationRepository
    {
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        public ComplectationRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// Відправляє дані комплектації для запису у бд.
        /// </summary>
        /// <param name="complectation">строковий ідентифікатор комплектації.</param>
        /// <param name="startDate">дата початку випуску.</param>
        /// <param name="endDate">дата кінця випуску.</param>
        /// <param name="carId">айді автівки до якої відноситься комплектація.</param>
        /// <returns>обєкт з створеним айді.</returns>
        public async Task<ItemId> SendComplectation(string complectation, DateTime? startDate, DateTime? endDate, int carId)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_complectationPage_insert_complectation",
                "complectation".PairedWith(complectation!),
                "startDate".PairedWith(startDate!),
                "endDate".PairedWith(endDate!),
                "carId".PairedWith(carId));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// Відправляє дані для запису про атрібут до бд.
        /// </summary>
        /// <param name="name">навзва атрібуту.</param>
        /// <param name="value">значення атрібуту.</param>
        /// <returns>обєкт з створеним айді.</returns>
        public async Task<ItemId> SendAttributes(string name, string value)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_complectationPage_insert_attribute",
                "name".PairedWith(name),
                "value".PairedWith(value));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// Відбравляє здані для створення пари атрибута та комплектації до бд.
        /// </summary>
        /// <param name="attributeId">айді атрибута.</param>
        /// <param name="complectationId">айді комплектації.</param>
        /// <returns>Task.</returns>
        public async Task SendPairAttribute_Complectation(int attributeId, int complectationId)
        {
            await SqlQuery.SendQueryAsync(
                _connectionString,
                "kpanfilenko_Parser.p_parser_complectationPage_insert_attribute_complectation",
                "attributeId".PairedWith(attributeId),
                "complectationId".PairedWith(complectationId));
        }

        /// <summary>
        /// повертає список комплектацій по id моделі.
        /// </summary>
        /// <param name="id">id моделі.</param>
        /// <returns>список комплектацій.</returns>
        public async Task<IEnumerable<Complectation>> GetComplectations(int id)
        {
            var complectations = new List<ComplectationEntity>();
            var items = await SqlQuery.SendQueryAsync<ComplectationView>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_complectations_getComplectations",
                "carId".PairedWith(id));

            foreach (var complectationData in items.DistinctBy(c => c.Id))
            {
                var attributes = new List<AttributeEntity>();
                foreach (var item in items.Where(w => w.Id == complectationData.Id))
                {
                    attributes.Add(new AttributeEntity
                    {
                        ColumnName = item.ColumnName,
                        ColumnValue = item.ColumnValue
                    });
                }

                complectations.Add(new ComplectationEntity
                {
                    Id = complectationData.Id,
                    StarDate = complectationData.StarDate,
                    EndDate = complectationData.EndDate,
                    Name = complectationData.Name,
                    Attributes = attributes
                });
            }

            return _mapper.Map<IEnumerable<Complectation>>(complectations);
        }

        /// <summary>
        /// Отримання усієї пов'язаної інформації до комплектації.
        /// </summary>
        /// <param name="complectationId">айді комплектації.</param>
        /// <returns>список повязаних даних.</returns>
        public async Task<IEnumerable<Global>> GetFullInfoForComplectation(int complectationId)
        {
            var globals = new List<GlobalEntity>();
            var items = await SqlQuery.SendQueryAsync<GlobalView>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_globalView_get",
                "complectationId".PairedWith(complectationId));
            var attributes = new List<AttributeEntity>();

            foreach (var globalView in items.DistinctBy(d => d.ColumnName))
            {
                attributes.Add(new AttributeEntity
                {
                    ColumnName = globalView.ColumnName,
                    ColumnValue = globalView.ColumnValue
                });
            }

            foreach (var item in items)
            {
                var codes = new List<string>();

                foreach (var detail in items.DistinctBy(d => d.InfoId))
                {
                    if (detail.DetailInfoId == detail.InfoId && !codes.Contains(detail.DetailCode))
                    {
                        codes.Add(detail.DetailCode);
                    }
                }

                globals.Add(new GlobalEntity
                {
                    ComplectationName = item.ComplectationName,
                    ComplectationStartDate = item.ComplectationStartDate,
                    ComplectationEndDate = item.ComplectationEndDate,
                    Attributs = attributes,
                    GroupName = item.GroupName,
                    SubGroupName = item.SubGroupName,
                    ImageName = item.ImageName,
                    TreeCode = item.TreeCode,
                    TreeName = item.TreeName,
                    DetailCode = codes[0],
                    DetailChangedCodes = codes.Count > 1 ? codes.Skip(1).ToList() : new List<string>(),
                    DetailStartDate = item.DetailStartDate,
                    DetailEndDate = item.DetailEndDate,
                    Count = item.Count,
                    Usings = item.Usings
                });
            }

            return _mapper.Map<IEnumerable<Global>>(globals);
        }
    }
}
