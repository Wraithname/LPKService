using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCM.Models;
using CCM.Repo;

namespace CCM.Infostraction
{
    public class CCreditEngine:ICCreditEngine
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
        public bool ForceModUserDatetime(bool falg, string modUserId, string str = "")
        {
            try
            {
                catCredit.modUserId = Convert.ToInt32(modUserId);
                return true;
            }
            catch { return false; }
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
        public bool SaveData(bool flag)
        {
            throw new NotImplementedException();
        }

        public bool SetAddressIdBillTo(int addressId)
        {
            try
            {
                catCredit.addressId = addressId;
                return true;
            }
            catch { return false; }
        }

        public bool SetCreditStatus(int num)
        {
           try
            {
                catCredit.creditStatus = num;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetCustomerID(int custIdFromDscr)
        {
            try
            {
                catCredit.customerId = custIdFromDscr;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
