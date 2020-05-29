using System;
using LPKService.Domain.Models;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Repository;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCatalEngine
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
        public void ForceModUserDatetime(string modUserId, string txt = "")
        {
            customerCat.modUserId = Convert.ToInt32(modUserId);
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
        public bool SaveData()
        {
            throw new NotImplementedException();
        }

        public void SetAddressIdCatalog(string addressId)
        {
            customerCat.addresId = Convert.ToInt32(addressId);
        }

        public void SetClassificationType(string customertype)
        {
            customerCat.customerType = customertype;
        }

        public void SetCreationUserId(string customerId)
        {
            customerCat.creationUserId = Convert.ToInt32(customerId);
        }

        public void SetCustomerCurrencyCode(string customercode)
        {
                customerCat.customerCurrencyCode = customercode;
        }

        public void SetCustomerDescrId(string l4CustimerId)
        {
                customerCat.customerDescrId = l4CustimerId;
        }

        public void SetCustomerShortName(string value)
        {
                customerCat.customerShortName = value;
        }

        public void SetExpirationDate(DateTime date)
        {
                customerCat.expirationDate = date;
        }

        public void SetInn(string inn)
        {
                customerCat.inn = inn;
        }

        public void SetInquiryValidityDays(int number)
        {
                customerCat.inquiryValidityDays = number;
        }

        public void SetInternalCustomerFlag(bool flag)
        {
                customerCat.internalCustomerFlag = BaseRepo.BoolToChar(flag);
        }

        public void SetKpp(string kpp)
        {
                customerCat.kpp = kpp;
        }

        public void SetLevel4CustomerId(string customerIdforL4)
        {
                customerCat.level4CustomerId = customerIdforL4;
        }

        public void SetRegion(string region)
        {
                customerCat.region = region;
        }

        public void SetRwStationCode(string rwstcode)
        {
            customerCat.rwstationCode = rwstcode;
        }
        public void SetWeightUnit(string value)
        {
                customerCat.customerWeightUnitId = value;
        }
    }
}
