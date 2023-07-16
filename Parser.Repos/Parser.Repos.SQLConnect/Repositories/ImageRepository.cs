using Parser.Utils.SqlQuery;
using Parser.Common.SqlRepository;
using Parser.Repos.SQLConnect.Entity;
using Parser.Utils.Shared;
using Microsoft.Extensions.Options;

namespace Parser.Repos.SQLConnect.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(IOptions<AppSettings> options)
        {
            _connectionString = options.Value.DatabaseConnectionString;
        }

        /// <summary>
        /// відправляє назву фотографії для збереження.
        /// </summary>
        /// <param name="imageName">назва фотографії.</param>
        /// <returns>айді створеного запису фотографії.</returns>
        public async Task<int> SendImage(string imageName)
        {
            var item = await SqlQuery.SendQueryAsync<ItemIdEntity>(
                _connectionString,
                "kpanfilenko_Parser.p_parser_image_insert",
                "imageName".PairedWith(imageName));

            return item.First().Id;
        }
    }
}
