using System;
using Repository;
using Repository.WorkModels;
using Shipping.Models;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Dapper.Oracle;
using Dapper;
using System.Collections.Generic;

namespace Shipping
{
    //Используется таблица L4_L3_Shipping
    //Файл L4L3ServiceCheckShipping
    public enum TForShipping { YESShipped,NOShipped }
    interface L4L3ServCheckShip
    {
        bool CheckPiece(string pieceId, string soId, string soLineId);
        bool L4L3ShipPieceSOCheck(L4L3Shipping ship);
        bool CheckBolExistNotShip(string strBolId);
        bool CheckBolExistIsShip(string strBolId);
        bool ShippingIsPieceAssignedToBOL(L4L3Shipping ship, TForShipping forShipping);
        bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping);
        TCheckResult ShippingCheck(L4L3Shipping ship, TL4MsgInfo l4MsgInfo);
        TCheckResult ShippingGeneralCheck(L4L3Shipping ship, TL4MsgInfo l4MsgInfo);
    }
    class L4L3ServiceCheckShipping : L4L3ServCheckShip
    {
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

        public bool L4L3ShipPieceSOCheck(L4L3Shipping ship)
        {
            return CheckPiece(ship.pieceId, ship.soId, ship.soLineId);
        }

        public TCheckResult ShippingCheck(L4L3Shipping ship,TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TCheckResult ShippingGeneralCheck(L4L3Shipping ship, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public bool ShippingIsPieceAssignedToBOL(L4L3Shipping ship, TForShipping forShipping)
        {
            
            throw new NotImplementedException();
        }
    }
}
