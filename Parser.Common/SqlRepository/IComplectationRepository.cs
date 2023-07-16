using Parser.Common.Shared;
using Parser.Common.SqlManager.Models;
using Attribute = Parser.Common.SqlManager.Models.Attribute;

namespace Parser.Common.SqlRepository
{
    public interface IComplectationRepository
    {
        /// <summary>
        /// Відправляє дані комплектації для запису у бд.
        /// </summary>
        /// <param name="complectation">строковий ідентифікатор комплектації.</param>
        /// <param name="startDate">дата початку випуску.</param>
        /// <param name="endDate">дата кінця випуску.</param>
        /// <param name="carId">айді автівки до якої відноситься комплектація</param>
        /// <returns>обєкт з створеним айді.</returns>
        public Task<ItemId> SendComplectation(string complectation, DateTime? startDate, DateTime? endDate, int carId);

        /// <summary>
        /// Відправляє дані для запису про атрібут до бд.
        /// </summary>
        /// <param name="name">навзва атрібуту.</param>
        /// <param name="value">значення атрібуту.</param>
        /// <returns>обєкт з створеним айді.</returns>
        public Task<ItemId> SendAttributes(string name, string value);

        /// <summary>
        /// Відбравляє здані для створення пари атрибута та комплектації до бд.
        /// </summary>
        /// <param name="attributeId">айді атрибута.</param>
        /// <param name="complectationId">айді комплектації.</param>
        /// <returns>Task.</returns>
        public Task SendPairAttribute_Complectation(int attributeId, int complectationId);

        /// <summary>
        /// повертає список комплектацій по id моделі.
        /// </summary>
        /// <param name="id">id моделі.</param>
        /// <returns>список комплектацій.</returns>
        public Task<IEnumerable<Complectation>> GetComplectations(int id);

        /// <summary>
        /// Отримання усієї пов'язаної інформації до комплектації.
        /// </summary>
        /// <param name="complectationId">айді комплектації.</param>
        /// <returns>список повязаних даних.</returns>
        public Task<IEnumerable<Global>> GetFullInfoForComplectation(int complectationId);
    }
}
