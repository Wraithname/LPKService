using SOM.Models;
using Work.Models;
using Dapper.Oracle;

namespace SOM.Repo
{
    interface ITSoHeader
    {
        TSoLine GetLine(int iIndex, TSoHeader ret);
        string GetCustIDFromL4CustID(int l4CustID, OracleDynamicParameters odp = null);
        bool ExistCustomer(string m_iCustSoldDescrID, OracleDynamicParameters odp = null);
        void Add(TSoLine lines, TSoHeader ret);
        TSoHeader Create(TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry, int iShipToCode,bool bVerifyData, bool bisUpdate = false);
        string GetLineMsgStatus(TSoHeader ret);
        void LinesUpdateMsgStatus(TSoHeader ret);
        bool CheckLineNumeration(int iCounter, OracleDynamicParameters odp = null);
    }
}
