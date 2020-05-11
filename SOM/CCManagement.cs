using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;
using Work.Models;
using Dapper.Oracle;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Repository;
using Logger;

namespace SOM
{
    class CCManagement : ICCManagement
    {
        private Log logger = LogFactory.GetLogger(nameof(CCManagement));
        public string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null)
        {
            string res = "";
            string str = "select an_control_value from   attrb_control_rules where  attrb_code like 'ENDUSER_STEEL_DESIGNATION' and an_control_value  = " + strClassification;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.QueryFirstOrDefault<string>(str, odp);
                if (res != "")
                    return res;
                else
                    return "N/A";
            }

        }

        public bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null)
        {
            int RecordCount=0;
            string str = "select count(*) from   customer_catalogue where  customer_descr_id  = " + strCustomerDescrId;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                RecordCount= connection.QueryFirstOrDefault<int>(str, odp);
            }
            if (RecordCount > 0)
                return true;
            else
                return false;
        }

        public TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public bool FillAddressEngine(AddresEngine addrEngine, string pModUserId, OracleDynamicParameters odp = null)
        {
            Country cnt = new Country();
            logger.Trace("Init 'FillAddressEngine' function");
            string str = "SELECT * FROM COUNTRY WHERE COUNTRY =  ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                cnt = connection.QueryFirstOrDefault<Country>(str, odp);
            }
            return false;
        }

        public int GetCustIDFromDescr(string sCustomerDescrId)
        {
            throw new NotImplementedException();
        }
    }
}
