namespace Parser.Repos.SQLConnect.Entity
{
    /// <summary>
    /// сутність моделі.
    /// </summary>
    public class ModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Complectations { get; set; } = null!;
    }
}
