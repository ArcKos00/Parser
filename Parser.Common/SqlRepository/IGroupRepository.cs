using Parser.Common.Shared;
using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Відправляє дані про групу до бд.
        /// </summary>
        /// <param name="name">назва групи.</param>
        /// <returns>Обєкт з айді сутності.</returns>
        public Task<ItemId> SendGroup(string name);

        /// <summary>
        /// повертає список груп по id комплектації.
        /// </summary>
        /// <param name="id">id комплектації.</param>
        /// <returns>список груп.</returns>
        public Task<IEnumerable<Group>> GetGroups(int id);
    }
}
