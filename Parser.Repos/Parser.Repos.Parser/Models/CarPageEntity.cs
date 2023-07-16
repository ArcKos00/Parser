namespace Parser.Repos.Parser.Models
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про автівку.
    /// </summary>
    internal class CarPageEntity
    {
        public string Name { get; set; } = null!;
        public List<CarEntity> Cars { get; set; } = new List<CarEntity>();
        public List<string> Urls { get; set; } = new List<string>();
    }
}
