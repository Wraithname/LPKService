using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPKServiceSDK
{
    class ProcedureAction
    {
        public List<Action<Work.Models.TL4MsgInfo>> GetActions()
        {
            List<Action<Work.Models.TL4MsgInfo>> actions = new List<Action<Work.Models.TL4MsgInfo>>();
            Action<Work.Models.TL4MsgInfo> targets;

            SOM.SOManagment som = new SOM.SOManagment();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) { som.SalesOrderMng(l4MsgInfo); };
            actions.Add(targets);
           
            SOM.CCManagement custom = new SOM.CCManagement();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) { custom.CustomerMng(l4MsgInfo); };
            actions.Add(targets);

            Shipping.L4L3ServiceShipping shipp = new Shipping.L4L3ServiceShipping();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) { shipp.ShippingMng(l4MsgInfo); };
            actions.Add(targets);

            return actions;
        }
    }
}
