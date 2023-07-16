namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель підгрупи, для вільного використання між слоями.
    /// </summary>
    public class SubGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
