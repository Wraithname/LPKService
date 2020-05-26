using System;
using LPKService.Domain.Models;

namespace LPKService.Infrastructure.CCM
{
    public class AddressEngine
    {
        AddresCat adressCatalog;

        public AddressEngine()
        {
            this.adressCatalog = new AddresCat();
        }
        //Узнать про функцию
        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }

        public string GetAddressId()
        {
            return adressCatalog.addressId.ToString();
        }
        //Узнать про функцию
        public int LoadData(int addressId)
        {
            throw new NotImplementedException();
        }
        //Узнать про функцию
        public bool SaveData()
        {
            throw new NotImplementedException();
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
