namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель комплектацій, для вільного використання між слоями.
    /// </summary>
    public class Complectation
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    }
}
