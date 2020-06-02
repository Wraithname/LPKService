using System;
using System.Collections.Generic;
using LPKService.Domain.Models.SOM;
using LPKService.Domain.Models.Work;
using static LPKService.Domain.Models.SOM.TSoHeader;

namespace LPKService.Domain.Interfaces
{
    public interface ISOManagment
    {
        bool CheckInquiryLinesStatus(int iMsgCounter);
        bool OrderExist(string sSoDescrID);
        bool OrderCanBeProcess(string sSoDescrID, int iOpCode, string sErrMsg);
        TCheckResult SalesOrderMng(TL4MsgInfo l4MsgInfo);
        void CreateNewOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo);
        bool AlreadyInsertInSuspended(int iSoID);
        void UpdateOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false);
        void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO, List<string> strArrayOfAttributes); 
        void AddVsw_detailsToOrder(TL4MsgInfo l4MsgInfo);
        TL4MsgInfo CheckUpdateOPCODE(TL4MsgInfo l4MsgInfo);
        TCheckResult AttributeCheck(L4L3SoHeader qryData, TL4MsgInfo l4MsgInfo);
        void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer);
        int RetrievePeriodNumID(DateTime date, int num);
        string ProductTypeCheck(string product);
    }
}
