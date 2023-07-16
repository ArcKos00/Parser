namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель схеми з деталями, для вільного використання між слоями.
    /// </summary>
    public class Schema
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public List<Detail> Details { get; set; } = new List<Detail>();
    }
}
