using Dapper;
using Dapper.Oracle;
using LPKService.Domain.Models.SOM;
using LPKService.Domain.Models.Work.Event;
using LPKService.Repository;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Text;

namespace LPKService.Infrastructure.SOM
{
    interface IPartitionOrder
    {
        bool PartitionOfOrder(int msgCounter);
    }
    class PartitionOrder : SOMRepoBase,IPartitionOrder
    {
        public bool PartitionOfOrder(int msgCounter)
        {
            int suffixSoId = 0; // суффикс номера заказа
            int newMessageCounter;
            List<string> cust=new List<string>();
            
                OracleDynamicParameters odp = new OracleDynamicParameters { BindByName = true };
                odp.Add("P_MSG_ct", msgCounter);
                StringBuilder stm = new StringBuilder(@"SELECT SO_LINE_ID FROM L4_L3_SO_LINE WHERE MSG_COUNTER =:P_MSG_ct");
                using (OracleConnection conn = BaseRepo.GetDBConnection())
                {
                    cust = conn.Query<string>(stm.ToString(), odp).AsList();
                }
                if (cust == null)
                    return false;
                using (OracleConnection conn = GetConnection())
                {
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (string line in cust)
                            {
                                suffixSoId++;
                                string str = "select L4_L3_SRV_MSG.NEXTVAL as val from dual";
                                newMessageCounter = conn.ExecuteScalar<int>(str, null,transaction);
                                L4L3SoLine l4L3SerSo = new L4L3SoLine();
                                //*******L4_L3_SERVICE_SO_LINE************
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
                                odpr.Add("MsgCnt", msgCounter);
                                odpr.Add("KEY_MSG_COUNTER", newMessageCounter);
                                odpr.Add("SO_LINE_ID", line);
                                l4L3SerSo=conn.QueryFirst<L4L3SoLine>(str, odpr, transaction);
                                if (l4L3SerSo != null)
                                {
                                    OracleDynamicParameters odpi = new OracleDynamicParameters();
                                    str = "INSERT INTO L4_L3_SERVICE_SO_LINE ( MSG_COUNTER,              " +
                       "                                    SO_LINE_ID,               " +
                       "                                    SO_ID,                    " +
                       "                                    SO_TYPE_CODE,             " +
                       "                                    SO_LINE_STATUS,           " +
                       "                                    SO_LINE_CREDIT_STATUS,    " +
                       "                                    DUE_DELIVERY_DATE,        " +
                       "                                    PRODUCT_TYPE,             " +
                       "                                    SO_QUALITY_CODE,          " +
                       "                                    SO_QTY,                   " +
                       "                                    SO_QTY_MAX,               " +
                       "                                    SO_QTY_MIN,               " +
                       "                                    GRADE,                    " +
                       "                                    CHEM_NORM,                " +
                       "                                    PRODUCT_DIMENSION_NORM,   " +
                       "                                    PRODUCT_MECHANICAL_NORM,  " +
                       "                                    THICK,                    " +
                       "                                    WIDTH,                    " +
                       "                                    THICK_NTOL,               " +
                       "                                    THICK_PTOL,               " +
                       "                                    WIDTH_NTOL,               " +
                       "                                    WIDTH_PTOL,               " +
                       "                                    DIAMETER_EXTERNAL_MAX,    " +
                       "                                    INTERNAL_DIAMETER,        " +
                       "                                    PIECE_WEIGHT,             " +
                       "                                    PIECE_MIN_WEIGHT,         " +
                       "                                    PIECE_MAX_WEIGHT,         " +
                       "                                    C_MIN,                    " +
                       "                                    MN_MIN,                   " +
                       "                                    SI_MIN,                   " +
                       "                                    S_MIN,                    " +
                       "                                    P_MIN,                    " +
                       "                                    CR_MIN,                   " +
                       "                                    NI_MIN,                   " +
                       "                                    CU_MIN,                   " +
                       "                                    MO_MIN,                   " +
                       "                                    N2_MIN,                   " +
                       "                                    AS_MIN,                   " +
                       "                                    TI_MIN,                   " +
                       "                                    AL_MIN,                   " +
                       "                                    V_MIN,                    " +
                       "                                    NB_MIN,                   " +
                       "                                    B_MIN,                    " +
                       "                                    C_MAX,                    " +
                       "                                    MN_MAX,                   " +
                       "                                    SI_MAX,                   " +
                       "                                    S_MAX,                    " +
                       "                                    P_MAX,                    " +
                       "                                    CR_MAX,                   " +
                       "                                    NI_MAX,                   " +
                       "                                    CU_MAX,                   " +
                       "                                    MO_MAX,                   " +
                       "                                    N2_MAX,                   " +
                       "                                    AS_MAX,                   " +
                       "                                    TI_MAX,                   " +
                       "                                    AL_MAX,                   " +
                       "                                    V_MAX,                    " +
                       "                                    NB_MAX,                   " +
                       "                                    B_MAX,                    " +
                       "                                    CEQ_MIN,                  " +
                       "                                    CEQ_MAX,                  " +
                       "                                    PCM_MIN,                  " +
                       "                                    PCM_MAX,                  " +
                       "                                    SO_ID_ERP,                " +
                       "                                    SO_LINE_ID_ERP,           " +
                       "                                    STRENGTH_CLASS,           " +
                       "                                    LENGTH,                   " +
                       "                                    LENGTH_PTOL,              " +
                       "                                    LENGTH_NTOL,              " +
                       "                                    PACK_MAX_WEIGHT,          " +
                       "                                    PACK_MIN_WEIGHT,          " +
                       "                                    SLIT_WIDTH,               " +
                       "                                    SLIT_WIDTH_PTOL,          " +
                       "                                    SLIT_WIDTH_NTOL,          " +
                       "                                    MSG_COUNTER_SOURCE,       " +
                       "                                    GARDE_CATEGORY)           " +
                       "VALUES                            ( :MSG_COUNTER,             " +
                       "                                    :SO_LINE_ID,              " +
                       "                                    :SO_ID,                   " +
                       "                                    :SO_TYPE_CODE,            " +
                       "                                    :SO_LINE_STATUS,          " +
                       "                                    :SO_LINE_CREDIT_STATUS,   " +
                       "                                    :DUE_DELIVERY_DATE,       " +
                       "                                    :PRODUCT_TYPE,            " +
                       "                                    :SO_QUALITY_CODE,         " +
                       "                                    :SO_QTY,                  " +
                       "                                    :SO_QTY_MAX,              " +
                       "                                    :SO_QTY_MIN,              " +
                       "                                    :GRADE,                   " +
                       "                                    :CHEM_NORM,               " +
                       "                                    :PRODUCT_DIMENSION_NORM,  " +
                       "                                    :PRODUCT_MECHANICAL_NORM, " +
                       "                                    :THICK,                   " +
                       "                                    :WIDTH,                   " +
                       "                                    :THICK_NTOL,              " +
                       "                                    :THICK_PTOL,              " +
                       "                                    :WIDTH_NTOL,              " +
                       "                                    :WIDTH_PTOL,              " +
                       "                                    :DIAMETER_EXTERNAL_MAX,   " +
                       "                                    :INTERNAL_DIAMETER,       " +
                       "                                    :PIECE_WEIGHT,            " +
                       "                                    :PIECE_MIN_WEIGHT,        " +
                       "                                    :PIECE_MAX_WEIGHT,        " +
                       "                                    :C_MIN,                   " +
                       "                                    :MN_MIN,                  " +
                       "                                    :SI_MIN,                  " +
                       "                                    :S_MIN,                   " +
                       "                                    :P_MIN,                   " +
                       "                                    :CR_MIN,                  " +
                       "                                    :NI_MIN,                  " +
                       "                                    :CU_MIN,                  " +
                       "                                    :MO_MIN,                  " +
                       "                                    :N2_MIN,                  " +
                       "                                    :AS_MIN,                  " +
                       "                                    :TI_MIN,                  " +
                       "                                    :AL_MIN,                  " +
                       "                                    :V_MIN,                   " +
                       "                                    :NB_MIN,                  " +
                       "                                    :B_MIN,                   " +
                       "                                    :C_MAX,                   " +
                       "                                    :MN_MAX,                  " +
                       "                                    :SI_MAX,                  " +
                       "                                    :S_MAX,                   " +
                       "                                    :P_MAX,                   " +
                       "                                    :CR_MAX,                  " +
                       "                                    :NI_MAX,                  " +
                       "                                    :CU_MAX,                  " +
                       "                                    :MO_MAX,                  " +
                       "                                    :N2_MAX,                  " +
                       "                                    :AS_MAX,                  " +
                       "                                    :TI_MAX,                  " +
                       "                                    :AL_MAX,                  " +
                       "                                    :V_MAX,                   " +
                       "                                    :NB_MAX,                  " +
                       "                                    :B_MAX,                   " +
                       "                                    :CEQ_MIN,                 " +
                       "                                    :CEQ_MAX,                 " +
                       "                                    :PCM_MIN,                 " +
                       "                                    :PCM_MAX,                 " +
                       "                                    :SO_ID_ERP,               " +
                       "                                    :SO_LINE_ID_ERP,          " +
                       "                                    :STRENGTH_CLASS,          " +
                       "                                    :LENGTH,                  " +
                       "                                    :LENGTH_PTOL,             " +
                       "                                    :LENGTH_NTOL,             " +
                       "                                    :PACK_MAX_WEIGHT,         " +
                       "                                    :PACK_MIN_WEIGHT,         " +
                       "                                    :SLIT_WIDTH,              " +
                       "                                    :SLIT_WIDTH_PTOL,         " +
                       "                                    :SLIT_WIDTH_NTOL,         " +
                       "                                    :MSG_COUNTER_SOURCE,      " +
                       "                                    :GRADE_CATEGORY)          ";
                                    odpi.Add("MSG_COUNTER", l4L3SerSo.keyMsgCounter);
                                    odpi.Add("SO_LINE_ID", l4L3SerSo.soLineId);
                                    odpi.Add("SO_ID", l4L3SerSo.soId);
                                    odpi.Add("SO_TYPE_CODE", l4L3SerSo.soTypeCode);
                                    odpi.Add("SO_LINE_STATUS", l4L3SerSo.soLineStatus);
                                    odpi.Add("SO_LINE_CREDIT_STATUS", l4L3SerSo.soLineCreditStatus);
                                    odpi.Add("DUE_DELIVERY_DATE", l4L3SerSo.dueDeliveryDate);
                                    odpi.Add("PRODUCT_TYPE", l4L3SerSo.productType);
                                    odpi.Add("SO_QUALITY_CODE", l4L3SerSo.soQualityCode);
                                    odpi.Add("SO_QTY", l4L3SerSo.soQty);
                                    odpi.Add("SO_QTY_MAX", l4L3SerSo.soQtyMax);
                                    odpi.Add("SO_QTY_MIN", l4L3SerSo.soQtyMin);
                                    odpi.Add("GRADE", l4L3SerSo.grade);
                                    odpi.Add("CHEM_NORM", l4L3SerSo.chemNorm);
                                    odpi.Add("PRODUCT_DIMENSION_NORM", l4L3SerSo.productDimensionNorm);
                                    odpi.Add("PRODUCT_MECHANICAL_NORM", l4L3SerSo.productMechanicalNorm);
                                    odpi.Add("THICK", l4L3SerSo.thick);
                                    odpi.Add("WIDTH", l4L3SerSo.width);
                                    odpi.Add("THICK_NTOL", l4L3SerSo.thickNtol);
                                    odpi.Add("THICK_PTOL", l4L3SerSo.thickPtol);
                                    odpi.Add("WIDTH_NTOL", l4L3SerSo.widthNtol);
                                    odpi.Add("WIDTH_PTOL", l4L3SerSo.widthPtol);
                                    odpi.Add("DIAMETER_EXTERNAL_MAX", l4L3SerSo.diameterExternalMax);
                                    odpi.Add("INTERNAL_DIAMETER", l4L3SerSo.intervalDiameter);
                                    odpi.Add("PIECE_WEIGHT", l4L3SerSo.pieceWeight);
                                    odpi.Add("PIECE_MIN_WEIGHT", l4L3SerSo.pieceMinWeight);
                                    odpi.Add("PIECE_MAX_WEIGHT", l4L3SerSo.pieceMaxWeight);
                                    odpi.Add("C_MIN", l4L3SerSo.cMin);
                                    odpi.Add("MN_MIN", l4L3SerSo.mnMin);
                                    odpi.Add("SI_MIN", l4L3SerSo.siMin);
                                    odpi.Add("S_MIN", l4L3SerSo.sMin);
                                    odpi.Add("P_MIN", l4L3SerSo.pMin);
                                    odpi.Add("CR_MIN", l4L3SerSo.crMin);
                                    odpi.Add("NI_MIN", l4L3SerSo.niMin);
                                    odpi.Add("CU_MIN", l4L3SerSo.cuMin);
                                    odpi.Add("MO_MIN", l4L3SerSo.moMin);
                                    odpi.Add("N2_MIN", l4L3SerSo.n2Min);
                                    odpi.Add("AS_MIN", l4L3SerSo.asMin);
                                    odpi.Add("TI_MIN", l4L3SerSo.tiMin);
                                    odpi.Add("AL_MIN", l4L3SerSo.alMin);
                                    odpi.Add("V_MIN", l4L3SerSo.vMin);
                                    odpi.Add("NB_MIN", l4L3SerSo.ndMin);
                                    odpi.Add("B_MIN", l4L3SerSo.bMin);
                                    odpi.Add("C_MAX", l4L3SerSo.cMax);
                                    odpi.Add("MN_MAX", l4L3SerSo.mnMax);
                                    odpi.Add("SI_MAX", l4L3SerSo.siMax);
                                    odpi.Add("S_MAX", l4L3SerSo.sMax);
                                    odpi.Add("P_MAX", l4L3SerSo.pMax);
                                    odpi.Add("CR_MAX", l4L3SerSo.crMax);
                                    odpi.Add("NI_MAX", l4L3SerSo.niMax);
                                    odpi.Add("CU_MAX", l4L3SerSo.cuMax);
                                    odpi.Add("MO_MAX", l4L3SerSo.moMax);
                                    odpi.Add("N2_MAX", l4L3SerSo.n2Max);
                                    odpi.Add("AS_MAX", l4L3SerSo.asMax);
                                    odpi.Add("TI_MAX", l4L3SerSo.tiMax);
                                    odpi.Add("AL_MAX", l4L3SerSo.alMax);
                                    odpi.Add("V_MAX", l4L3SerSo.vMax);
                                    odpi.Add("NB_MAX", l4L3SerSo.ndMax);
                                    odpi.Add("B_MAX", l4L3SerSo.bMax);
                                    odpi.Add("CEQ_MIN", l4L3SerSo.ceqMin);
                                    odpi.Add("CEQ_MAX", l4L3SerSo.ceqMax);
                                    odpi.Add("PCM_MIN", l4L3SerSo.pcmMin);
                                    odpi.Add("PCM_MAX", l4L3SerSo.pcmMax);
                                    odpi.Add("SO_ID_ERP", l4L3SerSo.soIdErp);
                                    odpi.Add("SO_LINE_ID_ERP", l4L3SerSo.soLineIdErp);
                                    odpi.Add("STRENGTH_CLASS", l4L3SerSo.strengthClass);
                                    odpi.Add("LENGTH", l4L3SerSo.length);
                                    odpi.Add("LENGTH_PTOL", l4L3SerSo.lengthPtol);
                                    odpi.Add("LENGTH_NTOL", l4L3SerSo.lengthNtol);
                                    odpi.Add("PACK_MAX_WEIGHT", l4L3SerSo.packMaxWeight);
                                    odpi.Add("PACK_MIN_WEIGHT", l4L3SerSo.packMinWeight);
                                    odpi.Add("SLIT_WIDTH", l4L3SerSo.slitWidth);
                                    odpi.Add("SLIT_WIDTH_PTOL", l4L3SerSo.slitWidthPtol);
                                    odpi.Add("SLIT_WIDTH_NTOL", l4L3SerSo.slitWidthNtol);
                                    odpi.Add("MSG_COUNTER_SOURCE", l4L3SerSo.msgCounter);
                                    odpi.Add("GRADE_CATEGORY", l4L3SerSo.gradeCategory);
                                    conn.Execute(str, odpi, transaction);
                                }
                                //*******L4_L3_SERVICE_EVENT************
                                L4L3Event l4L3Event = new L4L3Event();
                                OracleDynamicParameters odpe = new OracleDynamicParameters();
                                str = "SELECT :KEY_MSG_COUNTER AS KEY_MSG_COUNTER," +
                                    "l4l3Ev.MSG_ID," +
                                    "l4l3Ev.MSG_DATETIME," +
                                    "l4l3Ev.OP_CODE," +
                                    "l4l3Ev.KEY_STRING_1," +
                                    "l4l3Ev.KEY_STRING_2," +
                                    "l4l3Ev.KEY_NUMBER_1," +
                                    "l4l3Ev.KEY_NUMBER_2," +
                                    "l4l3Ev.MSG_STATUS," +
                                    "l4l3Ev.MSG_REMARK," +
                                    "l4l3Ev.ERP_MSG_ID," +
                                    "l4l3Ev.MOD_DATETIME," +
                                    "l4l3Ev.MSG_COUNTER AS MSG" +
                                    "FROM L4_L3_EVENT l4l3Ev " +
                                    "WHERE l4l3Ev.MSG_COUNTER=:MsgCnt";
                                odpe.Add("MsgCnt", msgCounter);
                                odpe.Add("KEY_MSG_COUNTER", newMessageCounter);
                                l4L3Event=conn.QueryFirst<L4L3Event>(str, odpe, transaction);
                                if (l4L3Event != null)
                                {
                                    str = "INSERT INTO L4_L3_SERVICE_EVENT (MSG_COUNTER, " +
                                        " MSG_ID," +
                                        "MSG_DATETIME," +
                                        "OP_CODE," +
                                        "KEY_STRING_1," +
                                        "KEY_STRING_2," +
                                        "KEY_NUMBER_1," +
                                        "KEY_NUMBER_2," +
                                        "MSG_STATUS," +
                                        "MSG_REMARK," +
                                        "ERP_MSG_ID," +
                                        "MOD_DATETIME," +
                                        "MSG_COUNTER_SOURCE)" +
                                        "VALUES (:MSG_COUNTER," +
                                        ":MSG_ID," +
                                        ":MSG_DATETIME," +
                                        ":OP_CODE," +
                                        ":KEY_STRING_1," +
                                        ":KEY_STRING_2," +
                                        ":KEY_NUMBER_1," +
                                        ":KEY_NUMBER_2," +
                                        ":MSG_STATUS," +
                                        ":MSG_REMARK," +
                                        ":ERP_MSG_ID," +
                                        ":MOD_DATETIME," +
                                        ":MSG_COUNTER_SOURCE)";
                                    OracleDynamicParameters odpre = new OracleDynamicParameters();
                                    odpre.Add("MSG_COUNTER", l4L3Event.keyMsgCounter);
                                    odpre.Add("MSG_ID", l4L3Event.msgId);
                                    odpre.Add("MSG_DATETIME", l4L3Event.msgDatetime);
                                    odpre.Add("OP_CODE", l4L3Event.opCode);
                                    odpre.Add("KEY_STRING_1", l4L3Event.keyString1);
                                    odpre.Add("KEY_STRING_2", l4L3Event.keyString2);
                                    odpre.Add("KEY_NUMBER_1", l4L3Event.keyNumber1);
                                    odpre.Add("KEY_NUMBER_2", l4L3Event.keyNumber2);
                                    odpre.Add("MSG_STATUS", l4L3Event.status);
                                    odpre.Add("MSG_REMARK", l4L3Event.remark);
                                    odpre.Add("ERP_MSG_ID", l4L3Event.erpMsgId);
                                    odpre.Add("MOD_DATETIME", l4L3Event.modDateTime);
                                    odpre.Add("MSG_COUNTER_SOURCE", l4L3Event.msgCounter);
                                    conn.Execute(str, odpre, transaction);
                                }
                                //*******L4_L3_SERVICE_SO_HEADER************
                                L4L3SoHeader l4L3SoHeader = new L4L3SoHeader();
                                OracleDynamicParameters trer = new OracleDynamicParameters();
                                str = "SELECT :KEY_MSG_COUNTER AS KEY_MSG_COUNTER,";
                                if (cust.Count == 1)
                                    str += "l4l3sh.SO_ID AS SO_ID,";
                                else
                                    str += "l4l3sh.SO_ID||''_''||" + suffixSoId.ToString() + " AS SO_ID, ";
                                str += "l4l3sh.INSERT_DATE," +
                                    "l4l3sh.CUSTOMER_ID," +
                                    "l4l3sh.CUSTOMER_PO," +
                                    "l4l3sh.CUSTOMER_PO_DATE," +
                                    "l4l3sh.SO_NOTES," +
                                    "l4l3sh.MSG_COUNTER AS MSG " +
                                    "FROM L4_L3_SO_HEADER l4l3sh" +
                                    "WHERE l4l3sh.MSG_COUNTER=:MsgCnt";
                                trer.Add("MsgCnt", msgCounter);
                                trer.Add("KEY_MSG_COUNTER", newMessageCounter);
                                l4L3SoHeader= conn.QueryFirst<L4L3SoHeader>(str, trer, transaction);
                                if(l4L3SoHeader!=null)
                                {
                                    OracleDynamicParameters tr2er = new OracleDynamicParameters();
                                    str = "INSERT INTO L4_L3_SERVICE_SO_HEADER (MSG_COUNTER," +
                                        "SO_ID," +
                                        "INSERT_DATE," +
                                        "CUSTOMER_ID," +
                                        "CUSTOMER_PO," +
                                        "CUSTOMER_PO_DATE," +
                                        "SO_NOTES," +
                                        "MSG_COUNTER_SOURCE)" +
                                        "VALUES (:MSG_COUNTER," +
                                        ":SO_ID," +
                                        ":INSERT_DATE," +
                                        ":CUSTOMER_ID," +
                                        ":CUSTOMER_PO," +
                                        ":CUSTOMER_PO_DATE," +
                                        ":SO_NOTES," +
                                        ":MSG_COUNTER_SOURCE) ";
                                    tr2er.Add("MSG_COUNTER", l4L3SoHeader.msgCounter);
                                    tr2er.Add("SO_ID", l4L3SoHeader.soID);
                                    tr2er.Add("INSERT_DATE", l4L3SoHeader.insertDate);
                                    tr2er.Add("CUSTOMER_ID", l4L3SoHeader.customerId);
                                    tr2er.Add("CUSTOMER_PO", l4L3SoHeader.customerPo);
                                    tr2er.Add("CUSTOMER_PO_DATE", l4L3SoHeader.customerPoDate);
                                    tr2er.Add("SO_NOTES", l4L3SoHeader.soNotes);
                                    tr2er.Add("MSG_COUNTER_SOURCE", l4L3SoHeader.msgCounter);
                                    conn.Execute(str, tr2er, transaction);
                                }
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
            return false;
        }
    }
}