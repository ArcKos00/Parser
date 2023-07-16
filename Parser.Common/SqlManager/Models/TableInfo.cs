using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Common.SqlManager.Models
{
    public class TableInfo
    {
        public string SchemaName { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public long RowCount { get; set; }
        public long TableSizeKB { get; set; }
    }
}
