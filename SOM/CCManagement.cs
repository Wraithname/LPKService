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
using SOM.Infostraction;

namespace SOM
{
    //Используется таблица L4_L3_CUSTOMER
    public class CCManagement : ICCManagement
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
            int RecordCount = 0;
            string str = "select count(*) from   customer_catalogue where  customer_descr_id  = " + strCustomerDescrId;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                RecordCount = connection.QueryFirstOrDefault<int>(str, odp);
            }
            if (RecordCount > 0)
                return true;
            else
                return false;
        }

        public TCheckResult CustomerMng(L4L3Customer customer, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }
        
        public bool FillAddressEngine(L4L3Customer customer, string pModUserId, OracleDynamicParameters odp = null)
        {
            AddressEngine addressEngine = new AddressEngine();
            Country cnt = new Country();
            ZipCatalogue zip = new ZipCatalogue();
            logger.Trace("Init 'FillAddressEngine' function");
            bool result = true;
            string str = "SELECT * FROM COUNTRY WHERE COUNTRY =  "+ customer.country;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                cnt = connection.QueryFirstOrDefault<Country>(str, odp);
            }
            if (cnt == null)
            {
                str = "INSERT INTO COUNTRY ( " +
                    "COUNTRY," +
                    "COUNTRY_CODE," +
                    "COUNTRY_ON_DOC," +
                    "MOD_USER_ID," +
                    "MOD_DATETIME" +
                    ") VALUES (" +
                    ":P_COUNTRY," +
                    ":P_COUNTRY_CODE," +
                    ":P_COUNTRY_ON_DOC," +
                    ":P_MOD_USER_ID," +
                    "SYSDATE )";
                odp.Add("P_COUNTRY", customer.country);
                odp.Add("P_COUNTRY_CODE", customer.country.Substring(0, 40));
                odp.Add("P_COUNTRY_ON_DOC", customer.country);
                odp.Add("P_MOD_USER_ID", pModUserId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(str, odp);
                }
                odp = null;
            }
            str = "SELECT * FROM ZIP_CATALOGUE WHERE COUNTRY = " + customer.country + " AND ZIP_CODE = " + customer.zipCode + " AND CITY = " + customer.city;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                zip = connection.QueryFirstOrDefault<ZipCatalogue>(str, odp);
            }
            if(zip==null)
            {
                str = "INSERT INTO ZIP_CATALOGUE ( " +
                    "COUNTRY," +
                    "ZIP_CODE," +
                    "CITY," +
                    "MOD_USER_ID," +
                    "MOD_DATETIME" +
                    ") VALUES (" +
                    ":P_COUNTRY," +
                    ":P_ZIP_CODE," +
                    ":P_CITY," +
                    ":P_MOD_USER_ID," +
                    "SYSDATE )";
                odp.Add("P_COUNTRY", customer.country);
                odp.Add("P_ZIP_CODE", customer.zipCode);
                odp.Add("P_CITY", customer.city);
                odp.Add("P_MOD_USER_ID", pModUserId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(str, odp);
                }
                odp = null;
            }
            if(result)
            result=addressEngine.SetAddressFullName(customer.customerName);
            if (result)
                result = addressEngine.SetZipCode(customer.zipCode);
            if (result)
                result = addressEngine.SetAddress1(customer.address1);
            if (result)
                result = addressEngine.SetAddress2(customer.address2);
            if (result)
                result = addressEngine.SetAddress3(customer.address3);
            if (result)
                result = addressEngine.SetCity(customer.city);
            return result;
        }

        public int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null)
        {
            int res = 0;
            string str = "SELECT CUSTOMER_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_DESCR_ID = " + sCustomerDescrId;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.QueryFirstOrDefault<int>(str, odp);
                if (res > 0)
                    return res;
                else
                    return -1;
            }
        }
    }
}
