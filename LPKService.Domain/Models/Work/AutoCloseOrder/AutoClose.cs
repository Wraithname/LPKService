namespace LPKService.Domain.Models.Work.AutoCloseOrder
{
    public class AutoClose
    {
        public int msgCounter { get;}
        public int soId { get; }
        public int soLineId { get; }
        public int orderStatus { get; }
        public int soTypeCode { get; }
        public int metSoId { get; }
        public int metSoLineId { get; }
    }
}
