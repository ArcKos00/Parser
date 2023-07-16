namespace Parser.Common.SqlRepository
{
    public interface IImageRepository
    {
        /// <summary>
        /// відправляє назву фотографії для збереження.
        /// </summary>
        /// <param name="imageName">назва фотографії.</param>
        /// <returns>айді створеного запису фотографії.</returns>
        public Task<int> SendImage(string imageName);
    }
}
