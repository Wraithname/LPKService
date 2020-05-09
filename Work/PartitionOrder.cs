using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work
{
    interface IPartitionOrder
    {
        bool PartitionOfOrder(int MsgCounter);
    }
    struct L4L3_soline
    {
        public string lineID { get; set; }
    }
    class PartitionOrder : IPartitionOrder
    {
        public bool PartitionOfOrder(int MsgCounter)
        {
            int suffixSoId = 0;
            int newMessageCounter;
            List<L4L3_soline> cust=new List<L4L3_soline>();
            try
            {
                OracleDynamicParameters odp = new OracleDynamicParameters { BindByName = true };
                odp.Add("P_MSG_ct", MsgCounter);
                StringBuilder stm = new StringBuilder(@"SELECT SO_LINE_ID FROM L4_L3_SO_LINE WHERE MSG_COUNTER =:P_MSG_ct");
                using (OracleConnection conn = BaseRepo.GetDBConnection())
                {
                    cust = conn.Query<L4L3_soline>(stm.ToString(), odp).AsList();
                }
                if (cust == null)
                    return false;
                using (OracleConnection conn = BaseRepo.GetDBConnection())
                {
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (L4L3_soline line in cust)
                            {
                                suffixSoId++;
                                string str = "select L4_L3_SRV_MSG.NEXTVAL as val from dual";
                                newMessageCounter = conn.QueryFirst<int>(str, null,transaction);
                                OracleDynamicParameters odpr = new OracleDynamicParameters();
                                str = "SELECT   :KEY_MSG_COUNTER as KEY_MSG_COUNTER,l4l3sl.SO_LINE_ID, ";
                                if (cust.Count == 1)
                                    str += "l4l3sl.SO_ID  AS SO_ID,";
                                else
                                    str += "l4l3sl.SO_ID||''_''||" + suffixSoId.ToString() + " AS SO_ID,";
                                str += " l4l3sl.SO_TYPE_CODE, " +
                                    " l4l3sl.SO_LINE_STATUS, " +
                                    " l4l3sl.SO_LINE_CREDIT_STATUS," +
                                    "l4l3sl.DUE_DELIVERY_DATE, " +
                                    "l4l3sl.PRODUCT_TYPE, " +
                                    "l4l3sl.SO_QUALITY_CODE," +
                                    "l4l3sl.SO_QTY," +
                                    "l4l3sl.SO_QTY_MAX,                 " +
                     "        l4l3sl.SO_QTY_MIN,                 " +
                     "        l4l3sl.GRADE,                      " +
                     "        l4l3sl.CHEM_NORM,                  " +
                     "        l4l3sl.PRODUCT_DIMENSION_NORM,     " +
                     "        l4l3sl.PRODUCT_MECHANICAL_NORM,    " +
                     "        l4l3sl.THICK,                      " +
                     "        l4l3sl.WIDTH,                      " +
                     "        l4l3sl.THICK_NTOL,                 " +
                     "        l4l3sl.THICK_PTOL,                 " +
                     "        l4l3sl.WIDTH_NTOL,                 " +
                     "        l4l3sl.WIDTH_PTOL,                 " +
                     "        l4l3sl.DIAMETER_EXTERNAL_MAX,      " +
                     "        l4l3sl.INTERNAL_DIAMETER,          " +
                     "        l4l3sl.PIECE_WEIGHT,               " +
                     "        l4l3sl.PIECE_MIN_WEIGHT,           " +
                     "        l4l3sl.PIECE_MAX_WEIGHT,           " +
                     "        l4l3sl.C_MIN,                      " +
                     "        l4l3sl.MN_MIN,                     " +
                     "        l4l3sl.SI_MIN,                     " +
                     "        l4l3sl.S_MIN,                      " +
                     "        l4l3sl.P_MIN,                      " +
                     "        l4l3sl.CR_MIN,                     " +
                     "        l4l3sl.NI_MIN,                     " +
                     "        l4l3sl.CU_MIN,                     " +
                     "        l4l3sl.MO_MIN,                     " +
                     "        l4l3sl.N2_MIN,                     " +
                     "        l4l3sl.AS_MIN,                     " +
                     "        l4l3sl.TI_MIN,                     " +
                     "        l4l3sl.AL_MIN,                     " +
                     "        l4l3sl.V_MIN,                      " +
                     "        l4l3sl.NB_MIN,                     " +
                     "        l4l3sl.B_MIN,                      " +
                     "        l4l3sl.C_MAX,                      " +
                     "        l4l3sl.MN_MAX,                     " +
                     "        l4l3sl.SI_MAX,                     " +
                     "        l4l3sl.S_MAX,                      " +
                     "        l4l3sl.P_MAX,                      " +
                     "        l4l3sl.CR_MAX,                     " +
                     "        l4l3sl.NI_MAX,                     " +
                     "        l4l3sl.CU_MAX,                     " +
                     "        l4l3sl.MO_MAX,                     " +
                     "        l4l3sl.N2_MAX,                     " +
                     "        l4l3sl.AS_MAX,                     " +
                     "        l4l3sl.TI_MAX,                     " +
                     "        l4l3sl.AL_MAX,                     " +
                     "        l4l3sl.V_MAX,                      " +
                     "        l4l3sl.NB_MAX,                     " +
                     "        l4l3sl.B_MAX,                      " +
                     "        l4l3sl.CEQ_MIN,                    " +
                     "        l4l3sl.CEQ_MAX,                    " +
                     "        l4l3sl.PCM_MIN,                    " +
                     "        l4l3sl.PCM_MAX,                    " +
                     "        l4l3sl.SO_ID_ERP,                  " +
                     "        l4l3sl.SO_LINE_ID_ERP,             " +
                     "        l4l3sl.STRENGTH_CLASS,             " +
                     "        l4l3sl.LENGTH,                     " +
                     "        l4l3sl.LENGTH_PTOL,                " +
                     "        l4l3sl.LENGTH_NTOL,                " +
                     "        l4l3sl.PACK_MAX_WEIGHT,            " +
                     "        l4l3sl.PACK_MIN_WEIGHT,            " +
                     "        l4l3sl.SLIT_WIDTH,                 " +
                     "        l4l3sl.SLIT_WIDTH_PTOL,            " +
                     "        l4l3sl.SLIT_WIDTH_NTOL,            " +
                     "        l4l3sl.MSG_COUNTER AS MSG,         " +
                     "        l4l3sl.GRADE_CATEGORY              " +
                     "FROM L4_L3_SO_LINE l4l3sl                  " +
                     "WHERE l4l3sl.MSG_COUNTER=:MsgCnt           " +
                     "AND   l4l3sl.SO_LINE_ID=:SO_LINE_ID        ";
                                odpr.Add("MsgCnt", MsgCounter);
                                odpr.Add("KEY_MSG_COUNTER", newMessageCounter);
                                odpr.Add("SO_LINE_ID", line.lineID);
                                conn.Execute(str, odpr, transaction);
                                //Продолжить отсюда
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            
                        }
                    }
                }
            }
            catch {  };
            return false;
        }
    }
}
