using System.Collections.Generic;
using LPKService.Domain.Models.Work;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Infrastructure.Repository;
using LPKService.Repository;
using LPKService.Domain.Models.Shipping;
using LPKService.Domain.Models.Material;

namespace LPKService.Infrastructure.Shipping
{
    //Используется таблица L4_L3_SHIPPING
    public enum TPieceAction { paAssign, paDeAssign }
    public interface IL4L3SerShipping
    {
        TCheckResult ShippingMng(TL4MsgInfo l4MsgInfo);
        void CreateBolIfNotEx(string strBolId);
        TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo, L4L3Shipping ship, TPieceAction action, TForShipping forShipping);
        TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo);
    }
    public class L4L3ServiceShipping : IL4L3SerShipping
    {
        #region Constant
        const string l4l3unterfacetable = "L4_L3_SHIPPING";
        const int soLineAcceptedOpen = 3;
        const int soLineAcceptedClosed = 4;
        #endregion
        private Logger logger = LogManager.GetLogger(nameof(Shipping));
        public void CreateBolIfNotEx(string strBolId)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string bolId = "";
            string str = "SELECT BOL_ID FROM EXT_BOL_HEADER WHERE BOL_ID = :BOL_ID";
            odp.Add("BOL_ID", strBolId);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId = connection.QueryFirstOrDefault<string>(str, odp);
            }
            if (bolId == "")
            {
                odp = new OracleDynamicParameters();
                str = "INSERT INTO EXT_BOL_HEADER " +
                    "(MOD_USER_ID,MOD_DATETIME, BOL_ID,CREATION_DATETIME,STATUS) " +
                    "VALUES (:MOD_USER_ID ,SYSDATE, :BOL_ID, SYSDATE, :STATUS)";
                //odp.Add("MOD_USER_ID",); //Узнать про g_oUser.Userid
                odp.Add("BOL_ID", strBolId);
                odp.Add("STATUS", L4L3InterfaceServiceConst.BOL_NOT_SENT);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(str, odp);
                }
            }
        }

        public TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo)
        {
            L4L3InterfaceServiceGlobalCheck global = new L4L3InterfaceServiceGlobalCheck();
            TCheckResult checkResult = global.InitResultWithFalse();
            List<L4L3RmAndMatCat> l4L3Rms = new List<L4L3RmAndMatCat>();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "select  l4.material_id as sap_code, " +
                "M.MATERIAL_CODE," +
                "m.material_name," +
                "m.actual_qty l3_qty," +
                "l4.material_amount*1000 as material_amount," +
                "case" +
                "when (m.actual_qty < l4.material_amount*1000) then" +
                "(-1)*(m.actual_qty - l4.material_amount*1000)" +
                "else" +
                "(m.actual_qty - l4.material_amount*10000)" +
                "end as movement_qty" +
                "l4.movement_datetime" +
                "from    L4_L3_RAW_MATERIAL l4," +
                "mat_catalog m " +
                "where trim(L4.MATERIAL_ID) = trim(M.MATERIAL_CODE_L4) " +
                "and l4.msg_counter = :Counter";
            odp.Add("Counter", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                l4L3Rms = connection.Query<L4L3RmAndMatCat>(str, odp).AsList();
            }
            if (l4L3Rms.Count == 0)
            {
                l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                l4MsgInfo.msgReport.remark = "Код материала не найдена в БД МЕТ2000";
            }
            else
            {
                foreach(L4L3RmAndMatCat l4L3Rm in l4L3Rms)
                {
                    //Условие с InsertNewMovement
                }
            }
            return checkResult;
        }

        public TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo, L4L3Shipping ship, TPieceAction action, TForShipping forShipping)
        {
            TCheckResult checkResult = new TCheckResult();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            List<int> pieceNum = new List<int>();
            checkResult.isOK = false;
            string str = "SELECT PIECE_NUM_ID FROM PIECE WHERE PIECE_ID = :PIECE_ID AND STATUS= :STATUS AND PRODUCTION_MACHINE_CODE = 'HSM' ";
            if (forShipping == TForShipping.NOShipped)
            {
                str += "AND READY_TO_SHIP = :READY_TO_SHIP ";
                //odp.Add("PIECE_ID",); //Сделать модель таблицы piece
                //odp.Add("STATUS",);// Узнать код PIECE_STATUS_EXIST
                //odp.Add("READY_TO_SHIP",)//Узнать код PIECE_READY_TO_SHIP
            }
            else if (action == TPieceAction.paAssign && forShipping == TForShipping.YESShipped)
            {
                str += "AND READY_TO_SHIP= :READY_TO_SHIP";
                //odp.Add("PIECE_ID",); //Сделать модель таблицы piece
                //odp.Add("STATUS",);// Узнать код PIECE_STATUS_EXIST
                //odp.Add("READY_TO_SHIP",)//Узнать код PIECE_READY_TO_SHIP
            }
            else if (action == TPieceAction.paDeAssign && forShipping == TForShipping.YESShipped)
            {
                str += "AND PIECE_EXIT_TYPE= :PIECE_EXIT_TYPE";
                //odp.Add("PIECE_ID",); //Сделать модель таблицы piece
                //odp.Add("STATUS",);// Узнать код PIECE_STATUS_EXIST
                //odp.Add("PIECE_EXIT_TYPE",)//Узнать код PIECE_EXIT_TYPE
            }
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                pieceNum = connection.Query<int>(str, odp).AsList();
            }
            if (forShipping == TForShipping.NOShipped)
            {
                foreach (int piece in pieceNum)
                {
                    //SetPieceAssignExternalBOL(QryTemp.FieldByName('PIECE_NUM_ID').AsInteger, Action, Qry.FieldByName('BOL_ID').AsString);
                }
            }
            else if (forShipping == TForShipping.YESShipped)
            {
                foreach (int piece in pieceNum)
                {
                    //SetPieceShippedFromExternal(QryTemp.FieldByName('PIECE_NUM_ID').AsInteger, Action, Qry.FieldByName('BOL_ID').AsString);
                }
            }
            checkResult.isOK = true;
            checkResult.data = $"В таблице {l4l3unterfacetable} поле MSG_COUNTER:{l4MsgInfo.msgCounter} Ошибок нет.";
            return checkResult;
        }
        //Сделать модель Piece
        public TCheckResult ShippingMng(TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = new TCheckResult();
            TForShipping forShipping=TForShipping.NOShipped;
            L4L3InterfaceServiceGlobalCheck global = new L4L3InterfaceServiceGlobalCheck();
            logger.Error($"ShippingMng - STARTED -> MsgId: {l4MsgInfo.msgCounter}");
            L4L3ShippingRepo shippingRepo = new L4L3ShippingRepo();
            List<L4L3Shipping> ship = shippingRepo.GetListData(l4MsgInfo);
            if(ship == null)
            {
                result.isOK = false;
                result.data = $"В таблице {l4l3unterfacetable} поле {l4MsgInfo.msgCounter} запись не найдена";
                global.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                return result;
            }
            L4L3ServiceCheckShipping checkship = new L4L3ServiceCheckShipping();
            result = checkship.ShippingCheck(ship, l4MsgInfo);
            if(result.isOK)
            {
                if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && ship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                {
                    CreateBolIfNotEx(ship.bolId);
                    forShipping = TForShipping.NOShipped;
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && ship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                    forShipping = TForShipping.NOShipped;
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && ship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                    forShipping = TForShipping.YESShipped;
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && ship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                    forShipping = TForShipping.YESShipped;
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL && ship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                    forShipping = TForShipping.YESShipped;
                result = LocalSetPiece(l4MsgInfo,ship ,TPieceAction.paAssign, forShipping);
            }
            if (!result.isOK)
            {
                global.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                logger.Error(result.data);
            }
            else
                global.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS, result.data);
            logger.Error($"ShippingMng - STOPPED -> MsgId:{l4MsgInfo.msgCounter}");
            return result;
        }
    }
}
