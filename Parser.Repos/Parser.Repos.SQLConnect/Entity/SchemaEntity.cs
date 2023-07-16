namespace Parser.Repos.SQLConnect.Entity
{
    /// <summary>
    /// сутність схеми.
    /// </summary>
    public class SchemaEntity
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public List<DetailEntity> Details { get; set; } = new List<DetailEntity>();
    }
}
