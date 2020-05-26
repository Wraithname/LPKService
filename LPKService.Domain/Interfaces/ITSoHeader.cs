using Dapper.Oracle;
using LPKService.Domain.Models;

namespace LPKService.Domain.Interfaces
{
    public interface ITSoHeader
    {
        TSoLine GetLine(int iIndex);
        string GetCustIDFromL4CustID(int l4CustID, OracleDynamicParameters odp = null);
        bool ExistCustomer(string m_iCustSoldDescrID, OracleDynamicParameters odp = null);
        void Add(TSoLine lines);
        TSoHeader Create(L4L3SoHeader l4l3soheader,TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry, int iShipToCode,bool bVerifyData, bool bisUpdate = false);
        string GetLineMsgStatus(TSoHeader ret);
        void LinesUpdateMsgStatus(TSoHeader ret);
        bool CheckLineNumeration(int iCounter, OracleDynamicParameters odp = null);
        int CheckValue(bool bisValid, TL4MsgInfo l4MsgInfo);
    }
}
