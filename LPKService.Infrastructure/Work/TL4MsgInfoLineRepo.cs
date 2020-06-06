using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using LPKService.Domain.Interfaces;
using LPKService.Domain.BaseRepository;
using LPKService.Domain.Models.Work;

namespace LPKService.Infrastructure.Work
{
    public class TL4MsgInfoLineRepo:ITL4MsgInfoLine
    {
        /// <summary>
        /// Обновление статуса сообщения
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработка кода</param>
        public void UpdateMsgStatus(TL4MsgInfo l4MsgInfo)
        {
            OracleDynamicParameters odp = new OracleDynamicParameters { BindByName = true };
            odp.Add("P_MSG_STATUS", l4MsgInfo.msgReport.status);
            odp.Add("P_MSG_REMARK", l4MsgInfo.msgReport.remark);
            odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
            string fields = "MSG_STATUS = :P_MSG_STATUS, MSG_REMARK = :P_MSG_REMARK";
            string where = "MSG_COUNTER = :P_MSG_COUNTER";
            string stm = $"UPDATE L4_L3_SERVICE_EVENT SET {fields} WHERE {where}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(stm, odp);
            } 
            where = "MSG_COUNTER = ( SELECT MSG_COUNTER_SOURCE FROM L4_L3_SERVICE_EVENT WHERE MSG_COUNTER=:P_MSG_COUNTER AND ROWNUM=1)";
            stm = $"UPDATE L4_L3_EVENT SET {fields} WHERE {where}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(stm, odp);
            }
        }
    }
}
