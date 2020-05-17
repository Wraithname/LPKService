using Shipping.Models;
using Repository.Models;
using System.Collections.Generic;

namespace Shipping.Repo
{
    interface IL4L3Shipping
    {
        L4L3Shipping GetData(TL4MsgInfo l4MsgInfo);
    }
}
