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
using Repository;
using LPKService.Domain.Models.Work;
using LPKService.Domain.Models.Work.AutoCloseOrder;

namespace LPKService.Infrastructure.Builders
{
    public interface INewMessageBuilder
    {
        void CloseOrder();
        void NewMessage(); 
    }
    public class NewMessageBuilder : BuildersRepoBase,INewMessageBuilder
    {
        private Logger logger = LogManager.GetLogger(nameof(Builders));
        private readonly IGlobalCheck check;
        private readonly ICCManagement ccm ;
        private readonly ISOManagment som ;
        private readonly IL4L3SerShipping sship;
        private readonly IMaterial mat;
        private TCheckResult cheker;
        Task<TCheckResult> somtask, ccmtask, shiptask, mattask;
        /// <summary>
        /// Конструктор для осуществления работы с необходимыми обработчиками
        /// </summary>
        /// <param name="check"></param>
        /// <param name="ccm"></param>
        /// <param name="som"></param>
        /// <param name="sship"></param>
        /// <param name="mat"></param>
        public NewMessageBuilder(IGlobalCheck check, ICCManagement ccm,ISOManagment som,IL4L3SerShipping sship,IMaterial mat)
        {
            this.check = check;
            this.ccm = ccm;
            this.som = som;
            this.sship = sship;
            this.mat = mat;
        }
        /// <summary>
        /// Автозакрытие заказа
        /// </summary>
        public void CloseOrder()
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            TL4MsgInfo l4MsgInfo = new TL4MsgInfo();
            List<AutoClose> close = new List<AutoClose>();
            string str1 = "UPDATE  SALES_ORDER_LINE " +
                "SET SO_LINE_STATUS = 4 " +
                " , MOD_DATETIME = sysdate" +
                ", MANUAL_CLOSE_ORDER = 'Y' " +
                "WHERE SO_ID = :P_SO_ID " +
                "AND   SO_LINE_ID = :P_SO_LINE_ID";
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
                    "AND   l.msg_id = 4301 ) l2 join SALES_ORDER_LINE sol on sol.SO_DESCR_ID = l2.so_id or sol.SO_DESCR_ID = l2.so_id||'_'||to_number(l2.so_line_id)/10 " +
                    "WHERE sol.SO_LINE_STATUS = 3 " +
                    "AND    l2.MSG_DATETIME > SYSDATE - 7";
                using (OracleConnection connection = GetConnection())
                {
                    close=connection.Query<AutoClose>(str2, null).AsList();
                }
                if(close!=null)
                {
                    foreach (AutoClose auto in close)
                    {
                        using (OracleConnection connection = GetConnection())
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    odp.Add("P_SO_ID",auto.metSoId);
                                    odp.Add("P_SO_LINE_ID",auto.metSoLineId);
                                    connection.Execute(str1, odp, transaction);
                                    transaction.Commit();
                                    l4MsgInfo.msgCounter = auto.msgCounter;
                                    l4MsgInfo.msgReport.status = 2;
                                    l4MsgInfo.msgReport.remark = $"Заказ {auto.soId}/{auto.soLineId} Закрыт автоматически";
                                    UpdateMsgStatus(l4MsgInfo,transaction);
                                }
                                catch (Exception e)
                                {
                                    transaction.Rollback();
                                    l4MsgInfo.msgCounter = auto.msgCounter;
                                    l4MsgInfo.msgReport.status = -1;
                                    l4MsgInfo.msgReport.remark = $"Заказ {auto.soId}/{auto.soLineId} {e.Message}";
                                    UpdateMsgStatus(l4MsgInfo,transaction);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                logger.Error("Ошибка выполнения автоматического закрытия заказа");
            }
        }
        /// <summary>
        /// Распределение работы по обработчикам
        /// </summary>
        public void NewMessage()
        {
            TL4MsgInfo l4MsgInfo = new TL4MsgInfo();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            bool isOpCodeOk;
            string acceptOrderConsts = "";
            string sqlstr = "SELECT CHAR_VALUE " +
                "FROM AUX_CONSTANT " +
                "WHERE CONSTANT_ID='ACCEPT_ORDER_IN_SRV'";
            try
            {
                using (OracleConnection connection = GetDBConnection())
                {
                    acceptOrderConsts = connection.ExecuteScalar<string>(sqlstr, null);
                }
                if (acceptOrderConsts == "")
                    acceptOrderConsts = "N";
                List<L4L3Event> events = new List<L4L3Event>();
                sqlstr = "SELECT le.* FROM L4_L3_EVENT le WHERE MSG_STATUS = 1 AND msg_id IN (4301,4303, 4304, 4305) AND le.MSG_DATETIME > SYSDATE - 7 ORDER BY MSG_COUNTER";
                using (OracleConnection connection = GetConnection())
                {
                    events = connection.Query<L4L3Event>(sqlstr, null).AsList();
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
                l4MsgInfo.msgReport = new TMessageResult();
                    l4MsgInfo.msgReport.status = 1;
                    l4MsgInfo.msgReport.remark = "";
                    logger.Info($"STARTED Event -> Table: L4_L3_EVENT, MsgCounter:{l4MsgInfo.msgCounter}");
                    using (OracleConnection conn = GetConnection())
                    {
                        conn.Open();
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
                                        l4MsgInfo=check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, $"OP_CODE: {l4MsgInfo.opCode} is not valid");
                                        UpdateMsgStatus(l4MsgInfo, transaction);
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
                                                //somtask=Task.Run(() => som.SalesOrderMng(l4MsgInfo));
                                                //somtask.Start();
                                                //somtask.Wait();
                                                cheker = som.SalesOrderMng(l4MsgInfo);
                                            }
                                            break;
                                        //Запуск задачи CUSTOMER_CATALOG
                                        case L4L3InterfaceServiceConst.L4_L3_CUSTOMER_CATALOG:
                                            //ccmtask = Task.Run(() => ccm.CustomerMng(l4MsgInfo));
                                            //ccmtask.Start();
                                            //ccmtask.Wait();
                                            cheker = ccm.CustomerMng(l4MsgInfo);
                                            break;
                                        //Запуск задачи SHIPPING
                                        case L4L3InterfaceServiceConst.L4_L3_SHIPPING:
                                            //shiptask = Task.Run(() => sship.ShippingMng(l4MsgInfo));
                                            //shiptask.Start();
                                            //shiptask.Wait();
                                            cheker = sship.ShippingMng(l4MsgInfo);
                                            break;
                                        //Запуск задачи MATERIAL
                                        case L4L3InterfaceServiceConst.L4_L3_RAW_MATERIAL:
                                            //mattask = Task.Run(() => mat.L4L3MaterialMovement(l4MsgInfo));
                                            //mattask.Start();
                                            //mattask.Wait();
                                            cheker = mat.L4L3MaterialMovement(l4MsgInfo);
                                            break;
                                    }
                                    if (l4MsgInfo.msgReport.status == L4L3InterfaceServiceConst.MSG_STATUS_INSERT)
                                        UpdateMsgStatus(l4MsgInfo, transaction);
                                    if(cheker.isOK)
                                    {
                                        l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS;
                                        l4MsgInfo.msgReport.remark = cheker.data;
                                    }
                                    else
                                    {
                                        l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                                        l4MsgInfo.msgReport.remark = cheker.data;
                                    }
                                    UpdateMsgStatus(l4MsgInfo, transaction);
                                }
                                logger.Info($"STOPPED Event -> Table: L4_L3_EVENT, MsgCounter:{l4MsgInfo.msgCounter}");
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                if (IsBlocked(l4MsgInfo.msgCounter))
                                    BlockForProcess(l4MsgInfo, false);
                                using (OracleConnection conn1 = GetConnection())
                                {
                                    using (var transaction1 = conn.BeginTransaction())
                                    {
                                        try
                                        {
                                            l4MsgInfo=check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, e.Message.Substring(0, 4000));
                                            logger.Info($"STOPPED Event -> Table: L4_L3_EVENT, MsgCounter:{l4MsgInfo.msgCounter} with error");
                                            UpdateMsgStatus(l4MsgInfo, transaction1);
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
            catch {
                logger.Error("Ошибка выполнения обработчика событий");
            }
        }
        /// <summary>
        /// Обновление статуса сообщения
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        private void UpdateMsgStatus(TL4MsgInfo l4MsgInfo, OracleTransaction transaction)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "UPDATE L4_L3_EVENT SET   MSG_STATUS  = :P_MSG_STATUS, MSG_REMARK  = :P_MSG_REMARK WHERE MSG_COUNTER = :P_MSG_COUNTER ";
            odp.Add("P_MSG_STATUS", l4MsgInfo.msgReport.status);
            odp.Add("P_MSG_REMARK", l4MsgInfo.msgReport.remark);
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = GetConnection())
            {
                connection.Execute(str, odp,transaction);
            }
        }
        /// <summary>
        /// Проверка блокировки на выполнение
        /// </summary>
        /// <param name="msgCounter"></param>
        /// <returns>
        /// true - заблокирован
        /// false - разблокирован
        /// </returns>
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
                res = connection.ExecuteScalar<int>(sqlstr, odp);
            }
            if (res == 0)
                return false;
            else if (res == 1)
                return true;
            return false;
        }
        /// <summary>
        /// Блокировка на выполнение
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        /// <param name="serRSer"></param>
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
