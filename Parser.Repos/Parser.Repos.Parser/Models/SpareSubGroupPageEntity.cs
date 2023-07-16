namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про підгрупу деталей.
    /// </summary>
    internal class SpareSubGroupPageEntity
    {
        public string Name { get; set; } = null!;
        public string NextUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
