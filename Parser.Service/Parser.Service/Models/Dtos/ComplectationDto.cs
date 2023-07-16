namespace Parser.Service.Models.Dtos
{
    public class ComplectationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AttributesDto> Attributes { get; set; } = new List<AttributesDto>();
    }
}
