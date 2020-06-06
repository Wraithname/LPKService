using LPKService.Domain.BaseRepository;

namespace LPKService.Domain.Models.Work.AutoCloseOrder
{
    /// <summary>
    /// Модель таблицы для закрытия заказа
    /// </summary>
    public class AutoClose:BaseModel
    {
        [Column("MSG_COUNTER")]
        public int msgCounter { get;}
        [Column("SO_ID")]
        public int soId { get; }
        [Column("SO_LINE_ID")]
        public int soLineId { get; }
        [Column("ORDER_STATUS")]
        public int orderStatus { get; }
        [Column("SO_TYPE_CODE")]
        public int soTypeCode { get; }
        [Column("MET_SO_ID")]
        public int metSoId { get; }
        [Column("MET_SO_LINE_ID")]
        public int metSoLineId { get; }
    }
}
