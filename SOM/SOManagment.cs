using System;
using System.Collections.Generic;
using System.Data;
using SOM.Models;
using SOM.Repo;
using SOM.Infostraction;
using Repository.Infostaction;
using Repository.WorkModels;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;
using NLog;

namespace SOM
{
    //Используется таблица L4_L3_SO_HEADER
    //Файл SOManagment.pas
    public enum TContractType { coInternal , coContract }
    public enum TShiptoType { Shipto,Billto};
    public class SOManagment : ISOManagment
    {
        private Logger logger = LogManager.GetLogger(nameof(SOM));
        List<TLineNote> lines = new List<TLineNote>();
        TLineNote line;
        public void AddVsw_detailsToOrder(TL4MsgInfo l4MsgInfo)
        {
            string vsw_detail, sodescId;
            int soidv;
        }

        public bool AlreadyInsertInSuspended(int iSoID)
        {
            throw new NotImplementedException();
        }

        public TCheckResult AttributeCheck(DataSet qryData, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer)
        {
            string sqlstr = "";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            if (serRSer)
                sqlstr = "Select UPDATE_BLOCK_ORDER(1,:msg_counter) FROM DUAL";
            else
                sqlstr = "Select UPDATE_BLOCK_ORDER(0,:msg_counter) FROM DUAL";
            odp.Add("msg_counter", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(sqlstr, odp);
            }
        }

        public bool CheckInquiryLinesStatus(int iMsgCounter)
        {
            throw new NotImplementedException();
        }

        public void CheckUpdateOPCODE(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public void CloseLine(int iSoId, int iSoLineId, int iSoTypeCode)
        {
            throw new NotImplementedException();
        }

        public bool CompareAttributes(TCheckRelatedList chlAttrbListFromSap, TCheckRelatedList chlAttrbListFromDB, List<string> strAttrbCodesFromSap, TL4EngineInterfaceMng eimOrderEntry, bool mandatory = true)
        {
            throw new NotImplementedException();
        }

        public bool CompareHeaderValues(TSoHeader objsoHeader, TL4EngineInterfaceMng eimOrderEntry)
        {
            throw new NotImplementedException();
        }

        public bool CompareLines(TSoHeader objsoHeader, int iNumOfLine, TL4EngineInterfaceMng eimOrderEntry)
        {
            throw new NotImplementedException();
        }

        public bool CompareLineValues(int iLineId, TL4EngineInterfaceMng eimOrderEntry)
        {
            throw new NotImplementedException();
        }

        public void CreateNewOrder(DataSet QryData, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TContractType DecodeContractType(string soid, string strCustomerId)
        {
            OracleDynamicParameters odp = null;
            char flag;
            string str = "select internal_customer_flag " +
                "from customer_catalog " +
                "where  customer_descr_id = p_customer_descr_id";
            odp.Add("p_customer_descr_id", strCustomerId,OracleMappingType.Varchar2);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                flag = connection.QueryFirstOrDefault<char>(str, odp);
            }
            if (flag == 'Y' || soid.Substring(0, 3) == "OMK")
                return TContractType.coInternal;
            return TContractType.coContract;
        }

        public TDeleteResponse DeleteOrder(int iOrderID)
        {
            throw new NotImplementedException();
        }

        public bool ExistCustomer(string m_iCustSoldDescrID)
        {
            throw new NotImplementedException();
        }

        public bool IsCustomerInternal(int iCustomerId)
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

        public int RetrievePeriodNumID(DateTime dDeliveryDate, int iPeriodCode, string sPeriodID = "", DateTime dStopDate = new DateTime())
        {
            throw new NotImplementedException();
        }

        public int RetrieveShipToCode(int iCustomerId, int iShiptoId)
        {
            throw new NotImplementedException();
        }

        public TCheckResult SalesOrderMng(TL4MsgInfo l4MsgInfo)
        {
            L4L3CustomerRepo customerRepo = new L4L3CustomerRepo();
            L4L3Customer customers = customerRepo.GetData(l4MsgInfo);
            throw new NotImplementedException();
        }

        public void SaveInMassForHeaderNote(int pSoId, string pHeaderNote)
        {
            throw new NotImplementedException();
        }


        public void SetOEHeaderValues(TSoHeader order, TL4EngineInterfaceMng eimOrderEntry, bool bUpdatingOrder = false)
        {
            throw new NotImplementedException();
        }

        public void SetOELinesValues(TSoLine order, bool bReordered = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateCreditStatus(TSoHeader objsoHeader, int iLineId)
        {
            throw new NotImplementedException();
        }

        public void UpdateHeaderFields(TSoHeader objsoHeader)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineFields(TSoHeader objsoHeader)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(DataSet QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false)
        {
            throw new NotImplementedException();
        }
    }
}
