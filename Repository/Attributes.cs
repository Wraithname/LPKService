using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ColumnAttribute:Attribute
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }

        public ColumnAttribute()
        { }

        public ColumnAttribute(string columnName, string displayName)
        {
            ColumnName = columnName;
            DisplayName = displayName;
        }

        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
            DisplayName = columnName;
        }
    }
}
