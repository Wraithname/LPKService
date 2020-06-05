using System;
using System.Collections.Generic;
using LPKService.Domain.Models.SOM;
using LPKService.Domain.Models.Work;
using static LPKService.Domain.Models.SOM.TSoHeader;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса SOManagment
    /// </summary>
    public interface ISOManagment
    {
        void AddVsw_detailsToOrder(TL4MsgInfo l4MsgInfo);
        bool AlreadyInsertInSuspended(int iSoID);
        TCheckResult AttributeCheck(L4L3SoHeader qryData, TL4MsgInfo l4MsgInfo);
        void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer);
        bool CheckInquiryLinesStatus(int iMsgCounter);
        TL4MsgInfo CheckUpdateOPCODE(TL4MsgInfo l4MsgInfo);
        void CreateNewOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo);
        void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO = null, List<string> strArrayOfAttributes = null);
        bool OrderCanBeProcess(string sSoDescrID, int iOpCode, string sErrMsg);
        bool OrderExist(string sSoDescrID);
        bool OrderExistLike(string sSodesrID);
        int RetrievePeriodNumID(DateTime date, int num);
        TCheckResult SalesOrderMng(TL4MsgInfo l4MsgInfo);
        void UpdateLineFields(TSoHeader soHeader);
        void UpdateOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false);
    }
}
