using Repository;
namespace LPKService.Domain.Models.Work.Delivery
{
    /// <summary>
    /// Модель объединенных таблиц
    /// </summary>
    public class JoinedModel:BaseModel
    {
        [Column("COUNT(POS_NUM_ID)", "Количество позиций")]
        public int count { get; set; }
        [Column("POS_NUM_ID", "Номер позиции")]
        public int posNumId { get; set; }
    }
}
