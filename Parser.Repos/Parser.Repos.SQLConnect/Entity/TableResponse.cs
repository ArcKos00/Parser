namespace Parser.Repos.SQLConnect.Entity
{
    internal class TableResponse
    {
        public string SchemaName { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public long RowCount { get; set; }
        public long TableSizeKB { get; set; }
    }
}