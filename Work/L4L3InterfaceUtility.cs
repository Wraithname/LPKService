using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Logger;
using Repository;

namespace Work
{
    enum tForShipping { NOShipped, YESShipped }
    public class L4L3InterfaceUtility
    {
        private Log logger = LogFactory.GetLogger(nameof(LPKService));
        string[] coloursAttributes = { "PAINT_FIN_BOTTOM_COLOUR", "PAINT_FIN_TOP_COLOUR", "PAINT_PRIME_TOP_TYPE", "PAINT_PRIME_BOTTOM_TYPE" };
        //Узнать что это и для чего
        public void GlobalVarInit()
        {

        }
        //Узнать что это и для чего
        public void GlovalVarFinalize()
        {

        }
        public bool IsAColorAttribute(string SAttrbCode)
        {
            foreach (string attr in coloursAttributes)
            {
                if (SAttrbCode == attr)
                    return true;
            }
            return false;
        }
        public int GetSoIdFromDescr(string sSoDescrID, OracleDynamicParameters odp = null)
        {
            string stm = @"SELECT SO_ID FROM   SALES_ORDER_HEADER WHERE  SO_DESCR_ID = " + sSoDescrID + "";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                return connection.QueryFirstOrDefault<int>(stm, odp);
            }
        }
        public string DecodeL4L3AttribValueAlpha(string SAttrbCode, string SAttrbValue, OracleDynamicParameters odp = null)
        {
            if (!IsAColorAttribute(SAttrbCode))
                return SAttrbValue;
            string stm = @"SELECT ACR.AN_CONTROL_VALUE AS AN_CONTROL_VALUE FROM ATTRB_CONTROL_RULES ACR,
ATTRB_CONTROL_VERSION ACV WHERE TRIM(SUBSTR(" + SAttrbValue + ", 1, length(" + SAttrbValue + "))) = " +
"TRIM(SUBSTR(ACR.AN_CONTROL_VALUE, 1, length(" + SAttrbValue + ")))AND ACR.ATTRB_CODE =" + SAttrbCode + "AND ACR.DUMMY_KEY = ACV.DUMMY_KEY " +
"AND TO_CHAR(ACV.EXPIRATION_DATE, 'DD/MM/YYYY') = '01/01/1970' ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                return connection.QueryFirstOrDefault<string>(stm, odp);
            }
        }
    }
}
