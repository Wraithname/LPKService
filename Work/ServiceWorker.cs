using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using System.Threading.Tasks;
using LPKService.Infrastructure.Builders;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.SOM;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.Shipping;

namespace Work
{
    public interface IServiceWork
    {
        void MngLoop();
    }
    public class ServiceWorker : IServiceWork
    {  
        private Action getDevMsg,getMsg,closeOrd;
        public ServiceWorker()
        {
            INewMessageBuilder newMessageBuilder = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(),new CCManagement(), new SOManagment(),new L4L3ServiceShipping());
            INewDevMsg newDevMsg = new NewDevMsg();
            getDevMsg = delegate
            {
                newDevMsg.GetNewMessageDelivery();
            };
            getMsg = delegate {
                newMessageBuilder.NewMessage();
            };
            closeOrd = delegate
            {
                newMessageBuilder.CloseOrder();
            };
        }
        public void MngLoop()
        {
            getDevMsg();
            getMsg();
            closeOrd();

        }

    }
}
