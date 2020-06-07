using System;
using LPKService.Domain.Models.Work;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Dapper.Oracle;
using Dapper;
using LPKService.Infrastructure.Repository;
using Repository;
using LPKService.Domain.Models.Shipping;
using System.Collections.Generic;

namespace LPKService.Infrastructure.Shipping
{
    public enum TForShipping { YESShipped,NOShipped }
    interface L4L3ServCheckShip
    {
        bool CheckPiece(string pieceId, string soId, string soLineId);
        bool L4L3ShipPieceSOCheck(L4L3Shipping ship);
        bool CheckBolExistNotShip(string strBolId);
        bool CheckBolExistIsShip(string strBolId);
        bool ShippingIsPieceAssignedToBOL(L4L3Shipping ship, TForShipping forShipping);
        bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping);
        TCheckResult ShippingCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo);
        TCheckResult ShippingGeneralCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo);
    }
    class L4L3ServiceCheckShipping : L4L3ServCheckShip
    {
        private IGlobalCheck check;
        private List<string> strPieceIdList = new List<string>();
        private Logger logger = LogManager.GetLogger(nameof(Shipping));
        /// <summary>
        /// Проверка накладной на предмет загрузки
        /// </summary>
        /// <param name="strBolId">ИД накладной</param>
        /// <returns>
        /// true - существует
        /// false - не существует
        /// </returns>
        public bool CheckBolExistIsShip(string strBolId)
        {
            string bolId = "";
            OracleDynamicParameters odp = null;
            string str = $"SELECT BOL_ID FROM EXT_BOL_HEADER WHERE STATUS='{L4L3InterfaceServiceConst.BOL_SENT.ToString()}' AND BOL_ID='{strBolId}'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId = connection.QueryFirstOrDefault<string>(str, odp);
            }
            if (bolId != "")
                return true;
            return false;
        }
        /// <summary>
        /// Проверка накладной на предмет разгрузки
        /// </summary>
        /// <param name="strBolId">ИД накладной</param>
        /// <returns>
        /// true - существует
        /// false - не существует
        /// </returns>
        public bool CheckBolExistNotShip(string strBolId)
        {
            string bolId = "";
            OracleDynamicParameters odp = null;
            string str = $"SELECT BOL_ID FROM EXT_BOL_HEADER WHERE STATUS='{L4L3InterfaceServiceConst.BOL_NOT_SENT.ToString()}' AND BOL_ID='{strBolId}'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId = connection.ExecuteScalar<string>(str, odp);
            }
            if (bolId != "")
                return true;
            return false;
        }
        /// <summary>
        /// Проверка принадлежности предмета к накладной
        /// </summary>
        /// <param name="strBolId">ИД накладной</param>
        /// <param name="forShipping">Метка отгрузки\разгрузки</param>
        /// <returns>
        /// true - принадлежит
        /// false - не принадлежит 
        /// </returns>
        public bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Проверка предмета
        /// </summary>
        /// <param name="pieceId">ИД предмета</param>
        /// <param name="soId">ИД заказа</param>
        /// <param name="soLineId">ИД линии заказа</param>
        /// <returns>
        /// true - существует
        /// false - не существует 
        /// </returns>
        public bool CheckPiece(string pieceId, string soId, string soLineId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Проверка существования предмета в заказе
        /// </summary>
        /// <param name="ship">Модель таблицы L4_L3_SHIPPING для обработки</param>
        /// <returns>
        /// true - существует
        /// false - не существует  
        /// </returns>
        public bool L4L3ShipPieceSOCheck(L4L3Shipping ship)
        {
            return CheckPiece(ship.pieceId, ship.soId, ship.soLineId);
        }
        /// <summary>
        /// Проверка отгрузки
        /// </summary>
        /// <param name="ship">Набор строк таблицы L4_L3_SHIPPING для обработки</param>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event</param>
        /// <returns>Результат работы</returns>
        public TCheckResult ShippingCheck(List<L4L3Shipping> ship,TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = ShippingGeneralCheck(ship,l4MsgInfo);
            if (!result.isOK)
            {
                return result;
            }
            foreach (L4L3Shipping sinship in ship)
            {
                if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && sinship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                {
                    if (!L4L3ShipPieceSOCheck(sinship))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} одна из заготовок не может быть назначена в накладную BOL: {sinship.bolId}";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && sinship.bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
                {
                    if (!CheckBolExistNotShip(sinship.bolId))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {sinship.bolId} не существует или отгружена";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                    if (!ShippingIsPieceAssignedToBOL(sinship, TForShipping.NOShipped))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {sinship.bolId}";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && sinship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                {
                    if (!CheckBolExistIsShip(sinship.bolId))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {sinship.bolId} не существует или отгружена";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                    if (!ShippingIsPieceAssignedToBOL(sinship, TForShipping.YESShipped))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {sinship.bolId}";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && sinship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                {
                    if (!CheckBolExistIsShip(sinship.bolId))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {sinship.bolId} не существует или отгружена";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                    if (!L4L3ShipPieceSOCheck(sinship))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {sinship.bolId}";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL && sinship.bolStatus == L4L3InterfaceServiceConst.BOL_SENT)
                {
                    if (!CheckBolExistNotShip(sinship.bolId))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {sinship.bolId} не существует или отгружена";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                    if (L4L3ShipPieceSOCheck(sinship))
                    {
                        result.isOK = false;
                        result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {sinship.bolId}";
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Основная проверка отгрузки
        /// </summary>
        /// <param name="ship">Модель таблицы L4_L3_SHIPPING для обработки</param>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event</param>
        /// <returns>Результат работы</returns>
        public TCheckResult ShippingGeneralCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = check.InitResultWithFalse();
            result.isOK = true;
            bool firstloop = true;
            string bol_idUsedToCheck="", strPieceId = "", strError = "";
            int bol_status=-1, bol_statusUserToCheck = -1;
            if(ship==null)
            {
                check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, $"Запись не найдена Msg_counter={l4MsgInfo.msgCounter}");
                return result;
            }
            foreach(L4L3Shipping shp in ship)
            {
                if(firstloop)
                {
                    bol_statusUserToCheck = shp.bolStatus;
                    bol_idUsedToCheck = shp.bolId;
                }
                else
                {
                    if(bol_idUsedToCheck!=shp.bolId)
                    {
                        strError = $"Таблица L4_L3_SHIPPING MSG_COUNTER: {l4MsgInfo.msgCounter} значение Накладной должно совпадать для каждой записи";
                        result.isOK = false;
                        result.data = strError;
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, strError);
                        break;
                    }
                }
                bol_status = shp.bolStatus;
                if(bol_statusUserToCheck!=bol_status)
                {
                    strError = $"Таблица L4_L3_SHIPPING MSG_COUNTER: {l4MsgInfo.msgCounter} значение поля BOL_STATUS должно быть одинаковым для каждой записи";
                    result.isOK = false;
                    result.data = strError;
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, strError);
                    break;
                }
                if(l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW)
                {
                    if (!(bol_status == L4L3InterfaceServiceConst.BOL_NOT_SENT || bol_status == L4L3InterfaceServiceConst.BOL_SENT))
                    {
                        strPieceId = shp.pieceId;
                        strError = $"Таблица L4_L3_SHIPPING MSG_COUNTER: {l4MsgInfo.msgCounter} значение PIECE_ID: {shp.pieceId} для BOL_STATUS = {bol_status} неверно";
                        result.isOK = false;
                        result.data = strError;
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, strError);
                        break;
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_DEL)
                {
                    if (bol_status != L4L3InterfaceServiceConst.BOL_SENT)
                    {
                        strPieceId = shp.pieceId;
                        strError = $"Таблица L4_L3_SHIPPING MSG_COUNTER: {l4MsgInfo.msgCounter} значение PIECE_ID: {shp.pieceId} для BOL_STATUS = {bol_status} неверно";
                        result.isOK = false;
                        result.data = strError;
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, strError);
                        break;
                    }
                }
                else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD)
                {
                    if (!(bol_status == L4L3InterfaceServiceConst.BOL_NOT_SENT || bol_status == L4L3InterfaceServiceConst.BOL_SENT))
                    {
                        strPieceId = shp.pieceId;
                        strError = $"Таблица L4_L3_SHIPPING MSG_COUNTER: {l4MsgInfo.msgCounter} значение PIECE_ID: {shp.pieceId} для BOL_STATUS = {bol_status} неверно";
                        result.isOK = false;
                        result.data = strError;
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, strError);
                        break;
                    }
                }
                firstloop = false;
            }
            return result;
        }
        /// <summary>
        /// Проверка принадлежности предмета к накладной
        /// </summary>
        /// <param name="ship">Модель таблицы L4_L3_SHIPPING для обработки</param>
        /// <param name="forShipping">Метка отгрузки\разгрузки</param>
        /// <returns>
        /// true - принадлежит
        /// false - не принадлежит
        /// </returns>
        public bool ShippingIsPieceAssignedToBOL(L4L3Shipping ship, TForShipping forShipping)
        {
            strPieceIdList.Add(ship.pieceId);
            return CheckIfPieceRelatedToBOL(ship.bolId, forShipping);
        }
    }
}
