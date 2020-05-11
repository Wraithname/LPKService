using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace SOM.Models
{
    class Country
    {
        [Column("COUNTRY", "Страна")]
        string countr { get; set; }
        [Column("COUNTRY_CODE", "Код страны")]
        string countrcode { get; set; }
        [Column("COUNTRY_ON_DOC", "Код страны в документе")]
        string countrondoc { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        int moduserid { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        DateTime modDatetime { get; set; }
    }
}
