using System;

namespace LPKService.Domain.BaseRepository
{
    public class ColumnAttribute:Attribute
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }

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
