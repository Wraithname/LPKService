using System;
using Repository;

namespace SOM.Models
{
    public class AddresCat
    {
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        public int addressId { set { addressId = value; } }
        [Column("ADDRESS_FULL_NAME", "Полный адрес")]
        public string addressFullName { set { addressFullName = value; } }
        [Column("CONTACT_NAME", "Имя контакного лица")]
        public string contactName { set { contactName = value; } }
        [Column("ZIP_CODE", "Индекс")]
        public string zipCode { set { zipCode = value; } }
        [Column("ADDRESS2", "Адрес 2")]
        public string address2 {  set { address2 = value; } }
        [Column("ADDRESS3", "Адрес 3")]
        public string address3 { set { address3 = value; } }
        [Column("ADDRESS1", "Адрес 1")]
        public string address1 { set { address1 = value; } }
        [Column("CITY", "Город")]
        public string city {  set { city = value; } }
        [Column("STATE_CODE", "Код области")]
        public string stateCode {  set { stateCode = value; } }
        [Column("STATE", "Штат(область)")]
        public string state {  set { state = value; } }
        [Column("COUNTRY", "Страна")]
        public string country { set { country = value; } }
        [Column("CONTACT_PHONE1", "Контактный телефон1")]
        public string contactPhone1 { set { contactPhone1 = value; } }
        [Column("CONTACT_FAX", "Контактный факс")]
        public string contactFax { set { contactFax = value; } }
        [Column("CONTACT_PHONE2", "Контактный телефон2")]
        public string contactPhone2 { set { contactPhone2 = value; } }
        [Column("CONTACT_MOBILE", "Контактный телефон")]
        public string contactMobile { set { contactMobile = value; } }
        [Column("EDI_ADDRESS", "EDI адрес")]
        public string ediAddress { set { ediAddress = value; } }
        [Column("EMAIL_ADDRESS", "Электронная почта")]
        public string emailAddress { set { emailAddress = value; } }
        [Column("SPECIAL_NOTES", "Специальные заметки")]
        public string specialNotes { set { specialNotes = value; } }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        public int modUserId { set { modUserId = value; } }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDateTime { set { modDateTime = value; } }
        [Column("RECEIVING_PHONE", "Телефон получателя")]
        public string receivingPhone { set { receivingPhone = value; } }
        [Column("COMPANY_PHONE", "Телефон компании")]
        public string companyPhone { set { companyPhone = value; } }
        [Column("CONTACT_NAME2", "Имя контакного лица2")]
        public string contactName2 { set { contactName2 = value; } }
        [Column("COMPANY_FAX", "Факс компании")]
        public string companyFax { set { companyFax = value; } }
        [Column("COMPANY_EMAIL", "Электронная почта компании")]
        public string companyEmail { set { companyEmail = value; } }
        [Column("CONTACT_POSITION", "Должность контактного лица")]
        public string contactPosition { set { contactPosition = value; } }
        [Column("COMPANY_WEB_SITE", "Веб-сайт компании")]
        public string companyWebSite { set { companyWebSite = value; } }
    }
}