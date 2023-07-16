using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlManager
{
    public interface IGetSqlManager
    {
        /// <summary>
        /// Отримати усі бренди.
        /// </summary>
        /// <returns>список брендів.</returns>
        public Task<List<Brand>> GetBrands();

        /// <summary>
        /// Отримати усі моделі стосовно бренда.
        /// </summary>
        /// <returns>список моделей.</returns>
        public Task<List<Model>> GetModels(int id);

        /// <summary>
        /// Отримати усі комплектації стосовно моделі.
        /// </summary>
        /// <returns>список комплектацій.</returns>
        public Task<List<Complectation>> GetComplectations(int id);

        /// <summary>
        /// Отримати усі групи стосовно комплектації.
        /// </summary>
        /// <returns>список груп.</returns>
        public Task<List<Group>> GetGroups(int id);

        /// <summary>
        /// Отримати усі підгрупи стосовно групи.
        /// </summary>
        /// <returns>список підгруп.</returns>
        public Task<List<SubGroup>> GetSubGroups(int complectationId, int groupId);

        /// <summary>
        /// Отримати усі схеми деталей стосовно підгрупи.
        /// </summary>
        /// <returns>список схем деталей.</returns>
        public Task<Schema> GetSpares(int ComplectationId, int groupId, int subGroupId);

        /// <summary>
        /// Отримати усі повязані дані до комплектаціх.
        /// </summary>
        /// <param name="id">айді комплектації.</param>
        /// <returns>список повязаних даних.</returns>
        public Task<List<Global>> GetFullDependDataForComplectation(int id);
    }
}
