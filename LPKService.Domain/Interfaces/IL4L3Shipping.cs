using LPKService.Domain.Models;

namespace LPKService.Domain.Interfaces
{
    public interface IL4L3Shipping
    {
        L4L3Shipping GetData(TL4MsgInfo l4MsgInfo);
    }
}
