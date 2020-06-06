using LPKService.Domain.Models.Work;
using LPKService.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using NLog;
using LPKService.Domain.BaseRepository;
using LPKService.Domain.Models.SOM;

namespace LPKService.Infrastructure.SOM
{
    public class L4L3SoHeaderRepo : SOMRepoBase,IL4L3SoHeader
    {
        private Logger logger = LogManager.GetLogger(nameof(SOM));
        /// <summary>
        /// Получение данных из таблицы L4_L3_SO_HEADER для обработки
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы</param>
        /// <returns>Модель таблицы L4_L3_SO_HEADER</returns>
        public L4L3SoHeader GetData(TL4MsgInfo l4MsgInfo)
        {
            L4L3SoHeader soHeader = new L4L3SoHeader();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "SELECT * FROM L4_L3_SO_HEADER WHERE MSG_COUNTER = :P_MSG_COUNTER";
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = GetConnection())
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
