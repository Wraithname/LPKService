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

namespace SOM.Repo
{
    class TSoHeaderRepo
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
        //public bool CheckLineNumeration(int iCounter)
        //{

        //}
    }
}
