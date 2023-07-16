namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про одну збірку автівки.
    /// </summary>
    internal class CarEntity
    {
        public string Code { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Assembly { get; set; } = null!;
    }
}
