namespace Parser.Common.TableManager
{
    public interface ITableInfoManager
    {
        public Task<string> GetTableInfoInMessage();
    }
}
