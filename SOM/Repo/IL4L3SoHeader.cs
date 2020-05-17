using Repository.Models;
using SOM.Models;

namespace SOM.Repo
{
    interface IL4L3SoHeader
    {
        L4L3SoHeader GetData(TL4MsgInfo l4MsgInfo);
    }
}
