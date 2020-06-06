namespace LPKService.Domain.Models.SOM
{
    /// <summary>
    /// Программная модель для пометок в строки заказа
    /// </summary>
    public class TLineNote
    {
        public int soId { get; set; }
        public int soLineId { get; set; }
        public string lineNote { get; set; }
        public string sSAPUser { get; set; }
    }
}
