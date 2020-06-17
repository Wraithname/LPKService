using System;
using Dapper.Oracle;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using NLog;
using LPKService.Infrastructure.Work;
using LPKService.Infrastructure.Repository;
using Repository;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCManagement : CCMRepoBase, ICCManagement
    {
        private Logger logger = LogManager.GetLogger(nameof(CCM));
        private L4L3CustomerRepo customerRepo = new L4L3CustomerRepo();
        /// <summary>
        /// Проверка классификации
        /// </summary>
        /// <param name="strClassification">Классификация</param>
        /// <param name="odp"></param>
        /// <returns>Тип классификации, если есть</returns>
        public string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null)
        {
            string res = "";
            try
            {
                string str = "select an_control_value from   attrb_control_rules where  attrb_code like 'ENDUSER_STEEL_DESIGNATION' and an_control_value  = '"+ strClassification+"'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.ExecuteScalar<string>(str, odp);
            }
            if (res != "")
                return res;
            else
                return "N/A";
            }
            catch { return "N/A"; }
        }
        /// <summary>
        /// Проверка существования заказчика в каталоге
        /// </summary>
        /// <param name="strCustomerDescrId">ИД заказчика</param>
        /// <param name="odp"></param>
        /// <returns>
        /// true - существует
        /// false - не существует
        /// </returns>
        public bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null)
        {
            int RecordCount = 0;
            string str = "select count(*) from   customer_catalogue where  customer_descr_id  = " + strCustomerDescrId;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                RecordCount = connection.ExecuteScalar<int>(str, odp);
            }
            if (RecordCount > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Обработчик события для кода заказчиков
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        /// <returns>Результат обработки</returns>
        public TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo)
        {
            L4L3Customer customer = customerRepo.GetData(l4MsgInfo);
            TCheckResult checkres = new TCheckResult();
            checkres.rejType = 0;
            TL4EngineInterfaceMngRepo engInterf = new TL4EngineInterfaceMngRepo(customer, l4MsgInfo);
            AddressEngine addressEngine = new AddressEngine(engInterf);
            CCatalEngine catalEngine = new CCatalEngine(engInterf);
            CCreditEngine creditEngine = new CCreditEngine(engInterf);
            bool res = true;
            float el4CustomerId;
            string l4CustomerId = "", customerIdForL4m = "", sAddressIdBillTo = "", l4CreateUserId = "", l4ModUserId = "";
            logger.Info("Init 'CustomerMng' function");
            l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS;
            try
            {
                l4CustomerId = customer.customerId.ToString();
                el4CustomerId = customer.customerId;
                l4CreateUserId = engInterf.GetCreateUserId();
                customerIdForL4m = l4CustomerId;
                logger.Info("'CustomerIdForL4':" + customerIdForL4m + "'l4CustomerId':" + l4CustomerId);
                if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL)
                {
                    if (catalEngine.LoadData(l4CustomerId) > 0)
                    {
                        if (catalEngine.IsCustomerDeletable(catalEngine.GetCustomerID()))
                            catalEngine.DeleteCustomer(catalEngine.GetCustomerID());
                    }
                    else
                    {
                        checkres.isOK = engInterf.NotifyErrorMessage($"Заказчик {l4CustomerId} не существует");
                        logger.Error($"Заказчик {l4CustomerId} не существует");
                    }
                }
                else
                {
                    customerIdForL4m = engInterf.GetCreateUserId();
                    if (l4CustomerId == "")
                        res = false;
                    l4ModUserId = engInterf.GetModUserId();
                    if (l4ModUserId == "")
                        res = false;
                    if (res)
                    {
                        if (catalEngine.LoadData(l4CustomerId) > 0)
                        {
                            if (addressEngine.LoadData(catalEngine.GetAddressIdCatalog()) != 1)
                            {
                                checkres.isOK = engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={catalEngine.GetAddressIdCatalog().ToString() } не найдено");
                                logger.Error($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID ={ catalEngine.GetAddressIdCatalog().ToString() } не найдено");
                                res = false;
                            }
                            if (res)
                                catalEngine.SetCustomerDescrId(l4CustomerId);
                        }
                        if (creditEngine.LoadData(catalEngine.GetCustomerID()) > 0)
                        {
                            if (addressEngine.LoadData(creditEngine.GetAddressIdBillTo()) != 1)
                            {
                                try
                                {
                                    sAddressIdBillTo = creditEngine.GetAddressIdBillTo().ToString();
                                }
                                catch
                                {
                                    sAddressIdBillTo = "";
                                }
                                checkres.isOK = engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={sAddressIdBillTo} не найдено");
                                res = false;
                            }
                        }
                    }
                    if (res)
                        res=FillAddressEngine(customer, addressEngine, l4ModUserId);
                    logger.Info("'CustomerMng - Load Customer Catalog data'");
                    if (res)
                    {
                        catalEngine.SetCustomerDescrId(l4CustomerId);
                        catalEngine.SetAddressIdCatalog(addressEngine.GetAddressId().ToString());
                        catalEngine.SetInternalCustomerFlag(customer.internalCustomerFlag);
                        catalEngine.SetInquiryValidityDays(30);
                        catalEngine.SetCustomerCurrencyCode(customer.customerCurrencyCode);
                        catalEngine.SetClassificationType(CheckClassificationType(customer.customerClassificationType));
                        catalEngine.SetWeightUnit("<NULL>");
                        if (customer.customerName.Length > 80)
                            catalEngine.SetCustomerShortName(customer.customerName.Substring(0, 80));
                        else
                            catalEngine.SetCustomerShortName(customer.customerName);
                        catalEngine.SetCreationUserId(l4CreateUserId);
                        if(customer.inn.Length>40)
                        catalEngine.SetInn(customer.inn.Substring(0, 40));
                        else
                            catalEngine.SetInn(customer.inn);
                        if(customer.kpp.Length>40)
                        catalEngine.SetKpp(customer.kpp.Substring(0, 40));
                        else
                            catalEngine.SetKpp(customer.kpp);
                        if(customer.rwstationCode.Length>40)
                        catalEngine.SetRwStationCode(customer.rwstationCode.Substring(0, 40));
                        else
                            catalEngine.SetRwStationCode(customer.rwstationCode);
                        if(customer.region.Length>40)
                        catalEngine.SetRegion(customer.region.Substring(0, 40));
                        else
                            catalEngine.SetRegion(customer.region);
                        catalEngine.SetLevel4CustomerId(customerIdForL4m);
                        
                        DateTime date = new DateTime();
                        if (customer.vailityFlag.ToString() == "Y")
                        {
                            if (catalEngine.GetExpirationDate() != date)
                                catalEngine.SetExpirationDate(date);
                        }
                        else
                        {
                            if (catalEngine.GetExpirationDate() == date)
                                catalEngine.SetExpirationDate(DateTime.Now);
                        }

                        catalEngine.ForceModUserDatetime(l4ModUserId);
                        res = catalEngine.SaveData();
                    }
                    logger.Info("'CustomerMng - Load Customer Catalog Credit Data'");
                    if (res)
                    {
                        creditEngine.SetCustomerID(GetCustIDFromDescr(l4CustomerId));
                        creditEngine.SetAddressIdBillTo(addressEngine.GetAddressId());
                        creditEngine.SetCreditStatus(1);
                        creditEngine.ForceModUserDatetime(l4ModUserId);
                        creditEngine.SetInvoiceType("Y");
                        creditEngine.SetDirectPaymnt("Y");
                        creditEngine.SetCustomerCode(Convert.ToInt32(customer.customerCurrencyCode));
                        res = creditEngine.SaveData();
                    }
                    if (!res && l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS)
                    {
                        engInterf.NotifyErrorMessage("CistomerMng - Unknown fatal error.");
                        checkres.data = "CistomerMng - Unknown fatal error.";
                        checkres.isOK = false;
                    }
                    if (res && l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS)
                    {
                        checkres.data = "CistomerMng - SUCCESS.";
                        checkres.isOK = true;
                    }
                }
                logger.Info("End ''CustomerMng'' function");
                return checkres;
            }
            catch { return checkres; }
        }
        /// <summary>
        /// Заполнение каталога адресов с зависимостями
        /// </summary>
        /// <param name="customer">Модель таблицы L4_L3_Customer</param>
        /// <param name="addressEngine">Интерфейс для каталога адресов</param>
        /// <param name="pModUserId">ИД пользователя</param>
        /// <param name="odp"></param>
        /// <returns>Результат обработки</returns>
        public bool FillAddressEngine(L4L3Customer customer, AddressEngine addressEngine, string pModUserId, OracleDynamicParameters odp = null)
        {
            bool res = false;
            try
            {
                Country cnt = new Country();
                ZipCatalogue zip = new ZipCatalogue();
                logger.Info("Init 'FillAddressEngine' function");
                string str = $"SELECT * FROM COUNTRY WHERE COUNTRY = '{customer.country}'";
                using (OracleConnection connection = GetConnection())
                {
                    cnt = connection.QueryFirstOrDefault<Country>(str, null);
                }
                if (cnt == null)
                {
                    odp = new OracleDynamicParameters();
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
                    odp.Add("P_COUNTRY_CODE", customer.country.Substring(0, 3));
                    odp.Add("P_COUNTRY_ON_DOC", customer.country);
                    odp.Add("P_MOD_USER_ID", pModUserId);
                    using (OracleConnection connection = GetConnection())
                    {
                        LogSqlWithParams(str, odp);
                        connection.Execute(str, odp);
                    }
                }
                odp = new OracleDynamicParameters();
                str = "SELECT * FROM ZIP_CATALOGUE WHERE COUNTRY = '" + customer.country + "' AND ZIP_CODE = '" + customer.zipCode + "' AND CITY = '" + customer.city + "'";
                using (OracleConnection connection = GetConnection())
                {
                    zip = connection.QueryFirstOrDefault<ZipCatalogue>(str, null);
                }
                if (zip == null)
                {
                    str = "INSERT INTO ZIP_CATALOGUE ( " +
                        "COUNTRY," +
                        "ZIP_CODE," +
                        "CITY," +
                        "STATE, " +
                        "MOD_USER_ID," +
                        "MOD_DATETIME" +
                        ") VALUES (" +
                        ":P_COUNTRY," +
                        ":P_ZIP_CODE," +
                        ":P_CITY," +
                        ":P_STATE, " +
                        ":P_MOD_USER_ID," +
                        "SYSDATE )";
                    odp.Add("P_COUNTRY", customer.country);
                    odp.Add("P_ZIP_CODE", customer.zipCode);
                    odp.Add("P_CITY", customer.city);
                    odp.Add("P_STATE", customer.state);
                    odp.Add("P_MOD_USER_ID", pModUserId);
                    using (OracleConnection connection = GetConnection())
                    {
                        LogSqlWithParams(str, odp);
                        connection.Execute(str, odp);
                    }
                }
                addressEngine.SetAddressFullName(customer.customerName);
                addressEngine.SetZipCode(customer.zipCode);
                addressEngine.SetAddress1(customer.address1);
                addressEngine.SetAddress2(customer.address2);
                addressEngine.SetAddress3(customer.address3);
                addressEngine.SetCity(customer.city);
                addressEngine.SetState(customer.state);
                addressEngine.SetCountry(customer.country);
                addressEngine.SetContactName(customer.contactName);
                addressEngine.SetContactPhone1(customer.contactPhone);
                addressEngine.SetContactFax(customer.contactFax);
                addressEngine.SetContactMobile(customer.contactMobile);
                addressEngine.SetEmailAddress(customer.contactEmail);
                logger.Info("End 'FillAddressEngine' function");
                res=addressEngine.SaveData();
                return res;
            }
            catch { return res; }
            }
        /// <summary>
        /// Получение ИД заказчика
        /// </summary>
        /// <param name="sCustomerDescrId">ИД описания клиента</param>
        /// <param name="odp"></param>
        /// <returns>
        /// Возвращает ИД заказчика, если существует
        /// </returns>
        public int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null)
        {
            int res;
            string str = "SELECT CUSTOMER_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_DESCR_ID = '" + sCustomerDescrId+"'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.ExecuteScalar<int>(str, odp);
            }
            if (res > 0)
                return res;
            else
                return -1;
        }
    }
}
