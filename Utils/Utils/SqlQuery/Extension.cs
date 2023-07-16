using System.Security.Cryptography;
using System.Text;

namespace Parser.Utils.SqlQuery
{
    public static class Extension
    {
        public static KeyValuePair<string, object> PairedWith(this string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}
