namespace Parser.Repos.SQLConnect.View
{
    internal class ComplectationView
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ColumnName { get; set; } = null!;
        public string ColumnValue { get; set; } = null!;
    }
}
