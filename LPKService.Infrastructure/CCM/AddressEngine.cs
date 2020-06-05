using System;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Interfaces;
using LPKService.Infrastructure.Work;
using Oracle.ManagedDataAccess.Client;
using LPKService.Repository;
using Dapper;
using Dapper.Oracle;

namespace LPKService.Infrastructure.CCM
{
    public class AddressEngine : CCMRepoBase,IAddressEngine
    {
        AddresCat adressCatalog;
        bool exist=false;
        public AddressEngine(TL4EngineInterfaceMngRepo interfaceMng)
        {
            this.adressCatalog = new AddresCat();
        }

        public string GetAddressId()
        {
            return adressCatalog.addressId.ToString();
        }
        
        public int LoadData(int addressId)
        {
            int count=-1;
            string sqlstr = $"SELECT COUNT(*) FROM ADDRESS_CATALOG WHERE ADDRESS_ID = {addressId}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            if (count > 0)
            {
                GetData(addressId);
                exist = true;
            }
            return count;
        }
        public void GetData(int addressId)
        {
            string sqlstr = $"SELECT * FROM ADDRESS_CATALOG WHERE ADDRESS_ID = {addressId}";
            using (OracleConnection connection = GetConnection())
            {
                adressCatalog = connection.QueryFirstOrDefault<AddresCat>(sqlstr, null);
            }
        }
        public bool SaveData()
        {
            bool res = false;
            if (!exist)
            {
                try
                {
                    OracleDynamicParameters odp = new OracleDynamicParameters();
                    string sqlstr = "INSERT INTO ADDRESS_CATALOG (ADDRESS_ID,ADDRESS_FULL_NAME,CONTACT_NAME,ZIP_CODE," +
                        "ADDRESS2,ADDRESS3,ADDRESS1,CITY,STATE_CODE,STATE,COUNTRY,CONTACT_PHONE1,CONTACT_FAX,CONTACT_PHONE2,CONTACT_MOBILE,EDI_ADDRESS," +
                        "EMAIL_ADDRESS,SPECIAL_NOTES,MOD_USER_ID,MOD_DATETIME,RECEIVING_PHONE,COMPANY_PHONE,CONTACT_NAME2,COMPANY_FAX,COMPANY_EMAIL,CONTACT_POSITION,COMPANY_WEB_SITE)" +
                        "VALUES(:P_ADDRESS_ID,:P_ADDRESS_FULL_NAME,:P_CONTACT_NAME,:P_ZIP_CODE,:P_ADDRESS2,:P_ADDRESS3,:P_ADDRESS1,:P_CITY,:P_STATE_CODE,:P_STATE," +
                        ":P_COUNTRY,:P_CONTACT_PHONE1,:P_CONTACT_FAX,:P_CONTACT_PHONE2,:P_CONTACT_MOBILE,:P_EDI_ADDRESS,:P_EMAIL_ADDRESS,:P_SPECIAL_NOTES,:P_MOD_USER_ID," +
                        "SYSDATE,:P_RECEIVING_PHONE,:P_COMPANY_PHONE,:P_CONTACT_NAME2,:P_COMPANY_FAX,:P_COMPANY_EMAIL,:P_CONTACT_POSITION,:P_COMPANY_WEB_SITE)";
                    odp.Add("P_ADDRESS_ID", adressCatalog.addressId);
                    odp.Add("P_ADDRESS_FULL_NAME", adressCatalog.addressFullName);
                    odp.Add("P_CONTACT_NAME", adressCatalog.contactName);
                    odp.Add("P_ZIP_CODE", adressCatalog.zipCode);
                    odp.Add("P_ADDRESS2", adressCatalog.address2);
                    odp.Add("P_ADDRESS3", adressCatalog.address3);
                    odp.Add("P_ADDRESS1", adressCatalog.address1);
                    odp.Add("P_CITY", adressCatalog.city);
                    odp.Add("P_STATE_CODE", adressCatalog.stateCode);
                    odp.Add("P_STATE", adressCatalog.state);
                    odp.Add("P_COUNTRY", adressCatalog.country);
                    odp.Add("P_CONTACT_PHONE1", adressCatalog.contactPhone1);
                    odp.Add("P_CONTACT_FAX", adressCatalog.contactFax);
                    odp.Add("P_CONTACT_PHONE2", adressCatalog.contactPhone2);
                    odp.Add("P_CONTACT_MOBILE", adressCatalog.contactMobile);
                    odp.Add("P_EDI_ADDRESS", adressCatalog.ediAddress);
                    odp.Add("P_EMAIL_ADDRESS", adressCatalog.ediAddress);
                    odp.Add("P_SPECIAL_NOTES", adressCatalog.specialNotes);
                    odp.Add("P_MOD_USER_ID", adressCatalog.modUserId);
                    odp.Add("P_RECEIVING_PHONE", adressCatalog.receivingPhone);
                    odp.Add("P_COMPANY_PHONE", adressCatalog.companyPhone);
                    odp.Add("P_CONTACT_NAME2", adressCatalog.contactName2);
                    odp.Add("P_COMPANY_FAX", adressCatalog.companyFax);
                    odp.Add("P_COMPANY_EMAIL", adressCatalog.companyEmail);
                    odp.Add("P_CONTACT_POSITION", adressCatalog.contactPosition);
                    odp.Add("P_COMPANY_WEB_SITE", adressCatalog.companyWebSite);
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
            else
            {
                try
                {
                    OracleDynamicParameters odp = new OracleDynamicParameters();
                    string sqlstr = "UPDATE ADDRESS_CATALOG SET (ADDRESS_FULL_NAME,CONTACT_NAME,ZIP_CODE," +
                        "ADDRESS2,ADDRESS3,ADDRESS1,CITY,STATE_CODE,STATE,COUNTRY,CONTACT_PHONE1,CONTACT_FAX,CONTACT_PHONE2,CONTACT_MOBILE,EDI_ADDRESS," +
                        "EMAIL_ADDRESS,SPECIAL_NOTES,MOD_USER_ID,MOD_DATETIME,RECEIVING_PHONE,COMPANY_PHONE,CONTACT_NAME2,COMPANY_FAX,COMPANY_EMAIL,CONTACT_POSITION,COMPANY_WEB_SITE)" +
                        "VALUES(:P_ADDRESS_FULL_NAME,:P_CONTACT_NAME,:P_ZIP_CODE,:P_ADDRESS2,:P_ADDRESS3,:P_ADDRESS1,:P_CITY,:P_STATE_CODE,:P_STATE," +
                        ":P_COUNTRY,:P_CONTACT_PHONE1,:P_CONTACT_FAX,:P_CONTACT_PHONE2,:P_CONTACT_MOBILE,:P_EDI_ADDRESS,:P_EMAIL_ADDRESS,:P_SPECIAL_NOTES,:P_MOD_USER_ID," +
                        "SYSDATE,:P_RECEIVING_PHONE,:P_COMPANY_PHONE,:P_CONTACT_NAME2,:P_COMPANY_FAX,:P_COMPANY_EMAIL,:P_CONTACT_POSITION,:P_COMPANY_WEB_SITE) WHERE ADDRESS_ID=:P_ADDRESS_ID";
                    odp.Add("P_ADDRESS_ID", adressCatalog.addressId);
                    odp.Add("P_ADDRESS_FULL_NAME", adressCatalog.addressFullName);
                    odp.Add("P_CONTACT_NAME", adressCatalog.contactName);
                    odp.Add("P_ZIP_CODE", adressCatalog.zipCode);
                    odp.Add("P_ADDRESS2", adressCatalog.address2);
                    odp.Add("P_ADDRESS3", adressCatalog.address3);
                    odp.Add("P_ADDRESS1", adressCatalog.address1);
                    odp.Add("P_CITY", adressCatalog.city);
                    odp.Add("P_STATE_CODE", adressCatalog.stateCode);
                    odp.Add("P_STATE", adressCatalog.state);
                    odp.Add("P_COUNTRY", adressCatalog.country);
                    odp.Add("P_CONTACT_PHONE1", adressCatalog.contactPhone1);
                    odp.Add("P_CONTACT_FAX", adressCatalog.contactFax);
                    odp.Add("P_CONTACT_PHONE2", adressCatalog.contactPhone2);
                    odp.Add("P_CONTACT_MOBILE", adressCatalog.contactMobile);
                    odp.Add("P_EDI_ADDRESS", adressCatalog.ediAddress);
                    odp.Add("P_EMAIL_ADDRESS", adressCatalog.ediAddress);
                    odp.Add("P_SPECIAL_NOTES", adressCatalog.specialNotes);
                    odp.Add("P_MOD_USER_ID", adressCatalog.modUserId);
                    odp.Add("P_RECEIVING_PHONE", adressCatalog.receivingPhone);
                    odp.Add("P_COMPANY_PHONE", adressCatalog.companyPhone);
                    odp.Add("P_CONTACT_NAME2", adressCatalog.contactName2);
                    odp.Add("P_COMPANY_FAX", adressCatalog.companyFax);
                    odp.Add("P_COMPANY_EMAIL", adressCatalog.companyEmail);
                    odp.Add("P_CONTACT_POSITION", adressCatalog.contactPosition);
                    odp.Add("P_COMPANY_WEB_SITE", adressCatalog.companyWebSite);
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

        public void SetAddress1(string address1)
        {
            adressCatalog.address1 = address1;
        }

        public void SetAddress2(string address2)
        {
            adressCatalog.address2 = address2;
        }

        public void SetAddress3(string address3)
        {
            adressCatalog.contactMobile = address3;
        }

        public void SetAddressFullName(string custName)
        {
            adressCatalog.addressFullName = custName;
        }

        public void SetCity(string city)
        {
            adressCatalog.city = city;
        }

        public void SetContactFax(string contactFax)
        {
            adressCatalog.contactFax = contactFax;
        }

        public void SetContactMobile(string contactMobile)
        {
            adressCatalog.contactMobile = contactMobile;
        }

        public void SetContactName(string contactName)
        {
            adressCatalog.contactName = contactName;
        }

        public void SetContactPhone1(string contactPhone)
        {
            adressCatalog.contactPhone1 = contactPhone;
        }

        public void SetCountry(string country)
        {
            adressCatalog.country = country;
        }

        public void SetEmailAddress(string contactEmail)
        {
            adressCatalog.emailAddress = contactEmail;
        }

        public void SetState(string state)
        {

            adressCatalog.state = state;
        }

        public void SetZipCode(string zimCode)
        {
            adressCatalog.zipCode = zimCode;
        }
    }
}
