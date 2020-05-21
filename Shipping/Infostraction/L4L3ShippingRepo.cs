using Shipping.Models;
using Shipping.Repo;
using Repository.WorkModels;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;
using NLog;

namespace Shipping.Infostraction
{
    public class L4L3ShippingRepo : IL4L3Shipping
    {
        private Logger logger = LogManager.GetLogger(nameof(Shipping));
        public L4L3Shipping GetData(TL4MsgInfo l4MsgInfo)
        {
            L4L3Shipping shipping = new L4L3Shipping();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT * FROM L4_L3_SHIPPING WHERE MSG_COUNTER = :P_MSG_COUNTER";
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                shipping = connection.QueryFirstOrDefault<L4L3Shipping>(str, odp);
            }
            if(shipping==null)
            {
                logger.Error($"Нет данных в таблице L4_L3_SHIPPING, для Msg_Counter: {l4MsgInfo.msgCounter}");
            }
            return shipping;
        }
    }
}
