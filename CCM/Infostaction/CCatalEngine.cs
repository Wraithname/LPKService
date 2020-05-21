using System;
using CCM.Models;
using CCM.Repo;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;

namespace CCM.Infostraction
{
    public class CCatalEngine:ICCatalEngine
    {
        CustomerCat customerCat;

        public CCatalEngine()
        {
            this.customerCat = new CustomerCat();
        }
        //Узнать про функцию
        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int customID)
        {
            OracleDynamicParameters odp = null;
            string str = $"DELETE * FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID=:{customID}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(str, odp);
            }
        }
        //Узнать про функцию
        public bool ForceModUserDatetime(bool flag, string modUserId, string txt = "")
        {
            throw new NotImplementedException();
        }

        public int GetAddressIdCatalog()
        {
            return customerCat.addresId;
        }

        public int GetCustomerID()
        {
            return customerCat.custimerId;
        }

        public DateTime GetExpirationDate()
        {
            return customerCat.expirationDate;
        }
        //Узнать про функцию
        public bool IsCustomerDeletable(int customID)
        {
            return true;
        }
        //Узнать про функцию
        public int LoadData(string id)
        {
            throw new NotImplementedException();
        }
        //Узнать про функцию
        public bool SaveData(bool flag)
        {
            throw new NotImplementedException();
        }

        public bool SetAddressIdCatalog(string addressId)
        {
            try
            {
                customerCat.addresId = Convert.ToInt32(addressId);
                return true;
            }
            catch { return false; }
        }

        public bool SetClassificationType(string customertype)
        {
            try
            {
                customerCat.customerType = customertype;
                return true;
            }
            catch { return false; }
        }

        public bool SetCreationUserId(string customerId)
        {
            try
            {
                customerCat.creationUserId = Convert.ToInt32(customerId);
                return true;
            }
            catch { return false; }
        }

        public bool SetCustomerCurrencyCode(string customercode)
        {
            try
            {
                customerCat.customerCurrencyCode = customercode;
                return true;
            }
            catch { return false; }
        }

        public bool SetCustomerDescrId(string l4CustimerId)
        {
            try
            {
                customerCat.customerDescrId = l4CustimerId;
                return true;
            }
            catch { return false; }
        }

        public bool SetCustomerShortName(string value)
        {
            try
            {
                customerCat.customerShortName = value;
                return true;
            }
            catch { return false; }
        }

        public bool SetExpirationDate(DateTime date)
        {
            try
            {
                customerCat.expirationDate = date;
                return true;
            }
            catch { return false; }
        }

        public bool SetInn(string inn)
        {
           try
            {
                customerCat.inn = inn;
                return true;
            }
            catch { return false; }
        }

        public bool SetInquiryValidityDays(int number)
        {
            try
            {
                customerCat.inquiryValidityDays = number;
                return true;
            }
            catch { return false; }
        }

        public bool SetInternalCustomerFlag(bool flag)
        {
            try
            {
                customerCat.internalCustomerFlag = BaseRepo.BoolToChar(flag);
                return true;
            }
            catch { return false; }
        }

        public bool SetKpp(string kpp)
        {
            try
            {
                customerCat.kpp = kpp;
                return true;
            }
            catch { return false; }
        }

        public bool SetLevel4CustomerId(string customerIdforL4)
        {
            try
            {
                customerCat.level4CustomerId = customerIdforL4;
                return true;
            }
            catch { return false; }
        }

        public bool SetRegion(string region)
        {
            try
            {
                customerCat.region = region;
                return true;
            }
            catch { return false; }
        }

        public bool SetRwStationCode(string rwstcode)
        {
            try
            {
                customerCat.rwstationCode = rwstcode;
                return true;
            }
            catch { return false; }
        }

        public bool SetWeightUnit(string value)
        {
            try
            {
                customerCat.customerWeightUnitId = value;
                return true;
            }
            catch { return false; }
        }
    }
}
