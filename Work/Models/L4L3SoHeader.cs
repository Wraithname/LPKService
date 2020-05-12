using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Work.Models
{
    class L4L3SoHeader
    {
        [Column("KEY_MSG_COUNTER", "Счетчик сообщений")]
        public int keyMsgCounter { get; set; }
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("SO_ID", "ИД заказа")]
        public string soID { get; set; }
        [Column("INSERT_DATE", "Дата вставки")]
        public DateTime insertDate { get; set; }
        [Column("CUSTOMER_ID", "ИД заказчика")]
        public int customerId { get; set; }
        [Column("CUSTOMER_PO", "ИД заказчика")]
        public string customerPo { get; set; }
        [Column("CUSTOMER_PO_DATE", "Дата выполнения заказа для заказчика")]
        public DateTime customerPoDate { get; set; }
        [Column("SO_NOTES", "Дополнения к заказу")]
        public string soNotes { get; set; }
        [Column("INQUIRY_REF_NUMBER", "Ссылка на номер коммерческого предложения")]
        public string inquiryRefNumber { get; set; }
        [Column("INQUIRY_REF_DATE", "Дата ссылки коммерческого предложения")]
        public DateTime inquiryRefDate { get; set; }
        [Column("STATUS", "Статус")]
        public int status { get; set; }
        [Column("HEADER_NOTE", "Дополнения к заказу")]
        public string headerNote { get; set; }
    }
}
