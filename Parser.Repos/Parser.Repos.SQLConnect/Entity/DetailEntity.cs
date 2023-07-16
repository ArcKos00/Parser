namespace Parser.Repos.SQLConnect.Entity
{
    /// <summary>
    /// сутність деталі.
    /// </summary>
    public class DetailEntity
    {
        public string TreeCode { get; set; } = null!;
        public string TreeName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public List<string>? Codes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Count { get; set; } = null!;
        public string Usings { get; set; } = null!;
    }
}
