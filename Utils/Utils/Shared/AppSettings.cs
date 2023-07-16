namespace Parser.Utils.Shared
{
    /// <summary>
    /// Клас для отримання конфігурацій з DI
    /// </summary>
    public class AppSettings
    {
        public string DatabaseConnectionString { get; set; } = null!;
        public string BlobConnectionString { get; set; } = null!;
        public string BlobContainerName { get; set; } = null!;
        public string AzureContainerUrl { get; set; } = null!;
    }
}
