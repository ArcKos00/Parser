namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про групу деталей.
    /// </summary>
    public class GroupPage
    {
        public string Name { get; set; } = null!;
        public string NextUrl { get; set; } = null!;
    }
}
