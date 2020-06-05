using Dapper;
using Dapper.Oracle;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.Material;
using LPKService.Domain.Models.Work;
using LPKService.Infrastructure.Repository;
using LPKService.Repository;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace LPKService.Infrastructure.Material
{
    public class Material : MatRepoBase,IMaterial
    {
        IGlobalCheck check;
        /// <summary>
        /// Добавление новомого перемещения (требуется дальнейшая реализация)
        /// </summary>
        /// <param name="matcode">Код материала</param>
        /// <param name="mt_reconciliation"></param>
        /// <param name="movement_datetime">Дата перемещения</param>
        /// <param name="movement_qty">Количество</param>
        /// <param name="userID">ИД пользователя</param>
        /// <param name="num">Число</param>
        /// <param name="title">Заголовок</param>
        /// <param name="mes">Сообщение</param>
        /// <returns>Пометка о выполнении</returns>
        public bool InsertNewMovement(string matcode, string mt_reconciliation, DateTime movement_datetime, float movement_qty, int userID, int num, string title, string mes = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Обработчик кода перемещения материала
        /// </summary>
        /// <param name="l4MsgInfo">Модель L4L3Event для обработки кода</param>
        /// <returns>Результат обработки</returns>
        public TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo)
        {
            L4L3InterfaceServiceGlobalCheck global = new L4L3InterfaceServiceGlobalCheck();
            TCheckResult checkResult = global.InitResultWithFalse();
            List<L4L3RmAndMatCat> l4L3Rms = new List<L4L3RmAndMatCat>();
            OracleDynamicParameters odp = new OracleDynamicParameters();
            string str = "select  l4.material_id as sap_code, " +
                "M.MATERIAL_CODE," +
                "m.material_name," +
                "m.actual_qty l3_qty," +
                "l4.material_amount*1000 as material_amount," +
                "case" +
                "when (m.actual_qty < l4.material_amount*1000) then" +
                "(-1)*(m.actual_qty - l4.material_amount*1000)" +
                "else" +
                "(m.actual_qty - l4.material_amount*10000)" +
                "end as movement_qty" +
                "l4.movement_datetime" +
                "from    L4_L3_RAW_MATERIAL l4," +
                "mat_catalog m " +
                "where trim(L4.MATERIAL_ID) = trim(M.MATERIAL_CODE_L4) " +
                "and l4.msg_counter = :Counter";
            odp.Add("Counter", l4MsgInfo.msgCounter);
            using (OracleConnection connection = GetConnection())
            {
                l4L3Rms = connection.Query<L4L3RmAndMatCat>(str, odp).AsList();
            }
            if (l4L3Rms.Count == 0)
            {
                l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                l4MsgInfo.msgReport.remark = "Код материала не найдена в БД МЕТ2000";
            }
            else
            {
                foreach (L4L3RmAndMatCat l4L3Rm in l4L3Rms)
                {
                    if (InsertNewMovement(l4L3Rm.materialCode, "MT_RECONCILIATION", l4L3Rm.movementdatetime, l4L3Rm.movmetnqty, 1123, 0, "Корректировка остатков от SAP"))
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS, "Материал перемещен");
                    else
                        check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, "Ошибка добавления материала");
                }
            }
            return checkResult;
        }
    }
}
