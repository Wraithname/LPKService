using Shipping.Models;
using Repository.WorkModels;

namespace Shipping.Repo
{
    interface IL4L3Shipping
    {
        L4L3Shipping GetData(TL4MsgInfo l4MsgInfo);
    }
}
