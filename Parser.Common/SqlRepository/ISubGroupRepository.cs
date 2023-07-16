using Parser.Common.Shared;
using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface ISubGroupRepository
    {
        /// <summary>
        /// Відправляє дані про підгрупу до бд.
        /// </summary>
        /// <param name="name">назва підгрупи.</param>
        /// <returns>Обєкт з айді сутності.</returns>
        public Task<ItemId> SendSubGroup(string name, int groupId, int complectationId, int imageId);

        /// <summary>
        /// повертає список підгруп по id груп.
        /// </summary>
        /// <param name="id">id груп.</param>
        /// <returns>список підгруп.</returns>
        public Task<IEnumerable<SubGroup>> GetSubGroups(int complectationId, int groupId);
    }
}
