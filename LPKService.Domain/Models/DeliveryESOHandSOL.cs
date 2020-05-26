
namespace LPKService.Domain.Models
{
    public class DeliveryESOHandSOL
    {
        public int msgCounter { get; set; }
        public int opCode { get; set; }
        public string bolId { get; set; }
        public int bolPositionId { get; set; }
        public string soId { get; set; }
        public string soLineId { get; set; }
        public float entryQnt { get; set; }
        public int soIdMet { get; set; }
        public int soLineIdMet { get; set; }
    }
}
