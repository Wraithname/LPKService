using System;
using Repository;

namespace LPKService.Domain.Models.CCM
{
    /// <summary>
    /// Модель таблицы COUNTRY
    /// </summary>
    public class Country:BaseModel
    {
        [Column("COUNTRY", "Страна")]
        public string countr { get; set; }
        [Column("COUNTRY_CODE", "Код страны")]
        public string countrcode { get; set; }
        [Column("COUNTRY_ON_DOC", "Код страны в документе")]
        public string countrondoc { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        public int moduserid { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDatetime { get; set; }
    }
}
