namespace Parser.Service.Models.Dtos
{
    public class ModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Complectations { get; set; } = null!;
    }
}
