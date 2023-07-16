namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про підгрупу деталей.
    /// </summary>
    public class SubGroupPage
    {
        public string Name { get; set; } = null!;
        public string NextUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
