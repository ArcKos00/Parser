using Parser.Common.Shared;
using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface ICarRepository
    {
        /// <summary>
        /// відправляєє до бд дані автівки.
        /// </summary>
        /// <param name="name">імя автівки.</param>
        /// <param name="code">кодовий ідентифікатор автівки.</param>
        /// <param name="startDate">дата початку виробництва.</param>
        /// <param name="endDate">дата кінця виробництва.</param>
        /// <param name="assembly">строка збірки.</param>
        /// <returns>обєкт з айді.</returns>
        public Task<ItemId> SendCar(string name, string code, DateTime? startDate, DateTime? endDate, string assembly);

        /// <summary>
        /// повертає список моделей по id бренда.
        /// </summary>
        /// <param name="id">id бренда.</param>
        /// <returns>список моделей.</returns>
        public Task<IEnumerable<Model>> GetModels(int id);
    }
}
