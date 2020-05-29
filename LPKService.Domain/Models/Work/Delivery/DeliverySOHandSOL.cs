namespace LPKService.Domain.Models.Work.Delivery
{
    public class DeliverySOHandSOL
    {
        public int bolPositionId { get; set; }
        public string vehicleId { get; set; }
        public string bolId { get; set; }
        public string soId { get; set; }
        public string soLineId { get; set; }
        public float entryQnt { get; set; }
        public string soIdMet { get; set; }
        public string soLineIdMet { get; set; }
    }
}
