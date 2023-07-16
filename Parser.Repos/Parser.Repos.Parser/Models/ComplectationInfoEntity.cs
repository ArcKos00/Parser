namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності з даними про загальні дані про комплектацію.
    /// </summary>
    internal class ComplectationInfoEntity
    {
        public string Complectation { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
