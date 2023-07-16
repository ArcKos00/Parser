namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про групу деталей.
    /// </summary>
    internal class SpareGroupPageEntity
    {
        public string Name { get; set; } = null!;
        public string NextUrl { get; set; } = null!;
    }
}
