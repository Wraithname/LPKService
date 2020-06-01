﻿using System;
using LPKService.Domain.Models.Work;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Dapper.Oracle;
using Dapper;
using LPKService.Infrastructure.Repository;
using LPKService.Repository;
using LPKService.Domain.Models.Shipping;
using System.Collections.Generic;

namespace LPKService.Infrastructure.Shipping
{
    //Используется таблица L4_L3_Shipping
    //Файл L4L3ServiceCheckShipping
    public enum TForShipping { YESShipped,NOShipped }
    interface L4L3ServCheckShip
    {
        bool CheckPiece(string pieceId, string soId, string soLineId);
        bool L4L3ShipPieceSOCheck(List<L4L3Shipping> ship);
        bool CheckBolExistNotShip(string strBolId);
        bool CheckBolExistIsShip(string strBolId);
        bool ShippingIsPieceAssignedToBOL(List<L4L3Shipping> ship, TForShipping forShipping);
        bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping);
        TCheckResult ShippingCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo);
        TCheckResult ShippingGeneralCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo);
    }
    class L4L3ServiceCheckShipping : L4L3ServCheckShip
    {
        private IGlobalCheck check;
        private Logger logger = LogManager.GetLogger(nameof(Shipping));
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

        public bool CheckBolExistNotShip(string strBolId)
        {
            string bolId = "";
            OracleDynamicParameters odp = null;
            string str = $"SELECT BOL_ID FROM EXT_BOL_HEADER WHERE STATUS='{L4L3InterfaceServiceConst.BOL_NOT_SENT.ToString()}' AND BOL_ID='{strBolId}'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId = connection.QueryFirstOrDefault<string>(str, odp);
            }
            if (bolId != "")
                return true;
            return false;
        }

        public bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping)
        {
            throw new NotImplementedException();
        }

        public bool CheckPiece(string pieceId, string soId, string soLineId)
        {
            throw new NotImplementedException();
        }

        public bool L4L3ShipPieceSOCheck(List<L4L3Shipping> ship)
        {
            return CheckPiece(ship[0].pieceId, ship[0].soId, ship[0].soLineId);
        }

        public TCheckResult ShippingCheck(List<L4L3Shipping> ship,TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = ShippingGeneralCheck(ship,l4MsgInfo);
            if (!result.isOK)
            {
                return result;
            }
            if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_NEW && ship[0].bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
            {
                if (!L4L3ShipPieceSOCheck(ship))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} одна из заготовок не может быть назначена в накладную BOL: {ship.bolId}";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
            }
            else if (l4MsgInfo.opCode == L4L3InterfaceServiceConst.OP_CODE_UPD && ship[0].bolStatus == L4L3InterfaceServiceConst.BOL_NOT_SENT)
            {
                if (!CheckBolExistNotShip(ship[0].bolId))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {ship[0].bolId} не существует или отгружена";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
                if (!ShippingIsPieceAssignedToBOL(ship, TForShipping.NOShipped))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {ship[0].bolId}";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
            }
            else if(l4MsgInfo.opCode==L4L3InterfaceServiceConst.OP_CODE_UPD&& ship[0].bolStatus==L4L3InterfaceServiceConst.BOL_SENT)
            {
                if(!CheckBolExistIsShip(ship[0].bolId))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {ship[0].bolId} не существует или отгружена";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
                if(!ShippingIsPieceAssignedToBOL(ship,TForShipping.YESShipped))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {ship[0].bolId}";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
            }
            else if(l4MsgInfo.opCode==L4L3InterfaceServiceConst.OP_CODE_NEW&& ship[0].bolStatus==L4L3InterfaceServiceConst.BOL_SENT)
            {
                if(!CheckBolExistIsShip(ship[0].bolId))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {ship[0].bolId} не существует или отгружена";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
                if(!L4L3ShipPieceSOCheck(ship))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {ship[0].bolId}";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
            }
            else if(l4MsgInfo.opCode==L4L3InterfaceServiceConst.OP_CODE_DEL&& ship[0].bolStatus==L4L3InterfaceServiceConst.BOL_SENT)
            {
                if(!CheckBolExistNotShip(ship[0].bolId))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} Накладная: {ship[0].bolId} не существует или отгружена";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
                if(L4L3ShipPieceSOCheck(ship))
                {
                    result.isOK = false;
                    result.data = $"Таблица L4_L3_SHIPPING MSG_COUNTER {l4MsgInfo.msgCounter} как минимум одна из заготовок не может быть назначена в накладную BOL: {ship[0].bolId}";
                    check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
                }
            }
            return result;
        }

        public TCheckResult ShippingGeneralCheck(List<L4L3Shipping> ship, TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = check.InitResultWithFalse();
            result.isOK = true;
            bool firstloop = true;
            if(ship==null)
            {
                check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, $"Запись не найдена Msg_counter={l4MsgInfo.msgCounter}");
                return result;
            }
        }

        public bool ShippingIsPieceAssignedToBOL(List<L4L3Shipping> ship, TForShipping forShipping)
        {
            
            throw new NotImplementedException();
        }
    }
}
