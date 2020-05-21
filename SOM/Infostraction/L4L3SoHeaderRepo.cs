using SOM.Models;
using SOM.Repo;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Repository;
using Repository.WorkModels;
using NLog;
using System.Collections.Generic;

namespace SOM.Infostraction
{
    public class L4L3SoHeaderRepo : IL4L3SoHeader
    {
        private Logger logger = LogManager.GetLogger(nameof(SOM));

        public L4L3SoHeader GetData(TL4MsgInfo l4MsgInfo)
        {
            L4L3SoHeader soHeader = new L4L3SoHeader();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT * FROM L4_L3_SO_HEADER WHERE MSG_COUNTER = :P_MSG_COUNTER";
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                soHeader = connection.QueryFirstOrDefault<L4L3SoHeader>(str, odp);
            }
            if (soHeader == null)
            {
                logger.Error($"Нет данных в таблице L4_L3_SO_HEADER, для Msg_Counter: {l4MsgInfo.msgCounter}");
            }
            return soHeader;
        }
    }
}
