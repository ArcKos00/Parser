using Parser.Utils.Shared;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace Parser.Common.Parser.Blob
{
    public class Blob : IBlob
    {
        private readonly CloudBlobContainer container;

        
        public async Task SaveStreamAsync(string blobName, Stream stream)
        {
            if (stream != null)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                stream.Position = 0;
                await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);
            }
        }

        public Blob(IOptions<AppSettings> options)
        {
            if (options.Value.BlobContainerName == null)
                throw new ArgumentNullException("container");

            CloudStorageAccount sa = CloudStorageAccount.Parse(options.Value.BlobConnectionString);
            CloudBlobClient blobClient = sa.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(1);

            container = blobClient.GetContainerReference(options.Value.BlobContainerName);
        }

        public bool Exist(string blobName)
        {
            return container.GetBlockBlobReference(blobName).Exists();
        }
    }
}
