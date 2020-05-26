using System;
using LPKService.Domain.Models;

namespace LPKService.Infrastructure.CCM
{
    public class CCreditEngine
    {
        CustomerCatCredit catCredit;

        public CCreditEngine()
        {
            this.catCredit = new CustomerCatCredit();
        }
        //Подумать над функционалом
        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }
        //Узнать про функцию
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
