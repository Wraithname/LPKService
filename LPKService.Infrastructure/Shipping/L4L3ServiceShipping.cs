using System.Collections.Generic;
using LPKService.Domain.Models.Work;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Infrastructure.Repository;
using Repository;
using LPKService.Domain.Models.Shipping;

namespace LPKService.Infrastructure.Shipping
{
    public enum TPieceAction { paAssign, paDeAssign }
    public interface IL4L3SerShipping
    {
        TCheckResult ShippingMng(TL4MsgInfo l4MsgInfo);
        void CreateBolIfNotEx(string strBolId);
        TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo, L4L3Shipping ship, TPieceAction action, TForShipping forShipping);
    }
    public class L4L3ServiceShipping : IL4L3SerShipping
    {
        #region Constant
        const string l4l3unterfacetable = "L4_L3_SHIPPING";
        const int soLineAcceptedOpen = 3;
        const int soLineAcceptedClosed = 4;
        #endregion
        private Logger logger = LogManager.GetLogger(nameof(Shipping));
        /// <summary>
        /// Создание позиции если не существует
        /// </summary>
        /// <param name="strBolId">ИД позиции</param>
        public void CreateBolIfNotEx(string strBolId)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string bolId = "";
            string str = "SELECT BOL_ID FROM EXT_BOL_HEADER WHERE BOL_ID = :BOL_ID";
            odp.Add("BOL_ID", strBolId);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId = connection.ExecuteScalar<string>(str, odp);
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
        /// <summary>
        /// Получение детали (Требуется доработка)
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        /// <param name="ship">Модель таблицы L4L3Shipping</param>
        /// <param name="action">Тип перечисления действий</param>
        /// <param name="forShipping">Тип перечисления отгрузки</param>
        /// <returns>Результат обработки</returns>
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
                odp.Add("PIECE_ID",ship.pieceId);
                //odp.Add("STATUS",);// Узнать код PIECE_STATUS_EXIST
                //odp.Add("READY_TO_SHIP",)//Узнать код PIECE_READY_TO_SHIP
            }
            else if (action == TPieceAction.paAssign && forShipping == TForShipping.YESShipped)
            {
                str += "AND READY_TO_SHIP= :READY_TO_SHIP";
                odp.Add("PIECE_ID", ship.pieceId);
                //odp.Add("STATUS",);// Узнать код PIECE_STATUS_EXIST
                //odp.Add("READY_TO_SHIP",)//Узнать код PIECE_READY_TO_SHIP
            }
            else if (action == TPieceAction.paDeAssign && forShipping == TForShipping.YESShipped)
            {
                str += "AND PIECE_EXIT_TYPE= :PIECE_EXIT_TYPE";
                odp.Add("PIECE_ID", ship.pieceId);
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
        /// <summary>
        /// Обработчик события кода на погрузку
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        /// <returns>Результат обработки</returns>
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
            foreach (L4L3Shipping sship in ship)
            {
                if (result.isOK)
                {
                    if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && sship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                    {
                        CreateBolIfNotEx(sship.bolId);
                        forShipping = TForShipping.NOShipped;
                    }
                    else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && sship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                        forShipping = TForShipping.NOShipped;
                    else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && sship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                        forShipping = TForShipping.YESShipped;
                    else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && sship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                        forShipping = TForShipping.YESShipped;
                    else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL && sship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                        forShipping = TForShipping.YESShipped;
                    result = LocalSetPiece(l4MsgInfo, sship, TPieceAction.paAssign, forShipping);
                }
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
