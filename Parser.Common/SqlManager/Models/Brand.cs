namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель брендів, для вільного використання між слоями.
    /// </summary>
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
