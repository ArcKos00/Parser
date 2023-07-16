namespace Parser.Common.Parser.ParsedPages
{
    /// <summary>
    /// Для повернення сутності(блоку) з даними про автівку.
    /// </summary>
    public class CarPage
    {
        public string Name { get; set; } = null!;
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<string> Urls { get; set; } = new List<string>();
    }
}
