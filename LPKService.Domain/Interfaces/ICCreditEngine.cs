using LPKService.Domain.Models.Work;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса CCreditEngine
    /// </summary>
    public interface ICCreditEngine
    {
        void Create(TL4EngineInterfaceMng interfaceMng);
        bool SetCustomerID(int custIdFromDscr);
        bool SetAddressIdBillTo(int addressId);
        bool SetCreditStatus(int num);
        bool SaveData(bool flag);
        bool ForceModUserDatetime(bool falg, string modUserId, string str = "");
        int LoadData(int num);
        int GetAddressIdBillTo();
    }
}
