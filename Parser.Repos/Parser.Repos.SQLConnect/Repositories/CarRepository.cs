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
    public class CarRepository : ICarRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public CarRepository(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// відправляєє до бд дані автівки.
        /// </summary>
        /// <param name="name">імя автівки.</param>
        /// <param name="code">кодовий ідентифікатор автівки.</param>
        /// <param name="startDate">дата початку виробництва.</param>
        /// <param name="endDate">дата кінця виробництва.</param>
        /// <param name="assembly">строка збірки.</param>
        /// <returns>обєкт з айді.</returns>
        public async Task<ItemId> SendCar(string name, string code, DateTime? startDate, DateTime? endDate, string assembly)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_carPage_insert_page",
                "brand".PairedWith("Toyota"),
                "name".PairedWith(name),
                "code".PairedWith(code),
                "startDate".PairedWith(startDate!),
                "endDate".PairedWith(endDate!),
                "assembly".PairedWith(assembly));

            return _mapper.Map<ItemId>(item.First());
        }

        /// <summary>
        /// повертає список моделей по id бренда.
        /// </summary>
        /// <param name="id">id бренда.</param>
        /// <returns>список моделей.</returns>
        public async Task<IEnumerable<Model>> GetModels(int id)
        {
            var result = await SqlQuery.SendQueryAsync<ModelEntity>(
                _connectionString,
                "kpanfilenko_GetView.p_parser_models_getModels",
                "brandid".PairedWith(id));

            return _mapper.Map<IEnumerable<Model>>(result);
        }
    }
}
