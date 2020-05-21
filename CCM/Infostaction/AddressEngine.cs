using System;
using CCM.Repo;
using CCM.Models;
using Repository.WorkModels;

namespace CCM.Infostraction
{
    public class AddressEngine : IAddressCat
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
        public bool SaveData(bool action)
        {
            throw new NotImplementedException();
        }

        public bool SetAddress1(string address1)
        {
            if (address1 != "")
            {
                adressCatalog.address1 = address1;
                return true;
            }
            return false;
        }

        public bool SetAddress2(string address2)
        {
            if (address2 != "")
            {
                adressCatalog.address2 = address2;
                return true;
            }
            return false;
        }

        public bool SetAddress3(string address3)
        {
            if (address3 != "")
            {
                adressCatalog.contactMobile = address3;
                return true;
            }
            return false;
        }

        public bool SetAddressFullName(string custName)
        {
            if (custName != "")
            {
                adressCatalog.addressFullName = custName;
                return true;
            }
            return false;
        }

        public bool SetCity(string city)
        {
            if (city != "")
            {
                adressCatalog.city = city;
                return true;
            }
            return false;
        }

        public bool SetContactFax(string contactFax)
        {
            if (contactFax != "")
            {
                adressCatalog.contactFax = contactFax;
                return true;
            }
            return false;
        }

        public bool SetContactMobile(string contactMobile)
        {
            if (contactMobile != "")
            {
                adressCatalog.contactMobile = contactMobile;
                return true;
            }
            return false;
        }

        public bool SetContactName(string contactName)
        {
            if (contactName != "")
            {
                adressCatalog.contactName = contactName;
                return true;
            }
            return false;
        }

        public bool SetContactPhone1(string contactPhone)
        {
            if (contactPhone != "")
            {
                adressCatalog.contactPhone1 = contactPhone;
                return true;
            }
            return false;
        }

        public bool SetCountry(string country)
        {
            if (country != "")
            {
                adressCatalog.country = country;
                return true;
            }
            return false;
        }

        public bool SetEmailAddress(string contactEmail)
        {
            if (contactEmail != "")
            {
                adressCatalog.emailAddress = contactEmail;
                return true;
            }
            return false;
        }

        public bool SetState(string state)
        {
            if (state != "")
            {
                adressCatalog.state = state;
                return true;
            }
            return false;
        }

        public bool SetZipCode(string zimCode)
        {
            if (zimCode != "")
            {
                adressCatalog.zipCode = zimCode;
                return true;
            }
            return false;
        }
    }
}
