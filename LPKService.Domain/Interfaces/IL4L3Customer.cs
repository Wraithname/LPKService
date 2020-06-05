using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса L4L3CustomerRepo
    /// </summary>
    public interface IL4L3Customer
    {
        L4L3Customer GetData(TL4MsgInfo l4MsgInfo);
    }
}
