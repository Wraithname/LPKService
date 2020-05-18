using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;

namespace SOM.Infostraction
{
    public class CCreditEngine:ICCreditEngine
    {
        CustomerCatCredit catCredit;

        public CCreditEngine()
        {
            this.catCredit = new CustomerCatCredit();
        }

        public void Create(TL4EngineInterfaceMng interfaceMng)
        {
            throw new NotImplementedException();
        }

        public bool ForceModUserDatetime(bool falg, string modUserId, string str = "")
        {
            throw new NotImplementedException();
        }

        public bool SaveData(bool flag)
        {
            throw new NotImplementedException();
        }

        public bool SetAddressIdBillTo(int addressId)
        {
            throw new NotImplementedException();
        }

        public bool SetCreditStatus(int num)
        {
            throw new NotImplementedException();
        }

        public bool SetCustomerID(int custIdFromDscr)
        {
            throw new NotImplementedException();
        }
    }
}
