using Parser.Common.Shared;
using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface IDetailRepository
    {
        /// <summary>
        /// Відправляє дані про деталь до бд.
        /// </summary>
        /// <param name="codeTree">код дерева деталі.</param>
        /// <param name="nameTree">назва дерева деталі.</param>
        /// <param name="imageName">назва повязаної фотографії.</param>
        /// <param name="previousId">айді батьківського елементу.</param>
        /// <returns>Обєкт з айді елементу.</returns>
        public Task<ItemId> SendDetail(string codeTree, string nameTree, int previousId);

        /// <summary>
        /// Відправляє інформацію про деталі.
        /// </summary>
        /// <param name="count">кількість деталей.</param>
        /// <param name="startDate">дата початку випуску деталі.</param>
        /// <param name="endDate">дата закінчення випуску деталі.</param>
        /// <param name="info">інформація щодо деталі.</param>
        /// <returns>Обєкт з айді елементу.</returns>
        public Task<ItemId> SendDetailInfo(int count, DateTime? startDate, DateTime? endDate, string info);

        /// <summary>
        /// Відправляє повязані коди для деталей до бд.
        /// </summary>
        /// <param name="code">код деталі.</param>
        /// <param name="detailId">айді деталі.</param>
        /// <param name="infoForCodeId">айді інформації про деталь.</param>
        /// <returns>Task.</returns>
        public Task SendDetailCode(string code, int infoId, int infoForCodeId);

        /// <summary>
        /// повертає список деталей по id підгрупи.
        /// </summary>
        /// <param name="id">id підгрупи.</param>
        /// <returns>список деталей.</returns>
        public Task<Schema> GetSchema(int complectationId, int groupId, int subGroupId);
    }
}
