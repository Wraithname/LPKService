using System;
using LPKService.Repository;

namespace LPKService.Domain.Models
{
    public class CustomerCat
    {
        [Column("CUSTOMER_ID", "ИД заказчика")]
        public int custimerId { get; set; }
        [Column("FOREIGN_CUSTOMER_FLAG", "Зарубежный клиент")]
        public char foreignCustomerFlag { get; set; }
        [Column("AGENT_NUM_ID", "Числовой ID агента")]
        public int agentNumId { get; set; }
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        public int addresId { get; set; }
        [Column("PRICE_LIST_NUM_ID", "ИД прайс-листа")]
        public int priceListNumId { get; set; }
        [Column("CUSTOMER_SHORT_NAME", "Короткое имя клиента")]
        public string customerShortName { get; set; }
        [Column("INSIDE_CUSTOMER_FLAG", "Внешний клиент")]
        public char insideCustomerFlag { get; set; }
        [Column("CUSTOMER_TYPE", "Тип клиента")]
        public string customerType { get; set; }
        [Column("SPECIAL_NOTES", "Специальные заметки")]
        public string specialNotes { get; set; }
        [Column("CREATION_DATE", "Дата создания")]
        public DateTime creationDate { get; set; }
        [Column("VALIDITY_DATE", "Дата действия")]
        public DateTime validityDate { get; set; }
        [Column("EXPIRATION_DATE", "Дата истечения")]
        public DateTime expirationDate { get; set; }
        [Column("EXPIRATION_DECISION_DATE", "Дата истечения решения")]
        public DateTime expirationDecisionDate { get; set; }
        [Column("REACTIVATION_DATE", "Дата возобновления деятельности")]
        public DateTime reactivationDate { get; set; }
        [Column("CREATION_USER_ID", "Создавший пользователь")]
        public int creationUserId { get; set; }
        [Column("EXPIRATION_USER_ID", "Пользователь, назначивший дату истечения")]
        public int expirationUserId { get; set; }
        [Column("REACTIVATION_USER_ID", "Пользователь, указавший дату возобновления деятельности")]
        public int reactivationUserId { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        public int modUserId { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDateTime { get; set; }
        [Column("INTERNAL_CUSTOMER_FLAG", "Внутренний клиент")]
        public char internalCustomerFlag { get; set; }
        [Column("FIELD_SALES_REP_ID", "Не используется")]
        public int fieldSalesRepId { get; set; }
        [Column("CLASSIFICATION_TYPE", "Тип классификации")]
        public string classificationType { get; set; }
        [Column("INQUIRY_VALIDITY_DAYS", "Доступные дни")]
        public int inquiryValidityDays { get; set; }
        [Column("CUSTOMER_CURRENCY_CODE", "Код валюты клиента")]
        public string customerCurrencyCode { get; set; }
        [Column("LEVEL4_CUSTOMER_ID", "ID клиента в ERP")]
        public string level4CustomerId { get; set; }
        [Column("RETAILER_FLAG", "Является ли клиент посредником")]
        public char retailerFlag { get; set; }
        [Column("PRODUCT_TYPE", "Тип продукта")]
        public string productType { get; set; }
        [Column("CUSTOMER_WEIGHT_UNIT_ID", "Единицы измерения веса у клиента")]
        public string customerWeightUnitId { get; set; }
        [Column("CUSTOMER_DESCR_ID", "ID описания клиента")]
        public string customerDescrId { get; set; }
        [Column("INN", "ИНН")]
        public string inn { get; set; }
        [Column("KPP", "КПП")]
        public string kpp { get; set; }
        [Column("RWSTATION_CODE", "Код ж/д станции")]
        public string rwstationCode { get; set; }
        [Column("REGION", "Код области")]
        public string region { get; set; }
    }
}
