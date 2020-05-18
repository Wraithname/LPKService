using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;

namespace SOM.Repo
{
    interface ICCreditEngine
    {
        void Create(TL4EngineInterfaceMng interfaceMng);
        bool SetCustomerID(int custIdFromDscr);
        bool SetAddressIdBillTo(int addressId);
        bool SetCreditStatus(int num);
        bool SaveData(bool flag);
        bool ForceModUserDatetime(bool falg, string modUserId, string str = "");
    }
}
