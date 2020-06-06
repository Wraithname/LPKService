using System;
using Dapper.Oracle;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using NLog;
using LPKService.Infrastructure.Work;
using LPKService.Infrastructure.Repository;
using LPKService.Domain.BaseRepository;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCManagement : CCMRepoBase,ICCManagement
    {
        private Logger logger = LogManager.GetLogger(nameof(CCManagement));
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
            string str = "select an_control_value from   attrb_control_rules where  attrb_code like 'ENDUSER_STEEL_DESIGNATION' and an_control_value  = " + strClassification;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.ExecuteScalar<string>(str, odp);
            }
            if (res != "")
                return res;
            else
                return "N/A";

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
                        if (catalEngine.LoadData(l4CustomerId) > 0)
                        {
                            if (addressEngine.LoadData(catalEngine.GetAddressIdCatalog()) != 1)
                            {
                                engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={catalEngine.GetAddressIdCatalog().ToString() } не найдено");
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
                                engInterf.NotifyErrorMessage($"Ошибка при обработке заказчика - Фатальная ошибка - поле ADDRESS_ID={sAddressIdBillTo} не найдено");
                                res = false;
                            }
                        }
                    }
                    // =====================================================================
                    // ADDRESS_CATALOG
                    // =====================================================================
                    if (res)
                        res = FillAddressEngine(customer, addressEngine, l4ModUserId);
                    logger.Trace("'CustomerMng - Load Customer Catalog data'");
                    // =====================================================================
                    // CUSTOMER_CATALOG
                    // =====================================================================
                    if (res)
                    {

                        catalEngine.SetCustomerDescrId(l4CustomerId);
                        catalEngine.SetAddressIdCatalog(addressEngine.GetAddressId());
                        catalEngine.SetInternalCustomerFlag(Convert.ToBoolean(customer.internalCustomerFlag));
                        catalEngine.SetInquiryValidityDays(30);
                        catalEngine.SetCustomerCurrencyCode(customer.customerCurrencyCode);
                        catalEngine.SetClassificationType(CheckClassificationType(customer.customerClassificationType));
                        catalEngine.SetWeightUnit("<NULL>");
                        catalEngine.SetCustomerShortName(customer.customerName.Substring(0, 80));
                        catalEngine.SetCreationUserId(l4CreateUserId);
                        catalEngine.SetInn(customer.inn.Substring(0, 40));
                        catalEngine.SetKpp(customer.kpp.Substring(0, 40));
                        catalEngine.SetRwStationCode(customer.rwstationCode.Substring(0, 40));
                        catalEngine.SetRegion(customer.region.Substring(0, 40));
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
                                catalEngine.SetExpirationDate(new DateTime());
                        }

                        catalEngine.ForceModUserDatetime(l4ModUserId);
                        res = catalEngine.SaveData();
                    }
                    logger.Trace("'CustomerMng - Load Customer Catalog Credit Data'");
                    // =====================================================================
                    // CUSTOMER_CATALOG_CREDIT
                    // =====================================================================
                    if (res)
                    {
                        creditEngine.SetCustomerID(GetCustIDFromDescr(l4CustomerId));
                        creditEngine.SetAddressIdBillTo(Convert.ToInt32(addressEngine.GetAddressId()));
                        creditEngine.SetCreditStatus(1);
                        creditEngine.ForceModUserDatetime(l4ModUserId);
                        res = creditEngine.SaveData();
                    }
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
                        checkres.data = "CistomerMng - SUCCESS.";
                        checkres.isOK = true;
                    }
                }
                logger.Trace("End ''CustomerMng'' function");
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
        public bool FillAddressEngine(L4L3Customer customer, IAddressEngine addressEngine, string pModUserId, OracleDynamicParameters odp = null)
        {
            Country cnt = new Country();
            ZipCatalogue zip = new ZipCatalogue();
            logger.Trace("Init 'FillAddressEngine' function");
            bool result = true;
            string str = "SELECT * FROM COUNTRY WHERE COUNTRY =  " + customer.country;
            using (OracleConnection connection = GetConnection())
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
            using (OracleConnection connection = GetConnection())
            {
                zip = connection.QueryFirstOrDefault<ZipCatalogue>(str, odp);
            }
            if (zip == null)
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
            if (result)
            {
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
                result = addressEngine.SaveData();
            }
            return result;
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
            int res = 0;
            string str = "SELECT CUSTOMER_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_DESCR_ID = " + sCustomerDescrId;
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
