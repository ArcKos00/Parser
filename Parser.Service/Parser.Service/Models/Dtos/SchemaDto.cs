namespace Parser.Service.Models.Dtos
{
    public class SchemaDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public List<DetailDto> Details { get; set; } = new List<DetailDto>();
    }
}
