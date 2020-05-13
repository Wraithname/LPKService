using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;
using Oracle.ManagedDataAccess.Client;
using Repository;
using Dapper.Oracle;
using Dapper;

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
            string res = "";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT nvl(BOL_ID,'' '') as BOL_ID FROM L4_L3_DELIVERY WHERE MSG_COUNTER = :P_MSG_COUNTER ";
            odp.Add("P_MSG_COUNTER", msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res= connection.QueryFirstOrDefault<string>(str, odp);
            }
            if (res != "")
                return res;
            return "-";
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
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "UPDATE L4_L3_EVENT SET   MSG_STATUS  = %p, MSG_REMARK  = %p WHERE MSG_COUNTER = %p ";
            odp.Add("P_MSG_STATUS", l4MsgInfo.msgReport.status);
            odp.Add("P_MSG_REMARK", l4MsgInfo.msgReport.remark);
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(str, odp);
            }
        }

        public void UpdateStatusMessage(int msgCounter, int status, string remark)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "UPDATE L4_L3_DELIVERY_EVENT SET MSG_STATUS = :P_MSG_STATUS, MSG_REMARK = :P_MSG_REMARK, " +
                "MSG_PROCCESS_DATETIME = SYSDATE WHERE  MSG_COUNTER = :P_MSG_COUNTER";
            odp.Add("P_MSG_STATUS", status);
            odp.Add("P_MSG_REMARK", remark);
            odp.Add("P_MSG_COUNTER", msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(str, odp);
            }
        }
    }
}
