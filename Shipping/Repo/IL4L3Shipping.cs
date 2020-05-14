using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.Models;
using Work.Models;

namespace Shipping.Repo
{
    interface IL4L3Shipping
    {
        L4L3Shipping GetData(TL4MsgInfo l4MsgInfo);
    }
}
