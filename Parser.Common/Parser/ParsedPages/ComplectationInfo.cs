namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності з даними про загальні дані про комплектацію.
    /// </summary>
    public class ComplectationInfo
    {
        public string Complectation { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
