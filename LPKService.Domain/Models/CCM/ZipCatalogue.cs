using System;
using LPKService.Repository;

namespace LPKService.Domain.Models.CCM
{
    /// <summary>
    /// Модель таблицы ZIP_CATALOGUE
    /// </summary>
    public class ZipCatalogue:BaseModel
    {
        [Column("ZIP_CODE", "Индекс")]
        string zipCode { set; get; }
        [Column("COUNTRY", "Страна")]
        string country { set; get; }
        [Column("CITY", "Город")]
        string city { set; get; }
        [Column("STATE_CODE", "Код области")]
        string stateCode { set; get; }
        [Column("STATE", "Штат(область)")]
        string state { set; get; }
        [Column("DISTANCE", "Расстояние")]
        float distance { set; get; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        int modUserId { set; get; }
        [Column("FREIGHT_ZONE", "Зона доставки")]
        string freightZone { set; get; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        DateTime modDatetime { set; get; }
        [Column("ZONE", "Зона")]
        string zone { set; get; }
    }
}
