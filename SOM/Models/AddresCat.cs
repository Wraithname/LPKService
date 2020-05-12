using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace SOM.Models
{
    public class AddresCat
    {
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        public int addressId { get; set; }
        [Column("ADDRESS_FULL_NAME", "Полный адрес")]
        public string addressFullName { get; set; }
        [Column("CONTACT_NAME", "Имя контакного лица")]
        public string contactName { get; set; }
        [Column("ZIP_CODE", "Индекс")]
        public string zipCode { get; set; }
        [Column("ADDRESS2", "Адрес 2")]
        public string address2 { get; set; }
        [Column("ADDRESS3", "Адрес 3")]
        public string address3 { get; set; }
        [Column("ADDRESS1", "Адрес 1")]
        public string address1 { get; set; }
        [Column("CITY", "Город")]
        public string city { get; set; }
        [Column("STATE_CODE", "Код области")]
        public string stateCode { get; set; }
        [Column("STATE", "Штат(область)")]
        public string state { get; set; }
        [Column("COUNTRY", "Страна")]
        public string country { get; set; }
        [Column("CONTACT_PHONE1", "Контактный телефон1")]
        public string contactPhone1 { get; set; }
        [Column("CONTACT_FAX", "Контактный факс")]
        public string contactFax { get; set; }
        [Column("CONTACT_PHONE2", "Контактный телефон2")]
        public string contactPhone2 { get; set; }
        [Column("CONTACT_MOBILE", "Контактный телефон")]
        public string contactMobile { get; set; }
        [Column("EDI_ADDRESS", "EDI адрес")]
        public string ediAddress { get; set; }
        [Column("EMAIL_ADDRESS", "Электронная почта")]
        public string emailAddress { get; set; }
        [Column("SPECIAL_NOTES", "Специальные заметки")]
        public string specialNotes { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        public int modUserId { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        public DateTime modDateTime { get; set; }
        [Column("RECEIVING_PHONE", "Телефон получателя")]
        public string receivingPhone { get; set; }
        [Column("COMPANY_PHONE", "Телефон компании")]
        public string companyPhone { get; set; }
        [Column("CONTACT_NAME2", "Имя контакного лица2")]
        public string contactName2 { get; set; }
        [Column("COMPANY_FAX", "Факс компании")]
        public string companyFax { get; set; }
        [Column("COMPANY_EMAIL", "Электронная почта компании")]
        public string companyEmail { get; set; }
        [Column("CONTACT_POSITION", "Должность контактного лица")]
        public string contactPosition { get; set; }
        [Column("COMPANY_WEB_SITE", "Веб-сайт компании")]
        public string companyWebSite { get; set; }
    }
}
