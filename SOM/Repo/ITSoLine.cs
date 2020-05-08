using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using Work.Models;

namespace SOM.Repo
{
    interface ITSoLine
    {
        TSoLine Create(TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate);
        void UpdateMsgStatus(TL4MsgInfo l4MsgInfo);
        string GetMsgStatus(TL4MsgInfoLine l4MsgInfo);
    }
}
