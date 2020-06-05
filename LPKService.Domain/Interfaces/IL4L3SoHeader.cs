using LPKService.Domain.Models.SOM;
using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса L4L3SoHeaderRepo
    /// </summary>
    public interface IL4L3SoHeader
    {
        L4L3SoHeader GetData(TL4MsgInfo l4MsgInfo);
    }
}
