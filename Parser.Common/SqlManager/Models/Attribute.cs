namespace Parser.Common.SqlManager.Models
{
    /// <summary>
    /// Модель динамічних атрибутів, для вільного використання між слоями.
    /// </summary>
    public class Attribute
    {
        public string ColumnName { get; set; } = null!;
        public string ColumnValue { get; set; } = null!;
    }
}
