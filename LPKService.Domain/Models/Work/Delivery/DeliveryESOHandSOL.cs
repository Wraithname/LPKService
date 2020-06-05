using LPKService.Repository;

namespace LPKService.Domain.Models.Work.Delivery
{
    /// <summary>
    /// Модель таблицы L4L3DeliveryEvent union SoHeader and SoLine
    /// </summary>
    public class DeliveryESOHandSOL:BaseModel
    {
        [Column("MSG_COUNTER")]
        public int msgCounter { get; set; }
        [Column("MSG_COUNTER")]
        public int opCode { get; set; }
        [Column("MSG_COUNTER")]
        public string bolId { get; set; }
        [Column("MSG_COUNTER")]
        public int bolPositionId { get; set; }
        [Column("MSG_COUNTER")]
        public string soId { get; set; }
        [Column("MSG_COUNTER")]
        public string soLineId { get; set; }
        [Column("MSG_COUNTER")]
        public float entryQnt { get; set; }
        [Column("MSG_COUNTER")]
        public int soIdMet { get; set; }
        [Column("MSG_COUNTER")]
        public int soLineIdMet { get; set; }
    }
}
