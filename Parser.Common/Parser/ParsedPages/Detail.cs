namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про деталь.
    /// </summary>
    public class Detail
    {
        public List<string> Codes { get; set; } = new List<string>(2);
        public int Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Usings { get; set; } = null!;
    }
}
