using System;
using Dapper;
using Dapper.Oracle;
using Repository;
using LPKService.Domain.Models.CCM;
using LPKService.Infrastructure.Work;
using Oracle.ManagedDataAccess.Client;

namespace LPKService.Infrastructure.CCM
{
    public class CCreditEngine:CCMRepoBase
    {
        CustomerCatCredit catCredit;
        bool exist = false;
        /// <summary>
        /// Конструктор создания модели Customer_catalog_credit
        /// </summary>
        /// <param name="interfaceMng"></param>
        public CCreditEngine(TL4EngineInterfaceMngRepo interfaceMng)
        {
            this.catCredit = new CustomerCatCredit();
        }
        /// <summary>
        /// Запись изменной даты пользователем
        /// </summary>
        /// <param name="modUserId">ИД пользователя</param>
        /// <param name="str">Текст</param>
        public void ForceModUserDatetime(string modUserId, string str = "")
        {
            catCredit.modUserId = Convert.ToInt32(modUserId);
        }
        /// <summary>
        /// Получение адреса
        /// </summary>
        /// <returns>ИД адреса</returns>
        public int GetAddressIdBillTo()
        {
            return catCredit.addressId;
        }
        /// <summary>
        /// Количество записей по доверенным заказчикам
        /// </summary>
        /// <param name="num">ИД заказчика</param>
        /// <returns>Количество строк</returns>
        public int LoadData(int num)
        {
            int count = -1;
            string sqlstr = $"SELECT COUNT(*) FROM CUSTOMER_CATALOG_CREDIT WHERE CUSTOMER_ID = {num}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            if (count > 0)
            {
                GetData(num);
                exist = true;
            }
            return count;
        }
        /// <summary>
        /// Получение данных по ИД заказчика
        /// </summary>
        /// <param name="id">ИД заказчиков</param>
        public void GetData(int id)
        {
            string sqlstr = $"SELECT * FROM ADDRESS_CATALOG WHERE ADDRESS_ID = {id}";
            using (OracleConnection connection = GetConnection())
            {
                catCredit = connection.QueryFirstOrDefault<CustomerCatCredit>(sqlstr, null);
            }
        }
        /// <summary>
        /// Сохранение данных по доверенным заказчикам
        /// </summary>
        /// <returns>
        /// true - данные сохранены
        /// false - данные не сохранены
        /// </returns>
        public bool SaveData()
        {
            bool res = false;
            if (!exist)
            {
                try
                {
                    OracleDynamicParameters odp = new OracleDynamicParameters();
                    string sqlstr = "INSERT INTO ADDRESS_CATALOG (CUSTOMER_ID,CREDIT_STATUS,ADDRESS_ID,DB_NUMBER," +
                        "DB_RATING,AUDITED_BALANCE_FLAG,VAT_NUMBER,LAST_FINANC_STMT_DATE,FIRST_SALE_DATE,LAST_SALE_DATE,CUSTOMER_CREDIT_LIMIT,REQUIRED_CREDIT_LIMIT,INVOICE_SEND_BY,ACCOUNTING_CONTACT_NAME,MOD_USER_ID,MOD_DATETIME," +
                        "ADMINISTRATION_CONTACT_NAME,DISCOUNT_NOTE_FOR_CREDIT_TERM,INVOICE_TYPE,INVOICE_CURRENCY_CODE,INVOICE_COPIES,FREIGHT_AND_STEEL_APART_ON_INV,DIRECT_PAYMNT_FLAG,BANK_ACCOUNT,CREDIT_TERM_CODE,CUSTOMER_CODE)" +
                        "VALUES(:P_CUSTOMER_ID,:P_CREDIT_STATUS,:P_ADDRESS_ID,:P_DB_NUMBER," +
                        ":P_DB_RATING,:P_AUDITED_BALANCE_FLAG,:P_VAT_NUMBER,:P_LAST_FINANC_STMT_DATE,:P_FIRST_SALE_DATE,:P_LAST_SALE_DATE,:P_CUSTOMER_CREDIT_LIMIT,:P_REQUIRED_CREDIT_LIMIT,:P_INVOICE_SEND_BY,:P_ACCOUNTING_CONTACT_NAME,:P_MOD_USER_ID,SYSDATE," +
                        ":P_ADMINISTRATION_CONTACT_NAME,:P_DISCOUNT_NOTE_FOR_CREDIT_TERM,:P_INVOICE_TYPE,:P_INVOICE_CURRENCY_CODE,:P_INVOICE_COPIES,:P_FREIGHT_AND_STEEL_APART_ON_INV,:P_DIRECT_PAYMNT_FLAG,:P_BANK_ACCOUNT,:P_CREDIT_TERM_CODE,:P_CUSTOMER_CODE)";
                    odp.Add("P_CUSTOMER_ID", catCredit.customerId);
                    odp.Add("P_CREDIT_STATUS", catCredit.creditStatus);
                    odp.Add("P_ADDRESS_ID", catCredit.addressId);
                    odp.Add("P_DB_NUMBER", catCredit.dbNumber);
                    odp.Add("P_DB_RATING", catCredit.dbRating);
                    odp.Add("P_AUDITED_BALANCE_FLAG", catCredit.auditedBalanceFlag);
                    odp.Add("P_VAT_NUMBER", catCredit.vatNumber);
                    odp.Add("P_LAST_FINANC_STMT_DATE", catCredit.lastFibabcStmtDate);
                    odp.Add("P_FIRST_SALE_DATE", catCredit.firstSaleDate);
                    odp.Add("P_LAST_SALE_DATE", catCredit.lastSaleDate);
                    odp.Add("P_CUSTOMER_CREDIT_LIMIT", catCredit.customerCreditLimit);
                    odp.Add("P_REQUIRED_CREDIT_LIMIT", catCredit.requiredCreditLimit);
                    odp.Add("P_INVOICE_SEND_BY", catCredit.invoiceSendBy);
                    odp.Add("P_ACCOUNTING_CONTACT_NAME", catCredit.accountingContactName);
                    odp.Add("P_MOD_USER_ID", catCredit.modUserId);
                    odp.Add("P_ADMINISTRATION_CONTACT_NAME", catCredit.administrationContcatName);
                    odp.Add("P_DISCOUNT_NOTE_FOR_CREDIT_TERM", catCredit.discountNoteForCreditTerm);
                    odp.Add("P_INVOICE_TYPE", catCredit.invoiceType);
                    odp.Add("P_INVOICE_CURRENCY_CODE", catCredit.invoiceCurrencyCode);
                    odp.Add("P_INVOICE_COPIES", catCredit.invoiceCopies);
                    odp.Add("P_FREIGHT_AND_STEEL_APART_ON_INV", catCredit.freightAndSteelApartOnInv);
                    odp.Add("P_DIRECT_PAYMNT_FLAG", catCredit.directPaymntFlag);
                    odp.Add("P_BANK_ACCOUNT", catCredit.bankAccount);
                    odp.Add("P_CREDIT_TERM_CODE", catCredit.creditTermCode);
                    odp.Add("P_CUSTOMER_CODE", catCredit.customerCode);
                    using (OracleConnection connection = GetConnection())
                    {
                        LogSqlWithParams(sqlstr, odp);
                        connection.Execute(sqlstr, odp);
                        res = true;
                    }
                }
                catch
                { }
            }
            else
            {
                try
                {
                    OracleDynamicParameters odp = new OracleDynamicParameters();
                    string sqlstr = "UPDATE ADDRESS_CATALOG SET (CREDIT_STATUS,ADDRESS_ID,DB_NUMBER," +
                        "DB_RATING,AUDITED_BALANCE_FLAG,VAT_NUMBER,LAST_FINANC_STMT_DATE,FIRST_SALE_DATE,LAST_SALE_DATE,CUSTOMER_CREDIT_LIMIT,REQUIRED_CREDIT_LIMIT,INVOICE_SEND_BY,ACCOUNTING_CONTACT_NAME,MOD_USER_ID,MOD_DATETIME," +
                        "ADMINISTRATION_CONTACT_NAME,DISCOUNT_NOTE_FOR_CREDIT_TERM,INVOICE_TYPE,INVOICE_CURRENCY_CODE,INVOICE_COPIES,FREIGHT_AND_STEEL_APART_ON_INV,DIRECT_PAYMNT_FLAG,BANK_ACCOUNT,CREDIT_TERM_CODE,CUSTOMER_CODE)" +
                        "VALUES(:P_CREDIT_STATUS,:P_ADDRESS_ID,:P_DB_NUMBER," +
                        ":P_DB_RATING,:P_AUDITED_BALANCE_FLAG,:P_VAT_NUMBER,:P_LAST_FINANC_STMT_DATE,:P_FIRST_SALE_DATE,:P_LAST_SALE_DATE,:P_CUSTOMER_CREDIT_LIMIT,:P_REQUIRED_CREDIT_LIMIT,:P_INVOICE_SEND_BY,:P_ACCOUNTING_CONTACT_NAME,:P_MOD_USER_ID,SYSDATE," +
                        ":P_ADMINISTRATION_CONTACT_NAME,:P_DISCOUNT_NOTE_FOR_CREDIT_TERM,:P_INVOICE_TYPE,:P_INVOICE_CURRENCY_CODE,:P_INVOICE_COPIES,:P_FREIGHT_AND_STEEL_APART_ON_INV,:P_DIRECT_PAYMNT_FLAG,:P_BANK_ACCOUNT,:P_CREDIT_TERM_CODE,:P_CUSTOMER_CODE) WHERE CUSTOMER_ID=:P_CUSTOMER_ID";
                    odp.Add("P_CREDIT_STATUS", catCredit.creditStatus);
                    odp.Add("P_ADDRESS_ID", catCredit.addressId);
                    odp.Add("P_DB_NUMBER", catCredit.dbNumber);
                    odp.Add("P_DB_RATING", catCredit.dbRating);
                    odp.Add("P_AUDITED_BALANCE_FLAG", catCredit.auditedBalanceFlag);
                    odp.Add("P_VAT_NUMBER", catCredit.vatNumber);
                    odp.Add("P_LAST_FINANC_STMT_DATE", catCredit.lastFibabcStmtDate);
                    odp.Add("P_FIRST_SALE_DATE", catCredit.firstSaleDate);
                    odp.Add("P_LAST_SALE_DATE", catCredit.lastSaleDate);
                    odp.Add("P_CUSTOMER_CREDIT_LIMIT", catCredit.customerCreditLimit);
                    odp.Add("P_REQUIRED_CREDIT_LIMIT", catCredit.requiredCreditLimit);
                    odp.Add("P_INVOICE_SEND_BY", catCredit.invoiceSendBy);
                    odp.Add("P_ACCOUNTING_CONTACT_NAME", catCredit.accountingContactName);
                    odp.Add("P_MOD_USER_ID", catCredit.modUserId);
                    odp.Add("P_ADMINISTRATION_CONTACT_NAME", catCredit.administrationContcatName);
                    odp.Add("P_DISCOUNT_NOTE_FOR_CREDIT_TERM", catCredit.discountNoteForCreditTerm);
                    odp.Add("P_INVOICE_TYPE", catCredit.invoiceType);
                    odp.Add("P_INVOICE_CURRENCY_CODE", catCredit.invoiceCurrencyCode);
                    odp.Add("P_INVOICE_COPIES", catCredit.invoiceCopies);
                    odp.Add("P_FREIGHT_AND_STEEL_APART_ON_INV", catCredit.freightAndSteelApartOnInv);
                    odp.Add("P_DIRECT_PAYMNT_FLAG", catCredit.directPaymntFlag);
                    odp.Add("P_BANK_ACCOUNT", catCredit.bankAccount);
                    odp.Add("P_CREDIT_TERM_CODE", catCredit.creditTermCode);
                    odp.Add("P_CUSTOMER_CODE", catCredit.customerCode);
                    odp.Add("P_CUSTOMER_ID", catCredit.customerId);
                    using (OracleConnection connection = BaseRepo.GetDBConnection())
                    {
                        LogSqlWithParams(sqlstr, odp);
                        connection.Execute(sqlstr, odp);
                        res = true;
                    }
                }
                catch
                { }
            }
            return res;
        }
        /// <summary>
        /// Запись ИД адреса
        /// </summary>
        /// <param name="addressId">ИД адреса</param>
        public void SetAddressIdBillTo(int addressId)
        {
            catCredit.addressId = addressId;
        }
        /// <summary>
        /// Запись доверительного статуса
        /// </summary>
        /// <param name="num">Статус</param>
        public void SetCreditStatus(int num)
        {
            catCredit.creditStatus = num;
        }
        /// <summary>
        /// Запись ИД заказчика из описания
        /// </summary>
        /// <param name="custIdFromDscr">ИД заказчика из описания</param>
        public void SetCustomerID(int custIdFromDscr)
        {
            catCredit.customerId = custIdFromDscr;
        }
        /// <summary>
        /// Запись типа счета
        /// </summary>
        /// <param name="type">Тип счета</param>
        public void SetInvoiceType(string type)
        {
            catCredit.invoiceType = type;
        }
        /// <summary>
        /// Запись флага груза
        /// </summary>
        /// <param name="chr">Флаг</param>
        public void SetFandSApartonINV(string chr)
        {
            catCredit.freightAndSteelApartOnInv=Convert.ToChar(chr.Substring(0,1));
        }
        /// <summary>
        /// Запись флага прямой оплаты
        /// </summary>
        /// <param name="chr">Флаг</param>
        public void SetDirectPaymnt(string chr)
        {
            catCredit.directPaymntFlag = Convert.ToChar(chr.Substring(0, 1));
        }
        /// <summary>
        /// Запись кода заказчика
        /// </summary>
        /// <param name="code">Код заказчика</param>
        public void SetCustomerCode(int code)
        {
            catCredit.customerCode = code;
        }
    }
}
