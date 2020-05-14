using Repository;

namespace Work.Models
{
    public class L4L3Customer
    {
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("CUSTOMER_ID", "ИД заказчика")]
        public int customerId { get; set; }
        [Column("CUSTOMER_NAME", "Наименование заказчика")]
        public string customerName { get; set; }
        [Column("INTERNAL_CUSTOMER_FLAG", "Внутренний клиент")]
        public char internalCustomerFlag { get; set; }
        [Column("CUSTOMER_CLASSIFICATION_TYPE", "Не используется")]
        public string customerClassificationType { get; set; }
        [Column("CUSTOMER_CURRENCY_CODE", "Код валюты клиента")]
        public string customerCurrencyCode { get; set; }
        [Column("ZIP_CODE", "Индекс")]
        public string zipCode { get; set; }
        [Column("ADDRESS_1", "Адрес 1")]
        public string address1 { get; set; }
        [Column("ADDRESS_2", "Адрес 2")]
        public string address2 { get; set; }
        [Column("ADDRESS_3", "Адрес 3")]
        public string address3 { get; set; }
        [Column("CITY", "Город")]
        public string city { get; set; }
        [Column("STATE", "Штат(область)")]
        public string state { get; set; }
        [Column("COUNTRY", "Страна")]
        public string country { get; set; }
        [Column("CONTACT_NAME", "Имя контакного лица")]
        public string contactName { get; set; }
        [Column("CONTACT_PHONE", "Контактный телефон")]
        public string contactPhone { get; set; }
        [Column("CONTACT_FAX", "Контактный факс")]
        public string contactFax { get; set; }
        [Column("CONTACT_MOBILE", "Контактный телефон")]
        public string contactMobile { get; set; }
        [Column("CONTACT_EMAIL", "Контактная эл.почта")]
        public string contactEmail { get; set; }
        [Column("VALIDITY_FLAG", "Флаг валидности")]
        public char vailityFlag { get; set; }
        [Column("ERP_CODE", "Код ERP")]
        public string erpCode { get; set; }
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
