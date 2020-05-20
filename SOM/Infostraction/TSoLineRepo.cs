using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using SOM.Models;
using SOM.Infostraction;
using Repository.Models;
using Repository.Infostraction;
using Repository;
using Logger;

namespace SOM.Repo
{
    public enum TLineStatus { IsClosed, IsOpened}
    public class TSoLineRepo : ITSoLine
    {
        SOManagment som = new SOManagment();
        TCheckRelatedList attributesOfLine=new TCheckRelatedList();
        List<string> slArrayofAttributes=new List<string>();
        private Log logger = LogFactory.GetLogger(nameof(L4L3SoHeader));
        public int CheckValueType(bool bisValid, TL4MsgInfoLine l4MsgInfo)
        {
            bisValid = false;
            int res = 0;
            List<int> values = new List<int>();
            string str = "SELECT SO_TYPE_CODE AS SO_TYPE_CODE" +
                "FROM L4_L3_SO_LINE " +
                "WHERE MSG_COUNTER=:MSG_COUNTER" +
                "AND SO_ID =:SO_ID " +
                "AND (SO_LINE_ID/10)= :SO_LINE_ID";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            odp.Add("MSG_COUNTER", l4MsgInfo.tL4MsgInfo.msgCounter);
            odp.Add("SO_ID", l4MsgInfo.sSoDescrIDInfoLine);
            odp.Add("SO_LINE_ID", l4MsgInfo.iSOLineID);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                values = connection.Query<int>(str, odp).AsList();
            }
            if (values.Count > 0)
            {
                foreach (int val in values)
                {
                    if (val != 0)
                    {
                        int count = 0;
                        str = "SELECT COUNT(*) AS COUNTER " +
                            "FROM AUX_VALUES " +
                            "WHERE INTEGER_VALUE=:INTEGER_VALUE" +
                            "AND VARIABLE_ID = 'ORDER_TYPE' ";
                        odp = new OracleDynamicParameters();
                        odp.Add("INTEGER_VALUE", val);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            count = connection.QueryFirstOrDefault<int>(str, odp);
                        }
                        if (count > 0)
                        {
                            bisValid = true;
                            res = val;
                        }
                        else
                        {
                            bisValid = false;
                            break;
                        }
                    }
                }
            }
            if (!bisValid)
                logger.Error($"Value L4_L3_SO_LINE.SO_TYPE_CODE is not valid");
            return res;
        }

        public int CheckValueProductType(bool bisValid, TL4MsgInfoLine l4MsgInfo)
        {
            bisValid = false;
            int res = 0;
            List<int> values = new List<int>();
            string str = "SELECT PRODUCT_TYPE AS PRODUCT_CODE" +
                "FROM L4_L3_SO_LINE " +
                "WHERE MSG_COUNTER=:MSG_COUNTER" +
                "AND SO_ID =:SO_ID " +
                "AND (SO_LINE_ID/10)= :SO_LINE_ID";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            odp.Add("MSG_COUNTER", l4MsgInfo.tL4MsgInfo.msgCounter);
            odp.Add("SO_ID", l4MsgInfo.sSoDescrIDInfoLine);
            odp.Add("SO_LINE_ID", l4MsgInfo.iSOLineID);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                values = connection.Query<int>(str, odp).AsList();
            }
            if (values.Count > 0)
            {
                foreach (int val in values)
                {
                    if (val != 0)
                    {
                        int count = 0;
                        str = "SELECT COUNT(*) AS COUNTER " +
                            "FROM PRODUCT_TYPE_CATALOGUE " +
                            "WHERE SAP_CODE=:SAP_CODE" +
                            "AND FINISHED_PRODUCT_FLAG = 'Y' AND  (EXPIRATION_DATE IS NULL OR TO_CHAR(EXPIRATION_DATE, 'DD/MM/YYYY')" +
                            "= '01/01/1970' OR EXPIRATION_DATE > sysdate) AND VALIDITY_DATE < sysdate ";
                        odp = new OracleDynamicParameters();
                        odp.Add("SAP_CODE", val);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            count = connection.QueryFirstOrDefault<int>(str, odp);
                        }
                        if (count > 0)
                        {
                            bisValid = true;
                            res = val;
                        }
                        else
                        {
                            bisValid = false;
                            break;
                        }
                    }
                }
            }
            if (!bisValid)
                logger.Error($"Value L4_L3_SO_LINE.SO_TYPE_CODE is not valid");
            return res;
        }

        public int CheckValueCredit(bool bisValid, TL4MsgInfoLine l4MsgInfo)
        {
            bisValid = false;
            int res = 0;
            List<int> values = new List<int>();
            string str = "SELECT SO_LINE_CREDIT_STATUS AS CREDIT_STATUS" +
                "FROM L4_L3_SO_LINE " +
                "WHERE MSG_COUNTER=:MSG_COUNTER" +
                "AND SO_ID =:SO_ID " +
                "AND (SO_LINE_ID/10)= :SO_LINE_ID";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            odp.Add("MSG_COUNTER", l4MsgInfo.tL4MsgInfo.msgCounter);
            odp.Add("SO_ID", l4MsgInfo.sSoDescrIDInfoLine);
            odp.Add("SO_LINE_ID", l4MsgInfo.iSOLineID);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                values = connection.Query<int>(str, odp).AsList();
            }
            if (values.Count > 0)
            {
                foreach (int val in values)
                {
                    if (val != 0)
                    {
                        int count = 0;
                        str = "SELECT COUNT(*) AS COUNTER " +
                            "FROM AUX_VALUES " +
                            "WHERE INTEGER_VALUE=:INTEGER_VALUE" +
                            "AND VARIABLE_ID = 'CREDIT_STATUS' ";
                        odp = new OracleDynamicParameters();
                        odp.Add("INTEGER_VALUE", val);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            count = connection.QueryFirstOrDefault<int>(str, odp);
                        }
                        if (count > 0)
                        {
                            bisValid = true;
                            res = val;
                        }
                        else
                        {
                            bisValid = false;
                            break;
                        }
                    }
                }
            }
            if (!bisValid)
                logger.Error($"Value L4_L3_SO_LINE.SO_TYPE_CODE is not valid");
            return res;
        }

        public TSoLine Create(LinesCom line, TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate)
        {
            TSoLine newData = new TSoLine();
            L4L3InterfaceUtility inter = new L4L3InterfaceUtility();
            bool bVerifyData = true;
            int iSysDatePeriodNumID = 0;
            try
            {
                TL4MsgInfoLine m_L4MsgInfoLine = new TL4MsgInfoLine();
                m_L4MsgInfoLine.tL4MsgInfo.msgCounter = line.msgCounter;
                m_L4MsgInfoLine.tL4MsgInfo.msgReport.status = line.msgStatus;
                m_L4MsgInfoLine.tL4MsgInfo.msgReport.remark = "";
                newData.m_iSoLineID = line.soLineMet;
                TL4EngineInterfaceMngRepo m_eimOELineInterface = new TL4EngineInterfaceMngRepo(m_L4MsgInfoLine.tL4MsgInfo);
                newData.m_sSoDescrID = line.soId;
                if (lbIsUpdate)
                    newData.m_iSoID = inter.GetSoIdFromDescr(newData.m_sSoDescrID);
                m_L4MsgInfoLine.sSoDescrIDInfoLine = newData.m_sSoDescrID;
                switch(som.DecodeContractType(newData.m_sSoDescrID,iCustomerID.ToString()))
                {
                    case TContractType.coInternal:
                        if(!som.IsCustomerInternal(iCustomerID))
                        {
                            m_eimOELineInterface.NotifyErrorMessage("Заказ обозначен как внутренний, но имеет внешнего заказчика");
                            bVerifyData = false;
                        }
                        break;
                }
                newData.m_iShipToCode = iShipToCode;
                if (bVerifyData)
                    newData.m_iSoTypeCode = CheckValueType(bVerifyData, m_L4MsgInfoLine);
                if (newData.m_iSoTypeCode == 2)
                    newData.m_iSoTypeCode = 4;
                if (bVerifyData)
                    newData.m_iCreditStatus = CheckValueCredit(bVerifyData, m_L4MsgInfoLine);
                if (bVerifyData)
                {
                    newData.m_iOrderLineStatus = -1;
                    if (line.soLineStatus.ToString().Length > 0)
                        newData.m_iOrderLineStatus = line.soLineStatus;
                    if (!(newData.m_iOrderLineStatus > 0))
                    {
                        m_eimOELineInterface.NotifyErrorMessage("Неверный статус строки заказа");
                        bVerifyData = false;
                    }
                    if (line.dueDeliveryDate.ToString().Length > 0)
                        newData.m_dDueDelivery = line.dueDeliveryDate;
                    newData.m_iDeliveryPeriodNumID = som.RetrievePeriodNumID(newData.m_dDueDelivery, 1);
                    if(GetLineStatus(newData.m_iOrderLineStatus)!=TLineStatus.IsClosed)
                        iSysDatePeriodNumID = som.RetrievePeriodNumID(DateTime.Now, 1);
                    if (bVerifyData)
                    {
                        if (line.productType.Length > 0)
                        {
                            newData.m_strProductCode = CheckValueProductType(bVerifyData, m_L4MsgInfoLine).ToString();
                            newData.m_strProductCode = som.ProductTypeCheck(newData.m_strProductCode);
                        }
                    }
                    if(bVerifyData)
                    {
                        newData.m_iInsertUser = m_eimOELineInterface.GetCreateUserId();
                        if (newData.m_iInsertUser == "")
                            bVerifyData = false;
                    }
                    if(bVerifyData)
                    {
                        newData.m_iUpdateUser = m_eimOELineInterface.GetModUserId();
                        if (newData.m_iUpdateUser == "")
                            bVerifyData = false;
                    }
                    if (l4MsgInfo.opCode > 0)
                        newData.m_iOpCode = l4MsgInfo.opCode;
                }
                if (bVerifyData)
                    som.LoadAttrb(newData.m_L4MsgInfoLine, newData.m_strProductCode, newData.m_iSoID, newData.m_iSoLineID,attributesOfLine, slArrayofAttributes);
                return newData;
            }
            catch(Exception e) {
                logger.Trace($"TSOLine.Create - Exception - Message:{e.Message}");
                return newData; }
        }

        public string GetMsgStatus(TL4MsgInfoLine l4MsgInfo)
        {
            return l4MsgInfo.tL4MsgInfo.msgReport.remark;
        }
        public void UpdateMsgStatus(TL4MsgInfo l4MsgInfo)
        {
            TL4MsgInfoLineRepo msgInfoLine = new TL4MsgInfoLineRepo();
            msgInfoLine.UpdateMsgStatus(l4MsgInfo);
        }

        public TLineStatus GetLineStatus(int linestatus)
        {
            throw new NotImplementedException();
        }
    }
}
