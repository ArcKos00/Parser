namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про комплектацію.
    /// </summary>
    public class ComplectationPage
    {
        public string Url { get; set; } = null!;
        public ComplectationInfo? Complectation { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}
