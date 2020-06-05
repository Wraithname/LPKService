using LPKService.Domain.Models.Shipping;
using LPKService.Domain.Models.Work;
using System.Collections.Generic;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса L4L3ShippingRepo
    /// </summary>
    public interface IL4L3Shipping
    {
        List<L4L3Shipping> GetListData(TL4MsgInfo l4MsgInfo);
    }
}
