using Work.Models;
using Work.Repo;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;
using Repository.WorkModels;
using NLog;

namespace Work.Infostraction
{
    public class L4L3CustomerRepo : IL4L3Customer
    {
        private Logger logger = LogManager.GetLogger(nameof(Work));
        public L4L3Customer GetData(TL4MsgInfo l4MsgInfo)
        {
            L4L3Customer customer = new L4L3Customer();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT * FROM L4_L3_CUSTOMER WHERE MSG_COUNTER = :P_MSG_COUNTER";
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                customer = connection.QueryFirstOrDefault<L4L3Customer>(str, odp);
            }
            if (customer == null)
            {
                logger.Error($"Нет данных в таблице L4_L3_CUSTOMER, для Msg_Counter: {l4MsgInfo.msgCounter}");
            }
            return customer;
        }
    }
}
