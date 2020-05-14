using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using Work.Models;
using System.Data;

namespace SOM.Repo
{
    interface ISOManagment
    {
        bool CheckInquiryLinesStatus(int iMsgCounter);
        bool OrderExist(string sSoDescrID);
        bool OrderExistLike(string sSoDescrID);
        bool OrderCanBeProcess(string sSoDescrID,int iOpCode,string sErrMsg);
        bool LineIsSalesApproval(int iSoID,int iSoLineID);
        bool ReadyForClosing(int iSoID, int iSoLineID);
        int ManageShipTo(TL4MsgInfo l4MsgInfo,TL4EngineInterfaceMng eimOrderEntry);
        TCheckResult SalesOrderMng(L4L3SoHeader soHeader,TL4MsgInfo l4MsgInfo);
        bool ExistCustomer(string m_iCustSoldDescrID);
        void CreateNewOrder(DataSet QryData, TL4MsgInfo l4MsgInfo);
        bool AlreadyInsertInSuspended(int iSoID);
        TDeleteResponse DeleteOrder(int iOrderID);
        string ProductTypeCheck(string l4ProductType);
        void UpdateOrder(DataSet QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false);
        void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO, List<string> strArrayOfAttributes);
        int RetrievePeriodNumID(DateTime dDeliveryDate, int iPeriodCode, string sPeriodID, DateTime dStopDate);
        int RetrieveShipToCode(int iCustomerId, int iShiptoId);
        void SetOEHeaderValues(TSoHeader order, TL4EngineInterfaceMng eimOrderEntry, bool bUpdatingOrder = false);
    }
}
