using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Dapper.Oracle;
using LPKService.Domain.Models;
using LPKService.Domain.Models.Work.AutoCloseOrder;
using LPKService.Domain.Models.Work.Delivery;
using LPKService.Domain.Models.Work.Event;
using LPKService.Domain.BaseRepository;
using Oracle.ManagedDataAccess.Client;
using NLog;

namespace LPKService.Infrastructure.Builders
{
    public class BuildersRepoBase :BaseRepo
    {
        private Logger logger = LogManager.GetLogger(nameof(Work));
        /// <summary>
        /// Подключение к БД
        /// </summary>
        /// <returns>Подключение для модели</returns>
        public override OracleConnection GetConnection()
        {
            return GetDBConnection(new List<Type>
            {
                typeof(L4L3Delivery),
                typeof(L4L3DelEventDel),
                typeof(JoinedModel),
                typeof(DeliverySOHandSOL),
                typeof(DeliveryESOHandSOL),
                typeof(L4L3Event),
                typeof(AutoClose),
                typeof(VecAuto)
            });
        }
        /// <summary>
        /// Логирование sql запросов
        /// </summary>
        /// <param name="stm">Строка sql</param>
        /// <param name="odp">Параметры</param>
        protected void LogSqlWithParams(string stm, OracleDynamicParameters odp = null)
        {
            Regex regex = new Regex(" {0,}\r{0,1}\n{0,1} {2,}");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(regex.Replace(stm, " "));
            if (odp != null)
            {
                // т.к. odp.GetParameter NullPointerException приходится использовать рефлексию
                Dictionary<string, OracleDynamicParameters.OracleParameterInfo> parameters =
                    (Dictionary<string, OracleDynamicParameters.OracleParameterInfo>)odp.GetType()
                    .GetProperty("Parameters", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(odp);
                foreach (KeyValuePair<string, OracleDynamicParameters.OracleParameterInfo> param in parameters)
                {
                    sb.AppendLine($"{param.Key}: {param.Value.Value}; ");
                }
            }
            logger.Trace(sb.ToString());
        }
    }
}
