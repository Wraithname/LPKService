using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса AddressCat
    /// </summary>
    public interface IAddressCat
    {
        void Create(TL4EngineInterfaceMng interfaceMng);
        bool SetAddressFullName(string custName);
        bool SetZipCode(string zimCode);
        bool SetAddress1(string address1);
        bool SetAddress2(string address2);
        bool SetAddress3(string address3);
        bool SetCity(string city);
        bool SetState(string state);
        bool SetCountry(string country);
        bool SetContactName(string contactName);
        bool SetContactPhone1(string contactPhone);
        bool SetContactFax(string contactFax);
        bool SetContactMobile(string contactMobile);
        bool SetEmailAddress(string contactEmail);
        bool SaveData(bool action);
        int LoadData(int addressId);
        string GetAddressId();
    }
}
