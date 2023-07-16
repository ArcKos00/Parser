namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про комплектацію.
    /// </summary>
    internal class ComplectationPageEntity
    {
        public string Url { get; set; } = null!;
        public ComplectationInfoEntity? Complectation { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}
