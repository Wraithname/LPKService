using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using LPKService.Domain.BaseRepository;
using LPKService.Domain.Models.SOM;

namespace LPKService.Infrastructure.Work
{
    public class TLineNoteRepo 
    {
        List<TLineNote> linenote;
        /// <summary>
        /// Создание листа примечаний для строк заказов
        /// </summary>
        public TLineNoteRepo()
        {
            this.linenote = new List<TLineNote>();
        }
        /// <summary>
        /// Очищение листа примечаний для строк заказов
        /// </summary>
        public void ClearNote()
        {
            linenote.Clear();
        }
        /// <summary>
        /// Добавление в лист новых примечаний для строк заказов
        /// </summary>
        /// <param name="soId">ИД заказа</param>
        /// <param name="soLineId">ИД линии</param>
        /// <param name="lineNote">Примечание</param>
        /// <param name="sapUser">Пользователь</param>
        public void SaveInMassForLineNote(int soId, int soLineId, string lineNote, string sapUser)
        {
            TLineNote line = new TLineNote();
            line.soId = soId;
            line.soLineId = soLineId;
            line.lineNote = lineNote;
            line.sSAPUser = sapUser;
            linenote.Add(line);
        }
        /// <summary>
        /// Сохранение листа примечаний
        /// </summary>
        public void SaveNote()
        {
            if (linenote.Count > 0)
            {
                foreach (TLineNote line in linenote)
                {
                    if (line.lineNote != "")
                    {
                        OracleDynamicParameters odp = new OracleDynamicParameters();
                        string sqlstr = "INSERT INTO SO_LINE_NOTE " +
                            "(SO_NOTE_DUMMY_KEY, SO_ID, SO_LINE_ID, " +
                            "NOTE_TEXT, INSERTION_DATETIME, INSERTED_BY, MOD_DATETIME, MOD_USER_ID) " +
                            "SELECT SEQ_SO_LINE_NOTE_DUMMY_KEY.NEXTVAL AS SO_NOTE_DUMMY_KEY, " +
                            ":P_SO_ID, :P_SO_LINE_ID,:P_LINENOTE,SYSDATE AS INSERTION_DATETIME," +
                            ":P_INSERTED_BY AS INSERTED_BY, SYSDATE AS MODE_DATETIME," +
                            ":P_MOD_USER_ID AS MOD_USER_ID FROM DUAL";
                        odp.Add("P_SO_ID",line.soId);
                        odp.Add("P_SO_LINE_ID",line.soLineId);
                        odp.Add("P_LINENOTE",line.lineNote);
                        odp.Add("P_INSERTED_BY",line.sSAPUser);
                        odp.Add("P_MOD_USER_ID", line.sSAPUser);
                        using (OracleConnection conn = BaseRepo.GetDBConnection())
                        {
                            conn.Execute(sqlstr, odp);
                        }
                    }
                }
            }
        }
    }
}
