﻿
namespace Repository.Models
{
    public class L4L3DelEventDel
    {
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("BOL_ID", "Номер накладной")]
        public string bolId { get; set; }
        [Column("OP_CODE", "Код операции")]
        public int opCode { get; set; }
        [Column("BOL_POSITION_ID", "Позиция в накладной")]
        public int bolPositionId { get; set; }
    }
}