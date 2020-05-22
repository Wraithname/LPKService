using System.Collections.Generic;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Repository.WorkModels;

namespace Repository.Infostaction
{
    public class THeaderNoteRepo
    {
        List<THeaderNote> headernote;
        public THeaderNoteRepo()
        {
            this.headernote = new List<THeaderNote>();
        }

        public void ClearMassNote()
        {
            headernote.Clear();
        }

        public void SaveInMassForHeaderNote(int soId, string headerNote)
        {
            THeaderNote header = new THeaderNote();
            header.so_id = soId;
            header.headerNote = headerNote;
            headernote.Add(header);
        }

        public void SaveNote()
        {
            if (headernote.Count > 0)
            {
                foreach (THeaderNote header in headernote)
                {
                    if (header.headerNote != "")
                    {
                        OracleDynamicParameters odp = new OracleDynamicParameters();
                        string sqlstr = "INSERT INTO SO_HEADER_NOTE " +
                            "(SO_NOTE_DUMMY_KEY, SO_ID, " +
                            "NOTE_TEXT, INSERTION_DATETIME, INSERTED_BY, MOD_DATETIME, MOD_USER_ID) " +
                            " SELECT SEQ_SO_HEADER_NOTE_DUMMY_KEY.NEXTVAL AS SO_NOTE_DUMMY_KEY, " +
                            ":P_SO_ID, :P_HEADERNOTE,SYSDATE AS INSERTION_DATETIME," +
                            "-555 AS INSERTED_BY, SYSDATE AS MODE_DATETIME," +
                            "-555 AS MOD_USER_ID FROM DUAL";
                        odp.Add("P_SO_ID", header.so_id);
                        odp.Add("P_HEADERNOTE", header.headerNote);
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
