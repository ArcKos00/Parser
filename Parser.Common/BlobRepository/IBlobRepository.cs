namespace Parser.Common.BlobRepository
{
    public interface IBlobRepository
    {
        /// <summary>
        /// Додає до хмари фотографію за посиланням та повертає назву файлу.
        /// </summary>
        /// <param name="url">посилання на файл для збереження.</param>
        /// <returns>назву файлу</returns>
        public Task<string> AddBlob(string url);
    }
}
