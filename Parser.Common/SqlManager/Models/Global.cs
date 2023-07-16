namespace Parser.Common.SqlManager.Models
{
    public class Global
    {
        public string ComplectationName { get; set; } = null!;
        public DateTime ComplectationStartDate { get; set; }
        public DateTime ComplectationEndDate { get; set; }
        public List<Attribute> Attributs { get; set; } = new List<Attribute>();
        public string GroupName { get; set; } = null!;
        public string SubGroupName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string TreeCode { get; set; } = null!;
        public string TreeName { get; set; } = null!;
        public string DetailCode { get; set; } = null!;
        public List<string> DetailChangedCodes { get; set; } = new List<string>();
        public DateTime DetailStartDate { get; set; }
        public DateTime DetailEndDate { get; set; }
        public int Count { get; set; }
        public string Usings { get; set; } = null!;
    }
}
