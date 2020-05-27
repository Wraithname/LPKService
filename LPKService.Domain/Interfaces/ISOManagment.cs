using System;
using System.Collections.Generic;
using LPKService.Domain.Models;
using System.Data;
using static LPKService.Domain.Models.TSoHeader;

namespace LPKService.Domain.Interfaces
{
    public interface ISOManagment
    {
        bool CheckInquiryLinesStatus(int iMsgCounter);
        bool OrderExist(string sSoDescrID);
        bool OrderExistLike(string sSoDescrID);
        bool OrderCanBeProcess(string sSoDescrID, int iOpCode, string sErrMsg);
        bool LineIsSalesApproval(int iSoID, int iSoLineID);
        bool ReadyForClosing(int iSoID, int iSoLineID);
        int ManageShipTo(TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry);
        TCheckResult SalesOrderMng(TL4MsgInfo l4MsgInfo);
        bool ExistCustomer(string m_iCustSoldDescrID);
        void CreateNewOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo);
        bool AlreadyInsertInSuspended(int iSoID);
        TDeleteResponse DeleteOrder(int iOrderID);
        string ProductTypeCheck(string l4ProductType);
        void UpdateOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false);
        void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO, List<string> strArrayOfAttributes);
        int RetrievePeriodNumID(DateTime dDeliveryDate, int iPeriodCode, string sPeriodID = "", DateTime dStopDate = new DateTime());
        int RetrieveShipToCode(int iCustomerId, int iShiptoId);
        void SetOEHeaderValues(TSoHeader order, TL4EngineInterfaceMng eimOrderEntry, bool bUpdatingOrder = false);
        void SetOELinesValues(TSoLine order, bool bReordered = false);
        bool CompareHeaderValues(TSoHeader objsoHeader, TL4EngineInterfaceMng eimOrderEntry);
        bool CompareLineValues(int iLineId, TL4EngineInterfaceMng eimOrderEntry);
        bool CompareLines(TSoHeader objsoHeader, int iNumOfLine, TL4EngineInterfaceMng eimOrderEntry);
        void UpdateCreditStatus(TSoHeader objsoHeader, int iLineId);
        void UpdateHeaderFields(TSoHeader objsoHeader);
        void UpdateLineFields(TSoHeader objsoHeader);
        void CloseLine(int iSoId, int iSoLineId, int iSoTypeCode);
        void AddVsw_detailsToOrder(TL4MsgInfo l4MsgInfo);
        bool CompareAttributes(TCheckRelatedList chlAttrbListFromSap, TCheckRelatedList chlAttrbListFromDB, List<string> strAttrbCodesFromSap, TL4EngineInterfaceMng eimOrderEntry, bool mandatory = true);
        bool IsCustomerInternal(int iCustomerId);
        void CheckUpdateOPCODE(TL4MsgInfo l4MsgInfo);
        TCheckResult AttributeCheck(L4L3SoHeader qryData, TL4MsgInfo l4MsgInfo);
        void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer);
        ContractType DecodeContractType(string soid, string strCustomerId);
    }
}
