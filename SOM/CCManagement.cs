using System;
using SOM.Models;
using SOM.Repo;
using Repository.Models;
using Dapper.Oracle;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Repository;
using Logger;
using SOM.Infostraction;

namespace SOM
{
    //Используется таблица L4_L3_CUSTOMER
    //Файл CCManagment.pas
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
            }
            if (res != "")
                return res;
            else
                return "N/A";

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

        public TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo)
        {
            L4L3CustomerRepo customerRepo = new L4L3CustomerRepo();
            L4L3Customer customer = customerRepo.GetData(l4MsgInfo);
            TCheckResult checkres = new TCheckResult();
            TL4EngineInterfaceMngRepo engInterf = new TL4EngineInterfaceMngRepo();
            engInterf.Create(customer, l4MsgInfo);
            AddressEngine addressEngine = new AddressEngine();
            CCatalEngine catalEngine = new CCatalEngine();
            CCreditEngine creditEngine = new CCreditEngine();
            bool res = true;
            float el4CustomerId;
            string l4CustomerId="", customerIdForL4m = "", sAddressIdBillTo="",l4CreateUserId = "", l4ModUserId = "";
            logger.Trace("Init 'CustomerMng' function");
            l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS;
            try
            {
                l4CustomerId = customer.customerId.ToString();
                el4CustomerId = customer.customerId;
                customerIdForL4m = l4CustomerId;
                logger.Trace("LOG_TRACE e - 'CustomerIdForL4':" + customerIdForL4m + "'l4CustomerId':" + l4CustomerId);
                if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL)
                {
                    if (catalEngine.LoadData(l4CustomerId) > 0)
                    {
                        if (catalEngine.IsCustomerDeletable(catalEngine.GetCustomerID()))
                            catalEngine.DeleteCustomer(catalEngine.GetCustomerID());
                    }
                    else
                        engInterf.NotifyErrorMessage($"Заказчик {l4CustomerId} не существует");
                }
                else
                {
                    if (res)
                    {
                        l4CustomerId = engInterf.GetCreateUserId();
                        if (l4CustomerId == "")
                            res = false;
                    }
                    if (res)
                    {
                        l4ModUserId = engInterf.GetModUserId();
                        if (l4ModUserId == "")
                            res = false;
                    }
                    if (res)
                    {
                        if(catalEngine.LoadData(l4CustomerId)>0)
                        {
                            if(addressEngine.LoadData(catalEngine.GetAddressIdCatalog())!=1)
                            {
                                engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={catalEngine.GetAddressIdCatalog().ToString() } не найдено");
                                res = false;
                            }
                            if (res)
                                res = catalEngine.SetCustomerDescrId(l4CustomerId);
                        }
                        if(creditEngine.LoadData(catalEngine.GetCustomerID())>0)
                        {
                            if(addressEngine.LoadData(creditEngine.GetAddressIdBillTo())!=1)
                            {
                                try
                                {
                                    sAddressIdBillTo = creditEngine.GetAddressIdBillTo().ToString();
                                }
                                catch
                                {
                                    sAddressIdBillTo = "";
                                }
                                engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={sAddressIdBillTo} не найдено");
                                res = false;
                            }
                        }
                    }
                    // =====================================================================
                    // ADDRESS_CATALOG
                    // =====================================================================
                    if (res)
                        res = FillAddressEngine(customer,addressEngine, l4ModUserId);
                    logger.Trace("'CustomerMng - Load Customer Catalog data'");
                    // =====================================================================
                    // CUSTOMER_CATALOG
                    // =====================================================================
                    if (res)
                        res = catalEngine.SetCustomerDescrId(l4CustomerId);
                    if (res)
                        res = catalEngine.SetAddressIdCatalog(addressEngine.GetAddressId());
                    if (res)
                        res = catalEngine.SetInternalCustomerFlag(Convert.ToBoolean(customer.internalCustomerFlag));
                    if (res)
                        res = catalEngine.SetInquiryValidityDays(30);
                    if (res)
                        res = catalEngine.SetCustomerCurrencyCode(customer.customerCurrencyCode);
                    if (res)
                        res = catalEngine.SetClassificationType(CheckClassificationType(customer.customerClassificationType));
                    if (res)
                        res = catalEngine.SetWeightUnit("<NULL>");
                    if (res)
                        res = catalEngine.SetCustomerShortName(customer.customerName.Substring(0, 80));
                    if (res)
                        res = catalEngine.SetCreationUserId(l4CreateUserId);
                    if (res)
                        res = catalEngine.SetInn(customer.inn.Substring(0, 40));
                    if (res)
                        res = catalEngine.SetKpp(customer.kpp.Substring(0, 40));
                    if (res)
                        res = catalEngine.SetRwStationCode(customer.rwstationCode.Substring(0, 40));
                    if (res)
                        res = catalEngine.SetRegion(customer.region.Substring(0, 40));
                    if (res)
                        res = catalEngine.SetLevel4CustomerId(customerIdForL4m);
                    if(res)
                    {
                        DateTime date = new DateTime();
                        if(customer.vailityFlag.ToString()=="Y")
                        {
                            if (catalEngine.GetExpirationDate() != date)
                                if (res)
                                    res = catalEngine.SetExpirationDate(date);
                        }
                        else
                        {
                            if (catalEngine.GetExpirationDate() == date)
                                if (res)
                                    res = catalEngine.SetExpirationDate(new DateTime());
                        }
                    }
                    if (res)
                        res = catalEngine.SaveData(false);
                    if (res)
                        res = catalEngine.ForceModUserDatetime(false, l4ModUserId);
                    logger.Trace("'CustomerMng - Load Customer Catalog Credit Data'");
                    // =====================================================================
                    // CUSTOMER_CATALOG_CREDIT
                    // =====================================================================
                    if (res)
                        res = creditEngine.SetCustomerID(GetCustIDFromDescr(l4CustomerId));
                    if (res)
                        res = creditEngine.SetAddressIdBillTo(Convert.ToInt32(addressEngine.GetAddressId()));
                    if (res)
                        res = creditEngine.SetCreditStatus(1);
                    if (res)
                        res = creditEngine.SaveData(false);
                    if (res)
                        res = creditEngine.ForceModUserDatetime(false, l4ModUserId);
                    // =====================================================================
                    // final operations
                    // =====================================================================
                    if (!res && l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS)
                    {
                        engInterf.NotifyErrorMessage("CistomerMng - Unknown fatal error.");
                        checkres.data = "CistomerMng - Unknown fatal error.";
                        checkres.isOK = false;
                    }
                    if (res && l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS)
                    {
                        engInterf.NotifyErrorMessage("Запись успешно обработана", "");
                        checkres.data = "CistomerMng - Unknown fatal error.";
                        checkres.isOK = true;
                    }
                }
                logger.Trace("End ''CustomerMng'' function");
                return checkres;
            }
            catch { return checkres; }
        }
        
        public bool FillAddressEngine(L4L3Customer customer,AddressEngine addressEngine, string pModUserId, OracleDynamicParameters odp = null)
        {
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
            if (result)
                result = addressEngine.SetState(customer.state);
            if (result)
                result = addressEngine.SetCountry(customer.country);
            if (result)
                result = addressEngine.SetContactName(customer.contactName);
            if (result)
                result = addressEngine.SetContactPhone1(customer.contactPhone);
            if (result)
                result = addressEngine.SetContactFax(customer.contactFax);
            if (result)
                result = addressEngine.SetContactMobile(customer.contactMobile);
            if (result)
                result = addressEngine.SetEmailAddress(customer.contactEmail);
            if (result)
                result = addressEngine.SaveData(false);
            return result;
        }

        public int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null)
        {
            int res = 0;
            string str = "SELECT CUSTOMER_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_DESCR_ID = " + sCustomerDescrId;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.QueryFirstOrDefault<int>(str, odp);
            }
            if (res > 0)
                return res;
            else
                return -1;
        }
    }
}
