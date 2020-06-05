using Dapper.Oracle;
using LPKService.Domain.Models.Work;
using LPKService.Domain.Models.SOM;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса TSoHeaderRepo
    /// </summary>
    public interface ITSoHeader
    {
        TSoLine GetLine(int iIndex);
        string GetCustIDFromL4CustID(int l4CustID, OracleDynamicParameters odp = null);
        bool ExistCustomer(string m_iCustSoldDescrID, OracleDynamicParameters odp = null);
        void Add(TSoLine lines);
        TSoHeader Create(L4L3SoHeader l4l3soheader,TL4MsgInfo l4MsgInfo, int iShipToCode,bool bVerifyData, bool bisUpdate = false);
        string GetLineMsgStatus(TSoHeader ret);
        void LinesUpdateMsgStatus(TSoHeader ret);
        bool CheckLineNumeration(int iCounter, OracleDynamicParameters odp = null);
        int CheckValue(bool bisValid, TL4MsgInfo l4MsgInfo);
    }
}
