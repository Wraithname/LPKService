using System;
using Repository;

namespace SOM.Models
{
    class CustomerCat
    {
        [Column("CUSTOMER_ID", "ИД заказчика")]
        int custimerId { get; set; }
        [Column("FOREIGN_CUSTOMER_FLAG", "Зарубежный клиент")]
        char foreignCustomerFlag{ get; set; }
        [Column("AGENT_NUM_ID", "Числовой ID агента")]
        int agentNumId{ get; set; }
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        int addresId{ get; set; }
        [Column("PRICE_LIST_NUM_ID", "ИД прайс-листа")]
        int priceListNumId{ get; set; }
        [Column("CUSTOMER_SHORT_NAME", "Короткое имя клиента")]
        string customerShortName{ get; set; }
        [Column("INSIDE_CUSTOMER_FLAG", "Внешний клиент")]
        char insideCustomerFlag{ get; set; }
        [Column("CUSTOMER_TYPE", "Тип клиента")]
        string customerType{ get; set; }
        [Column("SPECIAL_NOTES", "Специальные заметки")]
        string specialNotes{ get; set; }
        [Column("CREATION_DATE", "Дата создания")]
        DateTime creationDate{ get; set; }
        [Column("VALIDITY_DATE", "Дата действия")]
        DateTime validityDate{ get; set; }
        [Column("EXPIRATION_DATE", "Дата истечения")]
        DateTime expirationDate{ get; set; }
        [Column("EXPIRATION_DECISION_DATE", "Дата истечения решения")]
        DateTime expirationDecisionDate{ get; set; }
        [Column("REACTIVATION_DATE", "Дата возобновления деятельности")]
        DateTime reactivationDate{ get; set; }
        [Column("CREATION_USER_ID", "Создавший пользователь")]
        int creationUserId{ get; set; }
        [Column("EXPIRATION_USER_ID", "Пользователь, назначивший дату истечения")]
        int expirationUserId{ get; set; }
        [Column("REACTIVATION_USER_ID", "Пользователь, указавший дату возобновления деятельности")]
        int reactivationUserId{ get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        int modUserId{ get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        DateTime modDateTime{ get; set; }
        [Column("INTERNAL_CUSTOMER_FLAG", "Внутренний клиент")]
        char internalCustomerFlag{ get; set; }
        [Column("FIELD_SALES_REP_ID", "Не используется")]
        int fieldSalesRepId{ get; set; }
        [Column("CLASSIFICATION_TYPE", "Тип классификации")]
        string classificationType{ get; set; }
        [Column("INQUIRY_VALIDITY_DAYS", "Доступные дни")]
        int inquiryValidityDays{ get; set; }
        [Column("CUSTOMER_CURRENCY_CODE", "Код валюты клиента")]
        string customerCurrencyCode{ get; set; }
        [Column("LEVEL4_CUSTOMER_ID", "ID клиента в ERP")]
        string level4CustomerId{ get; set; }
        [Column("RETAILER_FLAG", "Является ли клиент посредником")]
        char retailerFlag{ get; set; }
        [Column("PRODUCT_TYPE", "Тип продукта")]
        string productType{ get; set; }
        [Column("CUSTOMER_WEIGHT_UNIT_ID", "Единицы измерения веса у клиента")]
        string customerWeightUnitId{ get; set; }
        [Column("CUSTOMER_DESCR_ID", "ID описания клиента")]
        string customerDescrId{ get; set; }
        [Column("INN", "ИНН")]
        string inn{ get; set; }
        [Column("KPP", "КПП")]
        string kpp{ get; set; }
        [Column("RWSTATION_CODE", "Код ж/д станции")]
        string rwstationCode{ get; set; }
        [Column("REGION", "Код области")]
        string region{ get; set; }
    }
}
