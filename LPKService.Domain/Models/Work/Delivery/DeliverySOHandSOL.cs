using Repository;

namespace LPKService.Domain.Models.Work.Delivery
{
    /// <summary>
    /// Модель таблицы Delivery union SoHeader and SoLine
    /// </summary>
    public class DeliverySOHandSOL:BaseModel
    {
        [Column("BOL_POSITION_ID")]
        public int bolPositionId { get; set; }
        [Column("VEHUCLE_ID")]
        public string vehicleId { get; set; }
        [Column("BOL_ID")]
        public string bolId { get; set; }
        [Column("SO_ID")]
        public string soId { get; set; }
        [Column("SO_LINE_ID")]
        public string soLineId { get; set; }
        [Column("ENTRY_QNT")]
        public float entryQnt { get; set; }
        [Column("SO_ID_MET")]
        public string soIdMet { get; set; }
        [Column("SO_LINE_ID_MET")]
        public string soLineIdMet { get; set; }
    }
}
