using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;

namespace LPKServiceSDK
{

    public class ServiceWork : IServiceWork
    {
        List<Action<Work.Models.TL4MsgInfo>> actions = new List<Action<Work.Models.TL4MsgInfo>>();

        public ServiceWork()
        {
            ProcedureAction procedure = new ProcedureAction();
            actions = procedure.GetActions();
        }

        public void CreateBol(int msgCounter, string bolId, bool allBol)
        {
            throw new NotImplementedException();
        }

        public void DeleteBol(int msgCounter, string bolId, int posNumId)
        {
            throw new NotImplementedException();
        }

        public bool ExistBolPosition(string bolId, string posId)
        {
            throw new NotImplementedException();
        }

        public bool ExistsBol(string bolId, bool selChild)
        {
            throw new NotImplementedException();
        }

        public void GetAutoCloseOrder()
        {
            throw new NotImplementedException();
        }

        public string GetBolId(int msgCounter)
        {
            throw new NotImplementedException();
        }

        public void GetNewMessage()
        {
            throw new NotImplementedException();
        }

        public void GetNewMessageDelivery()
        {
            throw new NotImplementedException();
        }

        public bool IsUpdate(int msgCounter, string bolId, int bolPosNumId)
        {
            throw new NotImplementedException();
        }

        public void MngLoop()
        {
            throw new NotImplementedException();
        }

        public void UpdateBolPosition(int msgCounter, int posNumId)
        {
            throw new NotImplementedException();
        }

        public void UpdateMsgStatus(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusMessage(int msgCounter, int status, string remark)
        {
            throw new NotImplementedException();
        }
    }
}
