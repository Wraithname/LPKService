using System;

namespace LPKService.Domain.Models.Work
{
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
