using System;
using System.Collections.Generic;
using SOM.Infostraction;
//using Shipping.Infostraction;
using Repository.WorkModels;
using Work.Infostraction;

namespace Work
{
    public class ProcedureAction
    {
        List<Action<TL4MsgInfo>> actions;
        public List<Action<TL4MsgInfo>> GetActions()
        {
            this.actions = new List<Action<TL4MsgInfo>>();
            Action<TL4MsgInfo> targets;
            //L4_L3_SALES_ORDER
            L4L3SoHeaderRepo soHeaderRepo = new L4L3SoHeaderRepo();
            SOM.SOManagment som = new SOM.SOManagment();
            targets = delegate (TL4MsgInfo l4MsgInfo) {
                som.SalesOrderMng(l4MsgInfo); };
            actions.Add(targets);
            //L4_L3_CUSTOMER_CATALOG
            CCM.CCManagement custom = new CCM.CCManagement();
            L4L3CustomerRepo customerRepo = new L4L3CustomerRepo();
            targets = delegate (TL4MsgInfo l4MsgInfo) {
                L4L3Customer cust = customerRepo.GetData(l4MsgInfo);
                custom.CustomerMng(cust,l4MsgInfo); };
            actions.Add(targets);
            //L4_L3_SHIPPING
            //Shipping.L4L3ServiceShipping shipp = new Shipping.L4L3ServiceShipping();
            //L4L3ShippingRepo shippingRepo = new L4L3ShippingRepo();
            //targets = delegate (TL4MsgInfo l4MsgInfo)
            //{
            //    shipp.ShippingMng(l4MsgInfo);
            //};
            //actions.Add(targets);
            return actions;
        }
    }
}
