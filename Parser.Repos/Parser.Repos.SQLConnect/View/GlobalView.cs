using Parser.Repos.SQLConnect.Entity;

namespace Parser.Repos.SQLConnect.View
{
    public class GlobalView
    {
        public int Id { get; set; }
        public string ComplectationName { get; set; } = null!;
        public DateTime ComplectationStartDate { get; set; }
        public DateTime ComplectationEndDate { get; set; }
        public string ColumnName { get; set; } = null!;
        public string ColumnValue { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string SubGroupName { get; set; } = null!;
        public string ImageName { get; set; } = null!;
        public string TreeCode { get; set; } = null!;
        public string TreeName { get; set; } = null!;
        public int DetailInfoId { get; set; }
        public int InfoId { get; set; }
        public string DetailCode { get; set; } = null!;
        public DateTime DetailStartDate { get; set; }
        public DateTime DetailEndDate { get; set; }
        public int Count { get; set; }
        public string Usings { get; set; } = null!;
    }
}
