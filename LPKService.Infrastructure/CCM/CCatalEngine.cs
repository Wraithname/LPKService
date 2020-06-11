using System;
using Repository;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Domain.Models.CCM;
using LPKService.Infrastructure.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCatalEngine : CCMRepoBase
    {
        CustomerCat customerCat;
        /// <summary>
        /// Конструктор создания модели Customer_catalog
        /// </summary>
        /// <param name="interfaceMng"></param>
        public CCatalEngine(TL4EngineInterfaceMngRepo interfaceMng)
        {
            this.customerCat = new CustomerCat();
        }
        /// <summary>
        /// Удаление заказчика
        /// </summary>
        /// <param name="customID">ИД заказчика</param>
        public void DeleteCustomer(int customID)
        {
            OracleDynamicParameters odp = null;
            string str = $"DELETE * FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID=:{customID}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(str, odp);
            }
        }
        /// <summary>
        /// Запись изменной даты пользователем
        /// </summary>
        /// <param name="modUserId">ИД пользователя</param>
        /// <param name="txt">Текст</param>
        public void ForceModUserDatetime(string modUserId, string txt = "")
        {
            customerCat.modUserId = Convert.ToInt32(modUserId);
        }
        /// <summary>
        /// Получение ИД адреса
        /// </summary>
        /// <returns></returns>
        public int GetAddressIdCatalog()
        {
            return customerCat.addresId;
        }
        /// <summary>
        /// Получение ИД заказчика
        /// </summary>
        /// <returns>ИД заказчика</returns>
        public int GetCustomerID()
        {
            return customerCat.custimerId;
        }
        /// <summary>
        /// Получение срока годности
        /// </summary>
        /// <returns>Срок годности</returns>
        public DateTime GetExpirationDate()
        {
            return customerCat.expirationDate;
        }
        /// <summary>
        /// Проверка связей заказчиков для удаления
        /// </summary>
        /// <param name="customID">ИД заказчика</param>
        /// <returns>
        /// true - удаление разрешено
        /// false - удаление запрещено  
        /// </returns>
        public bool IsCustomerDeletable(int customID)
        {
            return true;
        }
        /// <summary>
        /// Количество записей по заказчику
        /// </summary>
        /// <param name="id">ИД заказчика</param>
        /// <returns>Число строк</returns>
        public int LoadData(string id)
        {
            int count = -1;
            string sqlstr = $"SELECT COUNT(*) FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID = {id}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            return count;
        }
        /// <summary>
        /// Количество записей по заказчику
        /// </summary>
        /// <returns>Число строк</returns>
        private int LoadData()
        {
            int count = 0;
            string sqlstr = $"SELECT COUNT(*) FROM CUSTOMER_CATALOG";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            return count;
        }
        /// <summary>
        /// Получение данных по ИД заказчика
        /// </summary>
        /// <param name="id">ИД заказчика</param>
        public void GetData(string id)
        {
            string sqlstr = $"SELECT * FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID = {id}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                customerCat = connection.QueryFirstOrDefault<CustomerCat>(sqlstr, null);
            }
        }
        /// <summary>
        /// Сохранение данных по заказчику
        /// </summary>
        /// <returns>
        /// true - данные сохранены
        /// false - данные не сохранены 
        /// </returns>
        public bool SaveData()
        {
            customerCat.custimerId= LoadData();
            bool res = false;
            try
            {
                OracleDynamicParameters odp = new OracleDynamicParameters();
                string sqlstr = "INSERT INTO CUSTOMER_CATALOG (FOREIGN_CUSTOMER_FLAG,ADDRESS_ID," +
                "CUSTOMER_SHORT_NAME,INSIDE_CUSTOMER_FLAG,CREATION_DATE,VALIDITY_DATE,CREATION_USER_ID," +
                "MOD_USER_ID,MOD_DATETIME,INTERNAL_CUSTOMER_FLAG,RETAILER_FLAG," +
                "CUSTOMER_DESCR_ID,INN,KPP,RWSTATION_CODE)" +
                    "VALUES('N',:P_ADDRESS_ID," +
                    $"'{customerCat.customerShortName}','N',SYSDATE,SYSDATE,:P_CREATION_USER_ID," +
                    ":P_MOD_USER_ID,SYSDATE,'N','N'," +
                    $"'{customerCat.customerShortName}',:P_INN,:P_KPP,:P_RWSTATION_CODE)";
                odp.Add("P_ADDRESS_ID", customerCat.addresId);
                odp.Add("P_CREATION_USER_ID", customerCat.creationUserId);
                odp.Add("P_MOD_USER_ID", customerCat.modUserId);
                odp.Add("P_INN", customerCat.inn);
                odp.Add("P_KPP", customerCat.kpp);
                odp.Add("P_RWSTATION_CODE", customerCat.rwstationCode);

                using (OracleConnection connection = GetConnection())
                {
                    LogSqlWithParams(sqlstr, odp);
                    connection.Execute(sqlstr, odp);
                    res = true;
                }
            }
            catch {}
            return res;
        }
        /// <summary>
        /// Записи ИД адреса
        /// </summary>
        /// <param name="addressId">ИД адреса</param>
        public void SetAddressIdCatalog(string addressId)
        {
            customerCat.addresId = Convert.ToInt32(addressId);
        }
        /// <summary>
        /// Запись типа классификации
        /// </summary>
        /// <param name="customertype">Тип заказчика</param>
        public void SetClassificationType(string customertype)
        {
            customerCat.customerType = customertype;
        }
        /// <summary>
        /// Запись ИД пользователя - создателя
        /// </summary>
        /// <param name="customerId"></param>
        public void SetCreationUserId(string customerId)
        {
            customerCat.creationUserId = Convert.ToInt32(customerId);
        }
        /// <summary>
        /// Запись кода валюты заказчика
        /// </summary>
        /// <param name="customercode">Код заказчика</param>
        public void SetCustomerCurrencyCode(string customercode)
        {
            customerCat.customerCurrencyCode = customercode;
        }
        /// <summary>
        /// Запись описания заказчика
        /// </summary>
        /// <param name="l4CustimerId">ИД заказчика</param>
        public void SetCustomerDescrId(string l4CustimerId)
        {
            customerCat.customerDescrId = l4CustimerId;
        }
        /// <summary>
        /// Запись имени заказчика
        /// </summary>
        /// <param name="value">Имя заказчика</param>
        public void SetCustomerShortName(string value)
        {
            customerCat.customerShortName = value;
        }
        /// <summary>
        /// Запись срока годности
        /// </summary>
        /// <param name="date">Срок годности</param>
        public void SetExpirationDate(DateTime date)
        {
            customerCat.expirationDate = date;
            customerCat.creationDate = date;
        }
        /// <summary>
        /// Запись ИНН
        /// </summary>
        /// <param name="inn">ИНН</param>
        public void SetInn(string inn)
        {
            customerCat.inn = inn;
        }
        /// <summary>
        /// Запись дней действия запроса
        /// </summary>
        /// <param name="number">Дни</param>
        public void SetInquiryValidityDays(int number)
        {
            customerCat.inquiryValidityDays = number;
        }
        /// <summary>
        /// Запись внутреннего флага заказчика
        /// </summary>
        /// <param name="flag">Флаг заказчика</param>
        public void SetInternalCustomerFlag(char flag)
        {
            customerCat.internalCustomerFlag = flag;
        }
        /// <summary>
        /// Запись КПП
        /// </summary>
        /// <param name="kpp">КПП</param>
        public void SetKpp(string kpp)
        {
            customerCat.kpp = kpp;
        }
        /// <summary>
        /// Запись ИД заказчика для L4
        /// </summary>
        /// <param name="customerIdforL4">ИД заказчика для L4</param>
        public void SetLevel4CustomerId(string customerIdforL4)
        {
            customerCat.level4CustomerId = customerIdforL4;
        }
        /// <summary>
        /// Запись региона
        /// </summary>
        /// <param name="region">Регион</param>
        public void SetRegion(string region)
        {
            customerCat.region = region;
        }
        /// <summary>
        /// Запись железнодорожной станции
        /// </summary>
        /// <param name="rwstcode">Железнодорожная станция</param>
        public void SetRwStationCode(string rwstcode)
        {
            customerCat.rwstationCode = rwstcode;
        }
        /// <summary>
        /// Запись веса продукции
        /// </summary>
        /// <param name="value">Вес продукции</param>
        public void SetWeightUnit(string value)
        {
            customerCat.customerWeightUnitId = value;
        }
    }
}
