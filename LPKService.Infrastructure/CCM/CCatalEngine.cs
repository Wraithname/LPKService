﻿using System;
using LPKService.Domain.Models;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Domain.BaseRepository;
using LPKService.Domain.Models.CCM;
using LPKService.Infrastructure.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCatalEngine
    {
        CustomerCat customerCat;

        public CCatalEngine(TL4EngineInterfaceMngRepo interfaceMng)
        {
            this.customerCat = new CustomerCat();
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
        public bool IsCustomerDeletable(int customID)
        {
            return true;
        }
        public int LoadData(string id)
        {
            throw new NotImplementedException();
        }
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
