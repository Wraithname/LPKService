using System;
using Dapper;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.SOM;
using LPKService.Domain.Models.Work;
using LPKService.Repository;
using Oracle.ManagedDataAccess.Client;

namespace LPKService.Infrastructure.Work
{
    public class TL4EngineInterfaceMngRepo : ITL4EngineInterfaceMng
    {
        TL4EngineInterfaceMng tL4Engine;
        TL4MsgInfo l4MsgInfo;
        #region Constant
        string userNameNotDefined = "NOT DEFINED";
        int msgRemarkMaxLen = 4000;
        #endregion
        /// <summary>
        /// Конструктор для получения модели событий
        /// </summary>
        /// <param name="l4MsgInfo">Модель таблицы L4L3Event для обработки кода</param>
        public TL4EngineInterfaceMngRepo(TL4MsgInfo l4MsgInfo)
        {
            this.l4MsgInfo = l4MsgInfo;
        }
        /// <summary>
        /// Конструктор для получения модели событий и данных
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="pL4MsgInfoPtr">Модель событий</param>
        public TL4EngineInterfaceMngRepo(L4L3SoHeader data, TL4MsgInfo pL4MsgInfoPtr)
        {
            this.tL4Engine = new TL4EngineInterfaceMng();
            this.l4MsgInfo = new TL4MsgInfo();
            tL4Engine.m_QryData=new EngineSom(data);
            tL4Engine.m_L4MsgInfoPtr = pL4MsgInfoPtr;
        }
        /// <summary>
        /// Конструктор для получения модели событий и данных
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="pL4MsgInfoPtr">Модель событий</param>
        public TL4EngineInterfaceMngRepo(L4L3Customer data, TL4MsgInfo pL4MsgInfoPtr)
        {
            this.tL4Engine = new TL4EngineInterfaceMng();
            this.l4MsgInfo = new TL4MsgInfo();
            tL4Engine.m_QryData = new EngineSom(data);
            tL4Engine.m_L4MsgInfoPtr = pL4MsgInfoPtr;
        }
        /// <summary>
        /// Получение ID пользователя
        /// </summary>
        /// <param name="pL4UserName">Имя пользователя</param>
        /// <returns>ID пользователя</returns>
        public string DecodeUserToUserId(string pL4UserName)
        {
            string userId;
            string sqlstr = $"select user_id from ALL_USERS where username='{pL4UserName}'";
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                userId = conn.ExecuteScalar<string>(sqlstr, null);
            }
            return userId;
        }
        /// <summary>
        /// Получение даты создания
        /// </summary>
        /// <returns>Дата создания</returns>
        public DateTime GetCreateDate()
        {
            return tL4Engine.m_QryData.soHeader.insertDate;
        }
        /// <summary>
        /// Получение ИД пользователя
        /// </summary>
        /// <returns></returns>
        public string GetCreateUserId()
        {
            return DecodeUserToUserId("c##OMK");
        }
        /// <summary>
        /// Получение даты
        /// </summary>
        /// <returns></returns>
        public DateTime GetModDateitme()
        {
            DateTime date;
            string sqlstr = $"SELECT MOD_DATETIME FROM L4_L3_EVENT WHERE MSG_COUNTER={l4MsgInfo.msgCounter}";
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                date = conn.ExecuteScalar<DateTime>(sqlstr, null);
            }
            return date;
        }
        /// <summary>
        /// Получение ИД пользователя
        /// </summary>
        /// <returns></returns>
        public string GetModUserId()
        {
            return GetCreateUserId();
        }
        /// <summary>
        /// Получение счетчика сообщения
        /// </summary>
        /// <returns></returns>
        public int GetMsgCounter()
        {
            return l4MsgInfo.msgCounter;
        }
        /// <summary>
        /// Обработчик фатальных ошибок
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Caption"></param>
        /// <param name="pFatal"></param>
        /// <returns></returns>
        public bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true)
        {
            if (pFatal)
                l4MsgInfo.msgReport.status = -1;
            l4MsgInfo.msgReport.remark = "";
            l4MsgInfo.msgReport.remark = ((l4MsgInfo.msgReport.remark)+Caption+Text).Substring(0,msgRemarkMaxLen);
            return !pFatal;
        }
        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pErrorCode"></param>
        /// <returns></returns>
        public bool NotifyErrorMessage(string text, string pErrorCode)
        {
            return NotifyErrorMessage(text, "", true);
        }
    }
}
