using System;
using LPKService.Repository;

namespace LPKService.Domain.Models
{
    public class L4L3Delivery
    {
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("BOL_ID", "Номер накладной")]
        public string bolId { get; set; }
        [Column("SO_ID", "ИД заказа")]
        public string soId { get; set; }
        [Column("SO_LINE_ID", "ИД линии заказа")]
        public string soLineId { get; set; }
        [Column("MATNR", "Номенклатурный номер")]
        public string mantr { get; set; }
        [Column("ENTRY_QNT", "")]
        public int entryQnt { get; set; }
        [Column("PSTNG_DATE", "Дата создания")]
        public DateTime pstng_date { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDateTime { get; set; }
        [Column("BOL_POSITION_ID", "Позиция в накладной")]
        public int bolPositiomId { get; set; }
        [Column("VEHICLE_ID", "ИД транспортного средства")]
        public string vehicleId { get; set; }
        [Column("AUTO_FLG", "Флаг автоматического занесения")]
        public int autoFlg { get; set; }
    }
}
