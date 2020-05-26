using System;

namespace LPKService.Repository
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
