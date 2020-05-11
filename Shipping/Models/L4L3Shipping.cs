using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Shipping.Models
{
    public class L4L3Shipping:BaseModel
    {
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("PIECE_ID", "Номер изделия")]
        public string pieceId { get; set; }
        [Column("BOL_ID", "Номер накладной")]
        public string bolId { get; set; }
        [Column("BOL_STATUS", "Статус накладной")]
        public int bolStatus { get; set; }
        [Column("SO_ID", "ИД заказа")]
        public string soId { get; set; }
        [Column("SO_LINE_ID", "ИД линии заказа")]
        public string soLineId { get; set; }
        [Column("SHIP_DATE", "Дата отгрузки")]
        public DateTime shipDate {get; set;}
    }
}
