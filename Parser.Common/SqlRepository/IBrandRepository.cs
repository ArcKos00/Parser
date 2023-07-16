using Parser.Common.SqlManager.Models;

namespace Parser.Common.SqlRepository
{
    public interface IBrandRepository
    {
        /// <summary>
        /// Повертає список брендів.
        /// </summary>
        /// <returns>список брендів.</returns>
        public Task<IEnumerable<Brand>> GetBrands();
    }
}
