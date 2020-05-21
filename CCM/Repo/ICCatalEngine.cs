using System;
using Repository.WorkModels;

namespace CCM.Repo
{
    interface ICCatalEngine
    {
        void Create(TL4EngineInterfaceMng interfaceMng);
        bool SetCustomerDescrId(string l4CustimerId);
        bool SetAddressIdCatalog(string addressId);
        bool SetInternalCustomerFlag(bool flag);
        bool SetInquiryValidityDays(int number);
        bool SetCustomerCurrencyCode(string customercode);
        bool SetClassificationType(string customertype);
        bool SetWeightUnit(string value);
        bool SetCustomerShortName(string value);
        bool SetCreationUserId(string customerId);
        bool SetInn(string inn);
        bool SetKpp(string kpp);
        bool SetRwStationCode(string rwstcode);
        bool SetRegion(string region);
        bool SetLevel4CustomerId(string customerIdforL4);
        bool SetExpirationDate(DateTime date);
        DateTime GetExpirationDate();
        bool SaveData(bool flag);
        int LoadData(string id);
        bool ForceModUserDatetime(bool flag, string modUserId, string txt = "");
        bool IsCustomerDeletable(int customID);
        int GetCustomerID();
        void DeleteCustomer(int customID);
        int GetAddressIdCatalog();
    }
}
