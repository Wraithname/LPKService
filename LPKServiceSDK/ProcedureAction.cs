using System;
using System.Collections.Generic;
using SOM.Infostraction;
using Shipping.Models;
using Shipping.Infostraction;
using Work.Models;
using Work.Infostraction;

namespace LPKServiceSDK
{
    class ProcedureAction
    {
        
        public List<Action<Work.Models.TL4MsgInfo>> GetActions()
        {
            List<Action<Work.Models.TL4MsgInfo>> actions = new List<Action<Work.Models.TL4MsgInfo>>();
            Action<Work.Models.TL4MsgInfo> targets;
            //L4_L3_SALES_ORDER
            L4L3SoHeaderRepo soHeaderRepo = new L4L3SoHeaderRepo();
            SOM.SOManagment som = new SOM.SOManagment();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) {
                L4L3SoHeader soHeader = soHeaderRepo.GetData(l4MsgInfo);
                som.SalesOrderMng(soHeader,l4MsgInfo); };
            actions.Add(targets);
            //L4_L3_CUSTOMER_CATALOG
            SOM.CCManagement custom = new SOM.CCManagement();
            L4L3CustomerRepo customerRepo = new L4L3CustomerRepo();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) {
                L4L3Customer customers = customerRepo.GetData(l4MsgInfo);
                custom.CustomerMng(customers, l4MsgInfo); };
            actions.Add(targets);
            //L4_L3_SHIPPING
            Shipping.L4L3ServiceShipping shipp = new Shipping.L4L3ServiceShipping();
            L4L3ShippingRepo shippingRepo = new L4L3ShippingRepo();
            targets = delegate (Work.Models.TL4MsgInfo l4MsgInfo) {
                L4L3Shipping shipping = shippingRepo.GetData(l4MsgInfo);
                shipp.ShippingMng(shipping,l4MsgInfo); };
            actions.Add(targets);
            return actions;
        }
    }
}
