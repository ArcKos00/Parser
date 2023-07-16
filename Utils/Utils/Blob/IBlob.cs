namespace Parser.Common.Parser.Blob
{
    public interface IBlob
    {
        Task SaveStreamAsync(string blobName, Stream stream);
        bool Exist(string blobName);
    }
}
