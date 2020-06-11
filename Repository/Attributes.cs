using System;

namespace Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ColumnAttribute:Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ColumnAttribute()
        { }
        /// <summary>
        /// Название колонки модели БД c выводимым названием
        /// </summary>
        /// <param name="columnName">Название колонки</param>
        /// <param name="displayName">Выводимое название колонки</param>
        public ColumnAttribute(string columnName, string displayName)
        {
            ColumnName = columnName;
            DisplayName = displayName;
        }
        /// <summary>
        /// Название колонки модели БД
        /// </summary>
        /// <param name="columnName">Название колонки</param>
        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
            DisplayName = columnName;
        }
    }
}
