using Parser.Common.BlobRepository;
using Parser.Common.Parser.Blob;
using Utils;

namespace Parser.Repos.Blob
{
    public class BlobRepository : IBlobRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBlob _blobContainer;

        public BlobRepository(IBlob blobContainer, IHttpClientFactory factory)
        {
            _clientFactory = factory;
            _blobContainer = blobContainer;
        }

        /// <summary>
        /// Додає до хмари фотографію за посиланням та повертає назву файлу.
        /// </summary>
        /// <param name="url">посилання на файл для збереження.</param>
        /// <returns>назву файлу</returns>
        public async Task<string> AddBlob(string url)
        {
            using (var client = _clientFactory.CreateClient())
            {
                using (var stream = new MemoryStream(await client.GetByteArrayAsync(url)))
                {
                    var hash = await MD5HashCreator.GetImageHash(stream);
                    if (!_blobContainer.Exist(hash))
                    {
                        await _blobContainer.SaveStreamAsync(hash, stream);
                    }

                    return hash;
                }
            }
        }
    }
}
