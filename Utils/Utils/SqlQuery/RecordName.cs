namespace Parser.Utils.SqlQuery
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RecordName : Attribute
    {
        public RecordName(string _name)
        {
            Name = _name;
        }

        public string Name { get; set; }
    }
}
