using SOM.Models;
using Repository.Models;

namespace SOM.Repo
{
    interface ITSoLine
    {
        TSoLine Create(LinesCom line,TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate);
        void UpdateMsgStatus(TL4MsgInfo l4MsgInfo);
        string GetMsgStatus(TL4MsgInfoLine l4MsgInfo);
        int CheckValueType(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        int CheckValueCredit(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        int CheckValueProductType(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        TLineStatus GetLineStatus(int linestatus);
    }
}
