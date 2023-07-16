namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про одну збірку автівки.
    /// </summary>
    public class Car
    {
        public string Code { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Assembly { get; set; } = null!;
    }
}
