using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using SOM.Models;
using Repository;
using Work.Models;
using SOM.Repo;

namespace SOM.Infostraction
{
    class TSoHeaderRepo: ITSoHeader
    {
        public TSoLine GetLine(int iIndex, TSoHeader ret)
        {
            TSoLine line = new TSoLine();
            line = ret.m_Lines[iIndex];
            return line;
        }
        public string GetCustIDFromL4CustID(int l4CustID, OracleDynamicParameters odp = null)
        {
            string cust;
            StringBuilder stm = new StringBuilder(@"SELECT CUSTOMER_DESCR_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID =" + l4CustID + "");
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                cust = conn.QueryFirstOrDefault<string>(stm.ToString(), odp);
            }
            if (cust!=null)
                return cust;
            else
                return (-1).ToString();
        }
        public bool ExistCustomer(string m_iCustSoldDescrID, OracleDynamicParameters odp = null)
        {
            string cust;
            StringBuilder stm = new StringBuilder(@"SELECT EXPIRATION_DATE FROM CUSTOMER_CATALOG WHERE  CUSTOMER_DESCR_ID = " + m_iCustSoldDescrID + "");
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                cust = conn.QueryFirstOrDefault<string>(stm.ToString(), odp);
            }
            if (cust != null)
            {
                if (Convert.ToDateTime(cust) <= DateTime.Now)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void Add(TSoLine lines, TSoHeader ret)
        {
            ret.m_Lines[ret.m_iLinesCount++] = lines;
        }
        //Реализовать создание 
        public TSoHeader Create(TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry, int iShipToCode, bool bVerifyData, bool bisUpdate = false)
        {
            TSoHeader soHeader = new TSoHeader();
            TSoLine templine = new TSoLine();
            //string strProva;
            //bool bIsValid=true, bVerData;
            //TDeleteResponse deleteResponse;

            return soHeader;
        }

        public string GetLineMsgStatus(TSoHeader ret)
        {
            TSoLineRepo func = new TSoLineRepo();
            string strMessages="",strLineMessage;
            foreach(TSoLine line in ret.m_Lines)
            {
                strLineMessage = func.GetMsgStatus(line.m_L4MsgInfoLine);
                if (strLineMessage.ToCharArray().Length > 0 && (strMessages.ToCharArray().Length + strLineMessage.ToCharArray().Length + 3) > 4000)
                    strMessages = strMessages + " - " + strLineMessage;
            }
            return strMessages;
        }

        public void LinesUpdateMsgStatus(TSoHeader ret)
        {
            TSoLineRepo func = new TSoLineRepo();
            foreach(TSoLine line in ret.m_Lines)
            {
                func.UpdateMsgStatus(line.m_L4MsgInfoLine.tL4MsgInfo);
            }
        }

        public bool CheckLineNumeration(int iCounter,OracleDynamicParameters odp=null)
        {
            string solineid;
            string stm = @"SELECT SO_LINE_ID / 10 AS SO_LINE_ID FROM L4_L3_SO_LINE WHERE  MSG_COUNTER = "+iCounter.ToString()+ " ORDER BY SO_LINE_ID ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                solineid=connection.QueryFirstOrDefault<string>(stm, odp);
                int i = 1;
                while(true)
                {
                    if (i != Convert.ToInt32(solineid))
                        return false;
                    i++;
                }
            }
        }
    }
}
