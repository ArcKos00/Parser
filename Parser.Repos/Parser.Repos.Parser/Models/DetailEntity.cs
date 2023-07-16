namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про деталь.
    /// </summary>
    internal class DetailEntity
    {
        public List<string> Codes { get; set; } = new List<string>();
        public int Count { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Usings { get; set; } = null!;
    }
}
