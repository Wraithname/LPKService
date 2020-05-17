using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Dapper.Oracle;
using Dapper;
using Repository.Models;
using Repository;
using System.Threading;
namespace Work
{

    public class ServiceWork : IServiceWork
    {
        #region Constant
        const int bol_new = 1;
        const int bol_error = -1;
        const int bol_created = 2;
        const int bol_new_sap_met = 1001;
        const int op_new_bol = 1;
        const int op_update_bol = 2;
        const int op_delete_bol = 3;
        #endregion
        List<Action<TL4MsgInfo>> actions = new List<Action<TL4MsgInfo>>();

        public ServiceWork()
        {
            ProcedureAction procedure = new ProcedureAction();
            actions = procedure.GetActions();
        }
        //Сделать модели
        public void CreateBol(int msgCounter, string bolId, bool allBol)
        {
            throw new NotImplementedException();
        }

        public void DeleteBol(int msgCounter, string bolId, int posNumId)
        {
            if (bolId != "" && posNumId > 0)
            {
                OracleDynamicParameters odp = new OracleDynamicParameters();
                string str = "DELETE FROM EXT_BOL_POSITION WHERE POS_NUM_ID = :POS_NUM_ID ";
                odp.Add("POS_NUM_ID", posNumId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(str, odp);
                }
                OracleDynamicParameters odp1 = new OracleDynamicParameters();
                str = "SELECT count(1) as cnt FROM EXT_BOL_POSITION WHERE BOL_ID = :BOL_ID";
                odp1.Add("BOL_ID", bolId);
                int res;
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    res = connection.QueryFirstOrDefault<int>(str, odp);
                }
                if (res == 0)
                {
                    OracleDynamicParameters odp2 = new OracleDynamicParameters();
                    str = "DELETE FROM EXT_BOL_HEADER WHERE BOL_ID = :BOL_ID ";
                    using (OracleConnection connection = BaseRepo.GetDBConnection())
                    {
                        connection.Execute(str, odp);
                    }
                }
                UpdateStatusMessage(msgCounter, bol_created, "");
            }
            else
                UpdateStatusMessage(msgCounter, bol_error, "bolId или posNumId равны null");
        }

        public bool ExistBolPosition(string bolId, string posId)
        {
            string result = "";
            if (bolId != "" && posId != "")
            {
                OracleDynamicParameters odp = new OracleDynamicParameters();
                string str = "SELECT BOL_ID FROM EXT_BOL_POSITION WHERE upper(BOL_ID) = upper(:BOL_ID) AND upper(POS_ID) = upper(:POS_ID)";
                odp.Add("BOL_ID", bolId);
                odp.Add("POS_ID", posId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    result = connection.QueryFirstOrDefault<string>(str, odp);
                }
                if (result == "")
                    return false;
            }
            return true;
        }

