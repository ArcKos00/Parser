namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про деталь.
    /// </summary>
    internal class SpareDetailPageEntity
    {
        public string DetailCode { get; set; } = null!;
        public string DetailName { get; set; } = null!;
        public List<DetailEntity> Details { get; set; } = new List<DetailEntity>();
    }
}
