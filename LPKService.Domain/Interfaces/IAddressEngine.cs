using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса AddressEngine
    /// </summary>
    public interface IAddressEngine
    {
        string GetAddressId();
        int LoadData(int addressId);
        bool SaveData();
        void SetAddress1(string address1);
        void SetAddress2(string address2);
        void SetAddress3(string address3);
        void SetAddressFullName(string custName);
        void SetCity(string city);
        void SetContactFax(string contactFax);
        void SetContactMobile(string contactMobile);
        void SetContactName(string contactName);
        void SetContactPhone1(string contactPhone);
        void SetCountry(string country);
        void SetEmailAddress(string contactEmail);
        void SetState(string state);
        void SetZipCode(string zimCode);
    }
}