        public bool ExistsBol(string bolId, bool selChild)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            if (bolId != null)
            {
                string str, result = "";
                if (selChild)
                    str = "SELECT ebh.BOL_ID FROM EXT_BOL_HEADER ebh left join V_PIECE vp on ebh.BOL_ID = vp.BOL_ID WHERE upper(ebh.BOL_ID) = upper(:BOL_ID) AND vp.PIECE_NUM_ID IS NULL AND ebh.STATUS = 1";
                else
                    str = "SELECT BOL_ID FROM EXT_BOL_HEADER WHERE upper(BOL_ID) = upper(:BOL_ID) ";
                odp.Add("BOL_ID", bolId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    result = connection.QueryFirstOrDefault<string>(str, odp);
                }
                if (result == "") return false;
            }
            return true;
        }
        //Сделать модели
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
                res = connection.QueryFirstOrDefault<string>(str, odp);
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
            string bolId = "";
            int bolPosition;
            L4L3DelEventDel l3DelEventDel = new L4L3DelEventDel();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT LDE.MSG_COUNTER, LD.BOL_ID, LDE.OP_CODE, ld.BOL_POSITION_ID " +
                "FROM L4_L3_DELIVERY_EVENT lde join L4_L3_DELIVERY ld on LDE.MSG_COUNTER = LD.MSG_COUNTER " +
                "WHERE LDE.MSG_STATUS = :MSG_STATUS " +
                "AND LDE.MSG_ID = :MSG_ID ";
            odp.Add("MSG_STATUS", bol_new_sap_met);
            odp.Add("MSG_ID", bol_new);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                l3DelEventDel = connection.QueryFirstOrDefault<L4L3DelEventDel>(str, odp);
            }
            if (l3DelEventDel != null)
            {
                using (OracleConnection conn = BaseRepo.GetDBConnection())
                {
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            bolId = l3DelEventDel.bolId;
                            bolPosition = l3DelEventDel.bolPositionId;
                            if (bolId == "")
                                UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, "Номер накладной пуст");
                            else
                            {
                                if (l3DelEventDel.opCode == op_new_bol)
                                {
                                    if (!ExistsBol(bolId, false))
                                        CreateBol(l3DelEventDel.msgCounter, bolId, true);
                                    else
                                    {
                                        if (!ExistBolPosition(bolId, l3DelEventDel.bolPositionId.ToString()))
                                        {
                                            if (!IsUpdate(l3DelEventDel.msgCounter, bolId, bolPosition))
                                                CreateBol(l3DelEventDel.msgCounter, bolId, true);
                                            else
                                                UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, "Позиция заморожена для автоматического изменения.");
                                        }
                                        else
                                            UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, "Позиция в накладной уже существует в БД MET2000.");
                                    }
                                }
                                if(l3DelEventDel.opCode==op_delete_bol)
                                    if(ExistsBol(bolId,true))
                                    {
                                        if (IsUpdate(l3DelEventDel.msgCounter, bolId,bolPosition))
                                            DeleteBol(l3DelEventDel.msgCounter, bolId, bolPosition);
                                        else
                                            UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, "Накладной нет в МЕТ 2000 или она уже наполнена.");
                                    }
                                if (l3DelEventDel.opCode == op_update_bol)
                                    if (IsUpdate(l3DelEventDel.msgCounter, bolId, bolPosition))
                                        UpdateBolPosition(l3DelEventDel.msgCounter, bolPosition);
                                    else
                                        UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, "Позиция заморожена для автоматического изменения.");
                            }
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            UpdateStatusMessage(l3DelEventDel.msgCounter, bol_error, e.Message);
                        }
                    }
                }
            }
        }

        public bool IsUpdate(int msgCounter, string bolId, int bolPosNumId)
        {
            int status = -2;
            bolPosNumId = -1;
            try
            {
                JoinedModel joint = new JoinedModel();
                OracleDynamicParameters odp = new OracleDynamicParameters();
                string str = "SELECT STATUS FROM EXT_BOL_HEADER WHERE BOL_ID = :BOL_ID ";
                odp.Add("BOL_ID", bolId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    status = connection.QueryFirstOrDefault<int>(str, odp);
                }
                if (status != -2)
                {
                    if (status == 1)
                    {
                        OracleDynamicParameters odp1 = new OracleDynamicParameters();
                        str = "SELECT count(pp.POS_NUM_ID) as cnt, bp.POS_NUM_ID " +
                            "FROM EXT_BOL_POSITION bp " +
                            "join L4_L3_DELIVERY ld on bp.BOL_ID = ld.BOL_ID AND bp.POS_ID = ld.BOL_POSITION_ID " +
                            "left join EXT_BOL_POSITION_PIECE pp on pp.POS_NUM_ID = bp.POS_NUM_ID " +
                            "WHERE ld.MSG_COUNTER = :MSG_COUNTER " +
                            "AND pp.POS_NUM_ID is null " +
                            "group by bp.POS_NUM_ID";
                        odp1.Add("MSG_COUNTER", msgCounter);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            joint = connection.QueryFirstOrDefault<JoinedModel>(str, odp);
                        }
                        if (joint != null)
                        {
                            bolPosNumId = joint.posNumId;
                            if (joint.count <= 0)
                                return true;
                        }
                    }

                }
                return false;
            }
            catch { return false; }
        }

        public void MngLoop()
        {
            GetNewMessageDelivery();
            GetNewMessage();
            GetAutoCloseOrder();
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
