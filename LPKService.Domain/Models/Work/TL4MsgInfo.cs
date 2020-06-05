using System;

namespace LPKService.Domain.Models.Work
{
    /// <summary>
    /// Программная модель таблицы L4_L3_EVENT
    /// </summary>
    public class TL4MsgInfo
    {
        public int msgCounter { get; set; }
        public int msgId { get; set; }
        public DateTime msgDatetime { get; set; }
        public int opCode { get; set; }
        public string keyString1 { get; set; }
        public string keyString2 { get; set; }
        public int keyNumber1 { get; set; }
        public int keyNumber2 { get; set; }
        public TMessageResult msgReport { get; set; }
    }
}
