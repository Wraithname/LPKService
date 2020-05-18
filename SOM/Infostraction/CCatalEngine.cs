using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;

namespace SOM.Infostraction
{
    public class CCatalEngine:ICCatalEngine
    {
        CustomerCat customerCat;

        public CCatalEngine()
        {
            this.customerCat = new CustomerCat();
        }

        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int customID)
        {
            throw new NotImplementedException();
        }

        public bool ForceModUserDatetime(bool flag, string modUserId, string txt = "")
        {
            throw new NotImplementedException();
        }

        public int GetCustomerID()
        {
            throw new NotImplementedException();
        }

        public DateTime GetExpirationDate()
        {
            throw new NotImplementedException();
        }

        public bool IsCustomerDeletable(int customID)
        {
            throw new NotImplementedException();
        }

        public int LoadData(string id)
        {
            throw new NotImplementedException();
        }

        public bool SaveData(bool flag)
        {
            throw new NotImplementedException();
        }

        public bool SetAddressIdCatalog(string addressId)
        {
            throw new NotImplementedException();
        }

        public bool SetClassificationType(string customertype)
        {
            throw new NotImplementedException();
        }

        public bool SetCreationUserId(string customerId)
        {
            throw new NotImplementedException();
        }

        public bool SetCustomerCurrencyCode(string customercode)
        {
            throw new NotImplementedException();
        }

        public bool SetCustomerDescrId(string l4CustimerId)
        {
            throw new NotImplementedException();
        }

        public bool SetCustomerShortName(string value)
        {
            throw new NotImplementedException();
        }

        public bool SetExpirationDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public bool SetInn(string inn)
        {
            throw new NotImplementedException();
        }

        public bool SetInquiryValidityDays(int number)
        {
            throw new NotImplementedException();
        }

        public bool SetInternalCustomerFlag(bool flag)
        {
            throw new NotImplementedException();
        }

        public bool SetKpp(string kpp)
        {
            throw new NotImplementedException();
        }

        public bool SetLevel4CustomerId(string customerIdforL4)
        {
            throw new NotImplementedException();
        }

        public bool SetRegion(string region)
        {
            throw new NotImplementedException();
        }

        public bool SetRwStationCode(string rwstcode)
        {
            throw new NotImplementedException();
        }

        public bool SetWeightUnit(string value)
        {
            throw new NotImplementedException();
        }
    }
}
