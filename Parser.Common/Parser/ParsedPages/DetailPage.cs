namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про деталь.
    /// </summary>
    public class DetailPage
    {
        public string DetailCode { get; set; } = null!;
        public string DetailName { get; set; } = null!;
        public List<Detail> Details { get; set; } = new List<Detail>();
    }
}
