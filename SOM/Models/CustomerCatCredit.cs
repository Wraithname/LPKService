using System;
using Repository;

namespace SOM.Models
{
    class CustomerCatCredit
    {
        [Column("CUSTOMER_ID", "ИД заказчика")]
        int customerId { get; set; }
        [Column("CREDIT_STATUS", "Статус кредита")]
        int creditStatus{ get; set; }
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        int addressId{ get; set; }
        [Column("DB_NUMBER", "Не используется")]
        string dbNumber{ get; set; }
        [Column("DB_RATING", "Не используется")]
        string dbRating{ get; set; }
        [Column("AUDITED_BALANCE_FLAG", "Не используется")]
        char auditedBalanceFlag{ get; set; }
        [Column("VAT_NUMBER", "Не используется")]
        string vatNumber{ get; set; }
        [Column("LAST_FINANC_STMT_DATE", null)]
        DateTime lastFibabcStmtDate{ get; set; }
        [Column("FIRST_SALE_DATE", "Дата первой продажи")]
        DateTime firstSaleDate{ get; set; }
        [Column("LAST_SALE_DATE", "Дата последней продажи")]
        DateTime lastSaleDate{ get; set; }
        [Column("CUSTOMER_CREDIT_LIMIT", "Лимит кредита клиента")]
        float customerCreditLimit{ get; set; }
        [Column("REQUIRED_CREDIT_LIMIT", "Требуемый лимит кредита")]
        float requiredCreditLimit{ get; set; }
        [Column("INVOICE_SEND_BY", "Создатель накладной/счёта")]
        string invoiceSendBy{ get; set; }
        [Column("ACCOUNTING_CONTACT_NAME", "Не используется")]
        string accountingContactName{ get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        int modUserId{ get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        DateTime modDatetime{ get; set; }
        [Column("ADMINISTRATION_CONTACT_NAME", "Не используется")]
        string administrationContcatName{ get; set; }
        [Column("DISCOUNT_NOTE_FOR_CREDIT_TERM", "Заметка по скидке по виду оплаты")]
        string discountNoteForCreditTerm{ get; set; }
        [Column("INVOICE_TYPE", "Тип накладной/счёта")]
        string invoiceType{ get; set; }
        [Column("INVOICE_CURRENCY_CODE", "Валюта накладной/счёта")]
        string invoiceCurrencyCode{ get; set; }
        [Column("INVOICE_COPIES", "Копии накладной/счёта")]
        int invoiceCopies{ get; set; }
        [Column("FREIGHT_AND_STEEL_APART_ON_INV", "Не используется")]
        char freightAndSteelApartOnInv{ get; set; }
        [Column("DIRECT_PAYMNT_FLAG", "Не используется")]
        char directPaymntFlag{ get; set; }
        [Column("BANK_ACCOUNT", "Не используется")]
        string bankAccount{ get; set; }
        [Column("CREDIT_TERM_CODE", "Тип оплаты")]
        string creditTermCode{ get; set; }
        [Column("CUSTOMER_CODE", "Код клиента")]
        int customerCode{ get; set; }
    }
}
