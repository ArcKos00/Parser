namespace Parser.Repos.SQLConnect.Entity
{
    /// <summary>
    /// сутність комплектації.
    /// </summary>
    public class ComplectationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AttributeEntity> Attributes { get; set; } = new List<AttributeEntity>();
    }
}
