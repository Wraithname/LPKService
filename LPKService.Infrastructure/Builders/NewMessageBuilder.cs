using LPKService.Domain.Models.Work.Event;
using LPKService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.Shipping;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using NLog;
using LPKService.Repository;
using LPKService.Domain.Models.Work;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.SOM;

namespace LPKService.Infrastructure.Builders
{
    public interface INewMessageBuilder
    {
        void CloseOrder();
        void NewMessage(); 
    }
    public class NewMessageBuilder : INewMessageBuilder
    {
        private Logger logger = LogManager.GetLogger(nameof(NewMessageBuilder));
        private readonly IGlobalCheck check;
        private readonly ICCManagement ccm ;
        private readonly ISOManagment som ;
        private readonly IL4L3SerShipping sship;

        public NewMessageBuilder(IGlobalCheck check, ICCManagement ccm,ISOManagment som,IL4L3SerShipping sship)
        {
            this.check = check;
            this.ccm = ccm;
            this.som = som;
            this.sship = sship;
        }

        public void CloseOrder()
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str1 = "UPDATE  SALES_ORDER_LINE " +
                "SET SO_LINE_STATUS = 4 " +
                " , MOD_DATETIME = sysdate" +
                ", MANUAL_CLOSE_ORDER = 'Y' " +
                "WHERE SO_ID = :P_SO_ID " +
                "AND   SO_LINE_ID = :P_SO_LINE_ID";
            odp.Add("P_SO_ID");
            odp.Add("P_SO_LINE_ID");
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(str1, odp);
            }
            try
            {
                string str2 = "SELECT l2.*, " +
                    "sol.so_id as met_so_id, " +
                    "sol.so_line_id as met_so_line_id " +
                    "FROM (  select l.MSG_DATETIME, l.MSG_COUNTER, sh5.so_id , sh5.so_line_id ,sh5.ORDER_STATUS, sh5.SO_TYPE_CODE " +
                    "FROM L4_L3_EVENT l join(select shm.MSG_COUNTER, shm.SO_ID , shm.so_line_id, shm.ORDER_STATUS, shm.SO_TYPE_CODE " +
                    "FROM ( SELECT MAX(sh.MSG_COUNTER) as MSG_COUNTER , sh.SO_ID , sl.so_line_id , sl.ORDER_STATUS, sl.SO_TYPE_CODE " +
                    "FROM L4_L3_SO_HEADER sh JOIN L4_L3_SO_LINE sl ON sh.MSG_COUNTER =sl.MSG_COUNTER " +
                    "WHERE sl.ORDER_STATUS =40 " +
                    "GROUP BY sh.SO_ID , sl.so_line_id, sl.ORDER_STATUS, sl.SO_TYPE_CODE) shm ) sh5 on sh5.MSG_COUNTER=l.MSG_COUNTER " +
                    " WHERE l.MSG_STATUS = 1 " +
                    "AND   l.msg_id = 4301 ) l2 join SALES_ORDER_LINE sol on sol.SO_DESCR_ID = l2.so_id or sol.SO_DESCR_ID = l2.so_id||''_''||to_number(l2.so_line_id)/10 " +
                    "WHERE sol.SO_LINE_STATUS = 3 " +
                    "AND    l2.MSG_DATETIME > SYSDATE - 7";
            }
            catch
            {

            }
            throw new NotImplementedException();
        }

        public void NewMessage()
        {
            TL4MsgInfo l4MsgInfo = new TL4MsgInfo();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            bool isOpCodeOk;
            string acceptOrderConsts = "";
            string sqlstr = "SELECT CHAR_VALUE " +
                "FROM AUX_CONSTANT " +
                "WHERE CONSTANT_ID='ACCEPT_ORDER_IN_SRV'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                acceptOrderConsts = connection.QueryFirstOrDefault<string>(sqlstr, odp);
            }
            if (acceptOrderConsts == "")
                acceptOrderConsts = "N";
            List<L4L3Event> events = new List<L4L3Event>();
            sqlstr = "SELECT * " +
                "FROM ( " +
                "SELECT le.* FROM L4_L3_EVENT le WHERE MSG_STATUS = 1 AND msg_id IN (4303, 4304, 4305) AND le.MSG_DATETIME > SYSDATE - 7 " +
                "UNION ALL select l.* " +
                "join (select shm.MSG_COUNTER FROM ( SELECT MAX(sh.MSG_COUNTER) as MSG_COUNTER , sh.SO_ID FROM L4_L3_SO_HEADER sh JOIN L4_L3_SO_LINE sl ON sh.MSG_COUNTER =sl.MSG_COUNTER" +
                "WHERE sh.STATUS =20 GROUP BY sh.SO_ID) shm ) sh5 on sh5.MSG_COUNTER=l.MSG_COUNTER" +
                "WHERE l.MSG_STATUS = 1 " +
                "AND   l.msg_id = 4301 " +
                "AND   l.MSG_DATETIME > SYSDATE - 7 " +
                "UNION ALL" +
                "SELECT st.* FROM L4_L3_EVENT st WHERE MSG_STATUS = 1 " +
                "AND msg_id IN (4311,4312,4313,4314,4315)" +
                ") ORDER BY MSG_COUNTER ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                events = connection.Query<L4L3Event>(sqlstr, odp).AsList();
            }
            if (events == null)
            {
                logger.Error("SQL запрос из таблицы L4_L3_EVENT выполнен с ошибков в ServiceWorker.GetNewMessage.");
                throw new Exception();
            }
            foreach (L4L3Event evnt in events)
            {
                l4MsgInfo.msgCounter = evnt.msgCounter;
                l4MsgInfo.msgId = evnt.msgId;
                l4MsgInfo.msgDatetime = evnt.msgDatetime;
                l4MsgInfo.opCode = evnt.opCode;
                l4MsgInfo.keyString1 = evnt.keyString1;
                l4MsgInfo.keyString2 = evnt.keyString2;
                l4MsgInfo.keyNumber1 = evnt.keyNumber1;
                l4MsgInfo.keyNumber2 = evnt.keyNumber2;
                l4MsgInfo.msgReport.status = 1;
                l4MsgInfo.msgReport.remark = "";
                logger.Info($"STARTED Event -> Table: L4_L3_EVENT, MsgCounter:{l4MsgInfo.msgCounter}");
                using (OracleConnection conn = BaseRepo.GetDBConnection())
                {
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            switch (l4MsgInfo.opCode)
                            {
                                case L4L3InterfaceServiceConst.OP_CODE_NEW:
                                    isOpCodeOk = true;
                                    break;
                                case L4L3InterfaceServiceConst.OP_CODE_DEL:
                                    isOpCodeOk = true;
                                    break;
                                case L4L3InterfaceServiceConst.OP_CODE_UPD:
                                    isOpCodeOk = true;
                                    break;
                                case L4L3InterfaceServiceConst.OP_CODE_INUP:
                                    isOpCodeOk = true;
                                    break;
                                default:
                                    isOpCodeOk = false;
                                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, $"OP_CODE: {l4MsgInfo.opCode} is not valid");
                                    UpdateMsgStatus(l4MsgInfo);
                                    break;
                            }
                            if (isOpCodeOk)
                            {
                                switch (l4MsgInfo.msgId)
                                {
                                    //Запуск задачи SALES_ORDER
                                    case L4L3InterfaceServiceConst.L4_L3_SALES_ORDER:
                                        if (IsBlocked(evnt.msgCounter))
                                        {
                                            Task.Run(() => som.SalesOrderMng(l4MsgInfo));
                                        }
                                        break;
                                    //Запуск задачи CUSTOMER_CATALOG
                                    case L4L3InterfaceServiceConst.L4_L3_CUSTOMER_CATALOG:
                                        Task.Run(() => ccm.CustomerMng(l4MsgInfo));
                                        break;
                                    //Запуск задачи SHIPPING
                                    case L4L3InterfaceServiceConst.L4_L3_SHIPPING:
                                        Task.Run(() => sship.ShippingMng(l4MsgInfo));
                                        break;
                                }
                                if (l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_INSERT)
                                    UpdateMsgStatus(l4MsgInfo);
                            }
                            logger.Info($"STOPPED Event -> Table: L4_L3_EVENT, MsgCounter:{l4MsgInfo.msgCounter}");
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            if (IsBlocked(l4MsgInfo.msgCounter))
                                BlockForProcess(l4MsgInfo, false);
                            using (OracleConnection conn1 = BaseRepo.GetDBConnection())
                            {
                                using (var transaction1 = conn.BeginTransaction())
                                {
                                    try
                                    {
                                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, e.Message.Substring(0, 4000));
                                        UpdateMsgStatus(l4MsgInfo);
                                        transaction1.Commit();
                                    }
                                    catch
                                    {
                                        transaction1.Rollback();
                                        if (IsBlocked(l4MsgInfo.msgCounter))
                                            BlockForProcess(l4MsgInfo, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateMsgStatus(TL4MsgInfo l4MsgInfo)
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
        private bool IsBlocked(int msgCounter)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            int res = 0;
            string sqlstr = "SELECT BLOCK_FOR_PROCESS " +
                "FROM l4_l3_event " +
                "WHERE msg_counter = :P_MSG_COUNTER ";
            odp.Add("P_MSG_COUNTER", msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.QueryFirstOrDefault<int>(sqlstr, odp);
            }
            if (res == 0)
                return false;
            else if (res == 1)
                return true;
            return false;
        }
        private void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer)
        {
            string sqlstr = "";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            if (serRSer)
                sqlstr = "Select UPDATE_BLOCK_ORDER(1,:msg_counter) FROM DUAL";
            else
                sqlstr = "Select UPDATE_BLOCK_ORDER(0,:msg_counter) FROM DUAL";
            odp.Add("msg_counter", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(sqlstr, odp);
            }
        }
    }
}
