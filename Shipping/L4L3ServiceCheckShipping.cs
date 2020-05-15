using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;
using Logger;

namespace Shipping
{
    //Используется таблица L4_L3_Shipping
    //Файл L4L3ServiceCheckShipping
    public enum TForShipping { YESShipped,NOShipped }
    interface L4L3ServCheckShip
    {
        bool CheckPiece(string pieceId, string soId, string soLineId);
        bool L4L3ShipPieceSOCheck();
        bool CheckBolExistNotShip(string strBolId);
        bool CheckBolExistIsShip(string strBolId);
        bool ShippingIsPieceAssignedToBOL(TForShipping forShipping);
        bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping);
        TCheckResult ShippingCheck(TL4MsgInfo l4MsgInfo);
        TCheckResult ShippingGeneralCheck(TL4MsgInfo l4MsgInfo);
    }
    class L4L3ServiceCheckShipping : L4L3ServCheckShip
    {
        private Log logger = LogFactory.GetLogger(nameof(IL4L3SerShipping));
        public bool CheckBolExistIsShip(string strBolId)
        {
            throw new NotImplementedException();
        }

        public bool CheckBolExistNotShip(string strBolId)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfPieceRelatedToBOL(string strBolId, TForShipping forShipping)
        {
            throw new NotImplementedException();
        }

        public bool CheckPiece(string pieceId, string soId, string soLineId)
        {
            throw new NotImplementedException();
        }

        public bool L4L3ShipPieceSOCheck()
        {
            throw new NotImplementedException();
        }

        public TCheckResult ShippingCheck(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TCheckResult ShippingGeneralCheck(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public bool ShippingIsPieceAssignedToBOL(TForShipping forShipping)
        {
            throw new NotImplementedException();
        }
    }
}
