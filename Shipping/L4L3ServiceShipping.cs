using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;
using Work;
using Shipping.Models;
using Logger;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;

namespace Shipping
{
    //Используется таблица L4_L3_SHIPPING
    public enum TPieceAction { paAssign , paDeAssign }
    interface IL4L3SerShipping
    {
        TCheckResult ShippingMng(L4L3Shipping ship,TL4MsgInfo l4MsgInfo);
        void CreateBolIfNotEx(string strBolId);
        TCheckResult LocalSetPiece(TL4MsgInfo l4MsgInfo,TPieceAction action, TForShipping forShipping);
        TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo);
    }
    public class L4L3ServiceShipping : IL4L3SerShipping
    {
        private Log logger = LogFactory.GetLogger(nameof(IL4L3SerShipping));
        public void CreateBolIfNotEx(string strBolId)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string bolId="";
            string str = "SELECT BOL_ID FROM EXT_BOL_HEADER WHERE BOL_ID = :BOL_ID";
            odp.Add("BOL_ID", strBolId);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                bolId=connection.QueryFirstOrDefault<string>(str, odp);
            }
            if(bolId=="")
            {
                odp = new OracleDynamicParameters();
                str = "INSERT INTO EXT_BOL_HEADER " +
                    "(MOD_USER_ID,MOD_DATETIME, BOL_ID,CREATION_DATETIME,STATUS) " +
                    "VALUES (:MOD_USER_ID ,SYSDATE, :BOL_ID, SYSDATE, :STATUS)";
                //odp.Add("MOD_USER_ID",); //Узнать про g_oUser.Userid
                odp.Add("BOL_ID",strBolId);
                odp.Add("STATUS",L4L3InterfaceServiceConst.BOL_NOT_SENT);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                   connection.Execute(str, odp);
                }
            }
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
