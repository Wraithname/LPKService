using LPKService.Domain.Models;

namespace LPKService.Domain.Interfaces
{
    public interface IL4L3SoHeader
    {
        L4L3SoHeader GetData(TL4MsgInfo l4MsgInfo);
    }
}
