using System;
using LPKService.Repository;

namespace LPKService.Domain.Models
{
    public class CustomerCatCredit
    {
        [Column("CUSTOMER_ID", "ИД заказчика")]
        public int customerId { get; set; }
        [Column("CREDIT_STATUS", "Статус кредита")]
        public int creditStatus { get; set; }
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        public int addressId { get; set; }
        [Column("DB_NUMBER", "Не используется")]
        public string dbNumber { get; set; }
        [Column("DB_RATING", "Не используется")]
        public string dbRating { get; set; }
        [Column("AUDITED_BALANCE_FLAG", "Не используется")]
        public char auditedBalanceFlag { get; set; }
        [Column("VAT_NUMBER", "Не используется")]
        public string vatNumber { get; set; }
        [Column("LAST_FINANC_STMT_DATE", null)]
        public DateTime lastFibabcStmtDate { get; set; }
        [Column("FIRST_SALE_DATE", "Дата первой продажи")]
        public DateTime firstSaleDate { get; set; }
        [Column("LAST_SALE_DATE", "Дата последней продажи")]
        public DateTime lastSaleDate { get; set; }
        [Column("CUSTOMER_CREDIT_LIMIT", "Лимит кредита клиента")]
        public float customerCreditLimit { get; set; }
        [Column("REQUIRED_CREDIT_LIMIT", "Требуемый лимит кредита")]
        public float requiredCreditLimit { get; set; }
        [Column("INVOICE_SEND_BY", "Создатель накладной/счёта")]
        public string invoiceSendBy { get; set; }
        [Column("ACCOUNTING_CONTACT_NAME", "Не используется")]
        public string accountingContactName { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        public int modUserId { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDatetime { get; set; }
        [Column("ADMINISTRATION_CONTACT_NAME", "Не используется")]
        public string administrationContcatName { get; set; }
        [Column("DISCOUNT_NOTE_FOR_CREDIT_TERM", "Заметка по скидке по виду оплаты")]
        public string discountNoteForCreditTerm { get; set; }
        [Column("INVOICE_TYPE", "Тип накладной/счёта")]
        public string invoiceType { get; set; }
        [Column("INVOICE_CURRENCY_CODE", "Валюта накладной/счёта")]
        public string invoiceCurrencyCode { get; set; }
        [Column("INVOICE_COPIES", "Копии накладной/счёта")]
        public int invoiceCopies { get; set; }
        [Column("FREIGHT_AND_STEEL_APART_ON_INV", "Не используется")]
        public char freightAndSteelApartOnInv { get; set; }
        [Column("DIRECT_PAYMNT_FLAG", "Не используется")]
        public char directPaymntFlag { get; set; }
        [Column("BANK_ACCOUNT", "Не используется")]
        public string bankAccount { get; set; }
        [Column("CREDIT_TERM_CODE", "Тип оплаты")]
        public string creditTermCode { get; set; }
        [Column("CUSTOMER_CODE", "Код клиента")]
        public int customerCode { get; set; }
    }
}
