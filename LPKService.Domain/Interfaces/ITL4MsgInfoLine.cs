using LPKService.Domain.Models.Work;
namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса TL4MsgInfoLine
    /// </summary>
    public interface ITL4MsgInfoLine
    {
        void UpdateMsgStatus(TL4MsgInfo l4MsgInfo);
    }
}
