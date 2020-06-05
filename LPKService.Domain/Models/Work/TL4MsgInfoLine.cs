
namespace LPKService.Domain.Models.Work
{
    /// <summary>
    /// Класс для обработки строк заказов
    /// </summary>
    public class TL4MsgInfoLine
    {
      public TL4MsgInfo tL4MsgInfo { get; set; }
        public string sSoDescrIDInfoLine { get; set; }
        public int iSOLineID { get; set; }
    }
}
