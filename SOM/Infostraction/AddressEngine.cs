using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Repo;
using SOM.Models;

namespace SOM.Infostraction
{
    public class AddressEngine : IAddressCat
    {
        AddresCat adressCatalog;

        public AddressEngine()
        {
            this.adressCatalog = new AddresCat();
        }

        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }

        public string GetAddressId()
        {
            throw new NotImplementedException();
        }

        public int LoadData(int addressId)
        {
            throw new NotImplementedException();
        }

        public bool SaveData(bool action)
        {
            throw new NotImplementedException();
        }

        public bool SetAddress1(string address1)
        {
            throw new NotImplementedException();
        }

        public bool SetAddress2(string address2)
        {
            throw new NotImplementedException();
        }

        public bool SetAddress3(string address3)
        {
            throw new NotImplementedException();
        }

        public bool SetAddressFullName(string custName)
        {
            throw new NotImplementedException();
        }

        public bool SetCity(string city)
        {
            throw new NotImplementedException();
        }

        public bool SetContactFax(string contactFax)
        {
            throw new NotImplementedException();
        }

        public bool SetContactMobile(string contactMobile)
        {
            throw new NotImplementedException();
        }

        public bool SetContactName(string contactName)
        {
            throw new NotImplementedException();
        }

        public bool SetContactPhone1(string contactPhone)
        {
            throw new NotImplementedException();
        }

        public bool SetCountry(string country)
        {
            throw new NotImplementedException();
        }

        public bool SetEmailAddress(string contactEmail)
        {
            throw new NotImplementedException();
        }

        public bool SetState(string state)
        {
            throw new NotImplementedException();
        }

        public bool SetZipCode(string zimCode)
        {
            throw new NotImplementedException();
        }
    }
}
