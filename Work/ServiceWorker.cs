using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using System.Threading.Tasks;
using LPKService.Infrastructure.Builders;

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
            INewMessageBuilder newMessageBuilder = new NewMessageBuilder();
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
