namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель моделі автівки, для вільного використання між слоями.
    /// </summary>
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Complectations { get; set; } = null!;
    }
}
