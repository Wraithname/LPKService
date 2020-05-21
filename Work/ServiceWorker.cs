using System;
using System.Collections.Generic;
using Repository.WorkModels;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;
using Work.Models;
namespace Work
{
    class ServiceWorker : IServiceWork
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
        public ServiceWorker()
        {
            ProcedureAction procedure = new ProcedureAction();
            actions = procedure.GetActions();
        }

        public void CreateBol(int msgCounter, string bolId, bool allBol)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            DeliveryESOHandSOL deliveryev = new DeliveryESOHandSOL();
            string autoFlag = "N", vehicleSap = "-";
            try
            {
                if (bolId != "")
                {
                    string str = "SELECT LDE.MSG_COUNTER, " +
                        "lde.op_code, " +
                        "LD.BOL_ID, " +
                        "LD.BOL_POSITION_ID, " +
                        "LD.SO_ID, " +
                        "LD.SO_LINE_ID, " +
                        "LD.ENTRY_QNT, " +
                        "nvl(soh.so_id,-1) as SO_ID_MET, " +
                        "nvl(SOL.SO_LINE_ID,-1) as SO_LINE_ID_MET " +
                        "FROM L4_L3_DELIVERY_EVENT lde join L4_L3_DELIVERY ld on LDE.MSG_COUNTER = LD.MSG_COUNTER " +
                        "left join SALES_ORDER_HEADER soh on SOH.SO_DESCR_ID = ld.SO_ID||''_''||(nvl(to_number(ld.SO_LINE_ID),0)/10) " +
                        "left join SALES_ORDER_LINE sol on SOL.SO_ID  = SOH.SO_ID and SOL.SO_LINE_ID = nvl(to_number(lD.SO_LINE_ID),0)/10 " +
                        "WHERE LDE.MSG_COUNTER = :MSG_COUNTER";
                    odp.Add("MSG_COUNTER", msgCounter);
                    using (OracleConnection connection = BaseRepo.GetDBConnection())
                    {
                        deliveryev = connection.QueryFirstOrDefault<DeliveryESOHandSOL>(str, odp);
                    }
                    if (deliveryev != null)
                    {
                        str = "INSERT INTO EXT_BOL_POSITION (WEIGHT, POS_NUM_ID, BOL_ID, POS_ID, SO_ID, SO_LINE_ID, SO_ID_MET, SO_LINE_ID_MET) " +
                            "VALUES (:WEIGHT, SQN_EXT_BOL_POSITION.NEXTVAL, :BOL_ID, :POS_ID, :SO_ID, :SO_LINE_ID, :SO_ID_MET, :SO_LINE_ID_MET)";
                        odp.Add("WEIGHT", deliveryev.entryQnt);
                        odp.Add("BOL_ID", bolId);
                        odp.Add("POS_ID", deliveryev.bolPositionId);
                        odp.Add("SO_ID", deliveryev.soId);
                        odp.Add("SO_LINE_ID", deliveryev.soLineId);
                        odp.Add("SO_ID_MET", deliveryev.soIdMet);
                        odp.Add("SO_LINE_ID_MET", deliveryev.soLineIdMet);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            connection.Execute(str, odp);
                        }
                        UpdateStatusMessage(deliveryev.msgCounter, bol_created, "");
                    }
                    else
                    {
                        VecAuto auto = new VecAuto();
                        str = "SELECT nvl(VEHICLE_ID,''-'') as VEHICLE_ID, case when nvl(AUTO_FLG,''03'') = ''03'' then ''N'' else ''Y'' end as AUTO_FLG FROM L4_L3_DELIVERY WHERE MSG_COUNTER	= :MSG_COUNTER ";
                        odp.Add("MSG_COUNTER", msgCounter);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            auto = connection.QueryFirstOrDefault<VecAuto>(str, odp);
                        }
                        if (auto != null)
                        {
                            autoFlag = auto.autoFlg;
                            vehicleSap = auto.vehicleId;
                        }
                        odp = null;
                        str = "INSERT INTO EXT_BOL_HEADER (MOD_USER_ID,MOD_DATETIME,BOL_ID,CREATION_DATETIME,SHIP_DATETIME,VEHICLE_ID_SAP,ON_AUTO_SHIPPING,STATUS) " +
                            "VALUES (-999,SYSDATE,:BOL_ID,SYSDATE,SYSDATE,:VEHICLE_ID_SAP,:ON_AUTO_SHIPPING,1";
                        odp.Add("BOL_ID", bolId);
                        odp.Add("VEHICLE_ID_SAP", vehicleSap);
                        odp.Add("ON_AUTO_SHIPPING", autoFlag);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            connection.Execute(str, odp);
                        }
                        odp = null;
                        List<DeliveryESOHandSOL> deliv = new List<DeliveryESOHandSOL>();
                        str = "SELECT LDE.MSG_COUNTER, " +
                        "lde.op_code, " +
                        "LD.BOL_ID, " +
                        "LD.BOL_POSITION_ID, " +
                        "LD.SO_ID, " +
                        "LD.SO_LINE_ID, " +
                        "LD.ENTRY_QNT, " +
                        "nvl(soh.so_id,-1) as SO_ID_MET, " +
                        "nvl(SOL.SO_LINE_ID,-1) as SO_LINE_ID_MET " +
                        "FROM L4_L3_DELIVERY_EVENT lde join L4_L3_DELIVERY ld on LDE.MSG_COUNTER = LD.MSG_COUNTER " +
                        "left join SALES_ORDER_HEADER soh on SOH.SO_DESCR_ID = ld.SO_ID||''_''||(nvl(to_number(ld.SO_LINE_ID),0)/10) " +
                        "left join SALES_ORDER_LINE sol on SOL.SO_ID  = SOH.SO_ID and SOL.SO_LINE_ID = nvl(to_number(lD.SO_LINE_ID),0)/10 " +
                        "WHERE LDE.MSG_STATUS = :MSG_STATUS " +
                        "AND LDE.MSG_ID = :MSG_ID " +
                        "AND   LD.BOL_ID = :BOL_ID " +
                        "AND LDE.MSG_COUNTER = :MSG_COUNTER ";
                        odp.Add("MSG_STATUS", bol_new);
                        odp.Add("MSG_ID", bol_new_sap_met);
                        odp.Add("BOL_ID", bolId);
                        odp.Add("MSG_COUNTER", msgCounter);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            deliv = connection.Query<DeliveryESOHandSOL>(str, odp).AsList();
                        }
                        if (deliveryev != null)
                        {
                            foreach (DeliveryESOHandSOL del in deliv)
                            {
                                if (del.opCode == op_new_bol)
                                {
                                    if (!ExistBolPosition(bolId, del.bolPositionId.ToString()))
                                    {
                                        odp = null;
                                        str = "INSERT INTO EXT_BOL_POSITION (WEIGHT, POS_NUM_ID, BOL_ID, POS_ID, SO_ID, SO_LINE_ID, SO_ID_MET, SO_LINE_ID_MET) " +
                                            "VALUES (:WEIGHT, SQN_EXT_BOL_POSITION.NEXTVAL, :BOL_ID, :POS_ID, :SO_ID, :SO_LINE_ID, :SO_ID_MET, :SO_LINE_ID_MET) ";
                                        odp.Add("WEIGHT");
                                        odp.Add("BOL_ID");
                                        odp.Add("POS_ID");
                                        odp.Add("SO_ID");
                                        odp.Add("SO_LINE_ID");
                                        odp.Add("SO_ID_MET");
                                        odp.Add("SO_LINE_ID_MET");
                                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                                        {
                                            connection.Execute(str, odp);
                                        }
                                        UpdateStatusMessage(del.msgCounter, bol_created, "");
                                    }
                                    else
                                        UpdateStatusMessage(del.msgCounter, bol_error, "Позиция заказа уже существует");
                                }
                            }
                        }
                    }
                    UpdateStatusMessage(msgCounter, bol_created, "");
                }
                else
                    UpdateStatusMessage(msgCounter, bol_error, "BOL_ID is null");
            }
            catch (Exception e)
            {
                UpdateStatusMessage(msgCounter, bol_error, e.Message);
            }
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
            string str1 = "UPDATE  SALES_ORDER_LINE " +
                "SET     SO_LINE_STATUS = 4 " +
                " , MOD_DATETIME = sysdate" +
                ", MANUAL_CLOSE_ORDER = 'Y' " +
                "WHERE SO_ID = :SO_ID " +
                "AND   SO_LINE_ID = :SO_LINE_ID";
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
        //Основная логика работы (подумать насчёт потоков)
        public void GetNewMessage()
        {
            TL4MsgInfo l4MsgInfo = new TL4MsgInfo();
            bool isOpCodeOk;
            string acceptOrderConsts;
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
                                if (l3DelEventDel.opCode == op_delete_bol)
                                    if (ExistsBol(bolId, true))
                                    {
                                        if (IsUpdate(l3DelEventDel.msgCounter, bolId, bolPosition))
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
            OracleDynamicParameters odp = new OracleDynamicParameters();
            DeliverySOHandSOL del = new DeliverySOHandSOL();
            string str1 = "";
            string str = "SELECT LD.BOL_POSITION_ID  , " +
                " nvl(LD.VEHICLE_ID,''-'') as VEHICLE_ID, " +
                "LD.BOL_ID, " +
                "LD.SO_ID, " +
                "LD.SO_LINE_ID, " +
                "LD.ENTRY_QNT, " +
                "nvl(soh.so_id,-1) as SO_ID_MET, " +
                "nvl(SOL.SO_LINE_ID,-1) as SO_LINE_ID_MET " +
                "FROM L4_L3_DELIVERY ld " +
                "left join SALES_ORDER_HEADER soh on SOH.SO_DESCR_ID = ld.SO_ID||''_''||(nvl(to_number(ld.SO_LINE_ID),0)/10) " +
                "left join SALES_ORDER_LINE sol on SOL.SO_ID  = SOH.SO_ID and SOL.SO_LINE_ID = nvl(to_number(lD.SO_LINE_ID),0)/10 " +
                "WHERE ld.MSG_COUNTER = :MSG_COUNTER ";
            odp.Add("MSG_COUNTER", msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                del = connection.QueryFirstOrDefault<DeliverySOHandSOL>(str, odp);
            }
            if (del != null)
            {
                if (del.vehicleId != "-")
                {
                    odp = new OracleDynamicParameters();
                    str1 = "UPDATE EXT_BOL_HEADER SET VEHICLE_ID_SAP = :VEHICLE_ID_SAP WHERE BOL_ID = :BOL_ID ";
                    odp.Add("VEHICLE_ID_SAP", del.vehicleId);
                    odp.Add("BOL_ID", del.bolId);
                    using (OracleConnection connection = BaseRepo.GetDBConnection())
                    {
                        connection.Execute(str, odp);
                    }
                }
                odp = new OracleDynamicParameters();
                str1 = "UPDATE EXT_BOL_POSITION SET WEIGHT = :WEIGHT , SO_ID  = :SO_ID, SO_LINE_ID = :SO_LINE_ID , SO_ID_MET  = :SO_ID_MET, SO_LINE_ID_MET = :SO_LINE_ID_MET WHERE POS_NUM_ID = :POS_NUM_ID";
                odp.Add("WEIGHT", del.entryQnt);
                odp.Add("SO_ID", del.soId);
                odp.Add("SO_LINE_ID", del.soLineId);
                odp.Add("SO_ID_MET", del.soIdMet);
                odp.Add("SO_LINE_ID_MET", del.soLineIdMet);
                odp.Add("POS_NUM_ID", posNumId);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(str, odp);
                }
                UpdateStatusMessage(msgCounter, bol_created, "");
            }
            else
                UpdateStatusMessage(msgCounter, bol_error, "BOL_POSITION_ID is null");
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
