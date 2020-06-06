using System;
using LPKService.Domain.Models.CCM;
using LPKService.Infrastructure.Work;

namespace LPKService.Infrastructure.CCM
{
    public class CCreditEngine
    {
        CustomerCatCredit catCredit;

        public CCreditEngine(TL4EngineInterfaceMngRepo interfaceMng)
        {
            this.catCredit = new CustomerCatCredit();
        }
        public void ForceModUserDatetime(string modUserId, string str = "")
        {
            catCredit.modUserId = Convert.ToInt32(modUserId);
        }

        public int GetAddressIdBillTo()
        {
            return catCredit.addressId;
        }
        //Узнать про функцию
        public int LoadData(int num)
        {
            throw new NotImplementedException();
        }
        //Узнать про функцию
        public bool SaveData()
        {
            throw new NotImplementedException();
        }

        public void SetAddressIdBillTo(int addressId)
        {
            catCredit.addressId = addressId;
        }

        public void SetCreditStatus(int num)
        {
            catCredit.creditStatus = num;
        }

        public void SetCustomerID(int custIdFromDscr)
        {
            catCredit.customerId = custIdFromDscr;
        }
    }
}
