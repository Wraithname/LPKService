using System;
using LPKService.Repository;

namespace LPKService.Domain.Models.Work.Event
{
    public class L4L3Event
    {
        [Column("KEY_MSG_COUNTER", "Счетчик сообщений")]
        public int keyMsgCounter { get; set; }
        [Column("MSG", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("MSG_ID", "ИД сообщения")]
        public int msgId { get; set; }
        [Column("MSG_DATETIME", "Дата сообщения")]
        public DateTime msgDatetime { get; set; }
        [Column("OP_CODE", "Код операции")]
        public int opCode { get; set; }
        [Column("KEY_STRING_1", "Текстовая часть сообщения 1")]
        public string keyString1 { get; set; }
        [Column("KEY_STRING_2", "Текстовая часть сообщения 2")]
        public string keyString2 { get; set; }
        [Column("KEY_NUMBER_1", "Числовая часть сообщения 1")]
        public int keyNumber1 { get; set; }
        [Column("KEY_NUMBER_2", "Числовая часть сообщения 2")]
        public int keyNumber2 { get; set; }
        [Column("MSG_STATUS", "Статус сообщения")]
        public int status { get; set; }
        [Column("MSG_REMARK", "Примечание по сообщению")]
        public string remark { get; set; }
        [Column("ERP_MSG_ID", "ID события в ERP")]
        public int erpMsgId { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDateTime { get; set; }
        [Column("BLOCK_FOR_PROCESS", "Блокровка для выполнения")]
        public int msgCounterSource { get; set; }
    }
}
