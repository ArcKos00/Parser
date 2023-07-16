namespace Parser.Utils.SqlQuery
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreBind : Attribute
    {
        public IgnoreBind(Action a)
        {
            Action = a;
        }
        public Action Action { get; set; }
    }

    public enum Action
    {
        Request,
        RecordSet,
    }
}
