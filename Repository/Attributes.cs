using System;

namespace Repository
{
    public class Attributes:Attribute
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public Attributes()
        { }

        public Attributes(string columnName, string displayName)
        {
            ColumnName = columnName;
            DisplayName = displayName;
        }
        public Attributes(string columnName)
        {
            ColumnName = columnName;
            DisplayName = columnName;
        }
    }
}
