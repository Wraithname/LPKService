using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;
using Work.Models;

namespace SOM
{
    //Используется таблица L4_L3_SO_HEADER
    enum TShiptoType { Shipto,Billto};
    public class SOManagment : ISOManagment
    {
        public bool AlreadyInsertInSuspended(int iSoID)
        {
            throw new NotImplementedException();
        }

        public bool CheckInquiryLinesStatus(int iMsgCounter)
        {
            throw new NotImplementedException();
        }

        public void CreateNewOrder(DataSet QryData, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TDeleteResponse DeleteOrder(int iOrderID)
        {
            throw new NotImplementedException();
        }

        public bool ExistCustomer(string m_iCustSoldDescrID)
        {
            throw new NotImplementedException();
        }

        public bool LineIsSalesApproval(int iSoID, int iSoLineID)
        {
            throw new NotImplementedException();
        }

        public void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO, List<string> strArrayOfAttributes)
        {
            throw new NotImplementedException();
        }

        public int ManageShipTo(TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry)
        {
            throw new NotImplementedException();
        }

        public bool OrderCanBeProcess(string sSoDescrID, int iOpCode, string sErrMsg)
        {
            throw new NotImplementedException();
        }

        public bool OrderExist(string sSoDescrID)
        {
            throw new NotImplementedException();
        }

        public bool OrderExistLike(string sSoDescrID)
        {
            throw new NotImplementedException();
        }

        public string ProductTypeCheck(string l4ProductType)
        {
            throw new NotImplementedException();
        }

        public bool ReadyForClosing(int iSoID, int iSoLineID)
        {
            throw new NotImplementedException();
        }

        public int RetrievePeriodNumID(DateTime dDeliveryDate, int iPeriodCode, string sPeriodID, DateTime dStopDate)
        {
            throw new NotImplementedException();
        }

        public int RetrieveShipToCode(int iCustomerId, int iShiptoId)
        {
            throw new NotImplementedException();
        }

        public TCheckResult SalesOrderMng(L4L3SoHeader soHeader, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public void SetOEHeaderValues(TSoHeader order, TL4EngineInterfaceMng eimOrderEntry, bool bUpdatingOrder = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(DataSet QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false)
        {
            throw new NotImplementedException();
        }
    }
}
