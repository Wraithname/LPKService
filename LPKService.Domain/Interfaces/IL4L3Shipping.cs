using LPKService.Domain.Models.Shipping;
using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    public interface IL4L3Shipping
    {
        L4L3Shipping GetData(TL4MsgInfo l4MsgInfo);
    }
}
