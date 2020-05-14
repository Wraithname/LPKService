using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;
using Shipping.Models;
using Logger;

namespace Shipping
{
    //Используется таблица L4_L3_SHIPPING
    public enum TPieceAction { paAssign , paDeAssign }
    interface IL4L3SerShipping
    {
        TCheckResult ShippingMng(L4L3Shipping ship,TL4MsgInfo l4MsgInfo);
        void CreateBol(string strBolId);
        TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo,TPieceAction action, TForShipping forShipping);
        TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo);
    }
    public class L4L3ServiceShipping : IL4L3SerShipping
    {
        private Log logger = LogFactory.GetLogger(nameof(IL4L3SerShipping));
        public void CreateBol(string strBolId)
        {
            throw new NotImplementedException();
        }

        public TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo, TPieceAction action, TForShipping forShipping)
        {
            throw new NotImplementedException();
        }

        public TCheckResult ShippingMng(L4L3Shipping ship, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }
    }
}
