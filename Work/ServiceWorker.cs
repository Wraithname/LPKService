using System;
using LPKService.Infrastructure.Builders;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.SOM;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.Shipping;
using LPKService.Infrastructure.Material;

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
            INewMessageBuilder newMessageBuilder = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(),new CCManagement(), new SOManagment(),new L4L3ServiceShipping(),new Material());
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
