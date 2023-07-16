namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель групи, для вільного використання між слоями.
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
