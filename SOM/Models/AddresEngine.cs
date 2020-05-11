using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace SOM.Models
{
    class AddresEngine
    {
        [Column("ADDRESS_ID", "Идентификатор адреса")]
        int addressId { get; set; }
        [Column("ADDRESS_FULL_NAME", "Полный адрес")]
        string addressFullName { get; set; }
        [Column("CONTACT_NAME", "Имя контакного лица")]
        string contactName { get; set; }
        [Column("ZIP_CODE", "Индекс")]
        string zipCode { get; set; }
        [Column("ADDRESS2", "Адрес 2")]
        string address2 { get; set; }
        [Column("ADDRESS3", "Адрес 3")]
        string address3 { get; set; }
        [Column("ADDRESS1", "Адрес 1")]
        string address1 { get; set; }
        [Column("CITY", "Город")]
        string city { get; set; }
        [Column("STATE_CODE", "Код области")]
        string stateCode { get; set; }
        [Column("STATE", "Штат(область)")]
        string state { get; set; }
        [Column("COUNTRY", "Страна")]
        string country { get; set; }
        [Column("CONTACT_PHONE1", "Контактный телефон1")]
        string contactPhone1 { get; set; }
        [Column("CONTACT_FAX", "Контактный факс")]
        string contactFax { get; set; }
        [Column("CONTACT_PHONE2", "Контактный телефон2")]
        string contactPhone2 { get; set; }
        [Column("CONTACT_MOBILE", "Контактный телефон")]
        string contactMobile { get; set; }
        [Column("EDI_ADDRESS", "EDI адрес")]
        string ediAddress { get; set; }
        [Column("EMAIL_ADDRESS", "Электронная почта")]
        string emailAddress { get; set; }
        [Column("SPECIAL_NOTES", "Специальные заметки")]
        string specialNotes { get; set; }
        [Column("MOD_USER_ID", "Последний изменивший пользователь")]
        int modUserId { get; set; }
        [Column("MOD_DATETIME", "Дата последнего изменения")]
        DateTime modDateTime { get; set; }
        [Column("RECEIVING_PHONE", "Телефон получателя")]
        string receivingPhone { get; set; }
        [Column("COMPANY_PHONE", "Телефон компании")]
        string companyPhone { get; set; }
        [Column("CONTACT_NAME2", "Имя контакного лица2")]
        string contactName2 { get; set; }
        [Column("COMPANY_FAX", "Факс компании")]
        string companyFax { get; set; }
        [Column("COMPANY_EMAIL", "Электронная почта компании")]
        string companyEmail { get; set; }
        [Column("CONTACT_POSITION", "Должность контактного лица")]
        string contactPosition { get; set; }
        [Column("COMPANY_WEB_SITE", "Веб-сайт компании")]
        string companyWebSite { get; set; }
    }
}
