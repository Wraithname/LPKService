using System;
using System.Collections.Generic;
using System.Data;
using LPKService.Domain.Models.Work;
using LPKService.Domain.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using NLog;
using LPKService.Infrastructure.Repository;
using LPKService.Repository;
using LPKService.Infrastructure.Work;
using LPKService.Domain.Models.SOM;

namespace LPKService.Infrastructure.SOM
{
    //Используется таблица L4_L3_SO_HEADER
    //Файл SOManagment.pas
    public enum TContractType { coInternal, coContract }
    public enum TShiptoType { Shipto, Billto };
    struct l4sol
    {
        public string soLineId { get; set; }
        public string metSoLineId { get; set; }
    }
    public class SOManagment : ISOManagment
    {
        private Logger logger = LogManager.GetLogger(nameof(SOM));
        List<TLineNote> lines = new List<TLineNote>();
        TLineNote line;
        string onetoSeveralirderFromSap = "";
        string m_strSO_Line_Id_Params = "";
        string m_strSO_Line_Id_MET = "";
        string m_strSo_Lines_For_Where = "";
        IGlobalCheck check;
        public void AddVsw_detailsToOrder(TL4MsgInfo l4MsgInfo)
        {
            string vsw_detail, soDescID, soIdv;
            OracleDynamicParameters odp = new OracleDynamicParameters();
            try
            {
                string sqlstr = "SELECT case when L4SHIPTO.VSW_DETAILS is not null and length(L4SHIPTO.VSW_DETAILS) =1 then 'ТЭСЦ-'||L4SHIPTO.VSW_DETAILS else 'не_ВМЗ' end AS VSW_DETAILS " +
                    "FROM L4_L3_SO_LINE_SHIPTO L4SHIPTO " +
                    "WHERE L4SHIPTO.MSG_COUNTER = :P_MSG_COUNTER";
                odp.Add("P_MSG_COUNTER", l4MsgInfo.msgCounter);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    vsw_detail = connection.QueryFirstOrDefault<string>(sqlstr, odp);
                }
                if (vsw_detail == null)
                    vsw_detail = "не_ВМЗ";
                sqlstr = "SELECT SO_ID FROM L4_L3_SO_HEADER WHERE MSG_COUNTER = :P_MSG_COUNTER";
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    soDescID = connection.QueryFirstOrDefault<string>(sqlstr, odp);
                }
                if (soDescID == null)
                    soDescID = "-9999";
                odp = null;
                if (onetoSeveralirderFromSap == "Y")
                    sqlstr = $"SELECT SO_ID FROM SALES_ORDER_HEADER WHERE SO_DESCR_ID = '{soDescID}_{m_strSO_Line_Id_MET}' ";
                else
                    sqlstr = $"SELECT SO_ID FROM SALES_ORDER_HEADER WHERE SO_DESCR_ID = '{soDescID}'";
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    soIdv = connection.QueryFirstOrDefault<string>(sqlstr, odp);
                }
                int soIDV;
                if (soIdv != null)
                    Int32.TryParse(soIdv, out soIDV);
                else
                    soIDV = -9999;
                sqlstr = "SELECT SO_LINE_ID FROM SALES_ORDER_LINE WHERE SO_ID=:P_SO_ID";
                odp.Add("P_SO_ID", soIDV);
                List<string> lines = new List<string>();
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    lines = connection.Query<string>(sqlstr, odp).AsList();
                }
                odp = null;
                if (lines != null)
                {
                    foreach (string soLineID in lines)
                    {
                        string rcrt;
                        sqlstr = "SELECT SO_ID FROM CONFIG_COMM_ATTRB_OF_SO_LINE " +
                            "WHERE SO_ID = :P_SO_ID " +
                            "AND  SO_LINE_ID = :P_SO_LINE_ID " +
                            "AND  ATTRB_CODE =  'VSW_DETAILS'";
                        odp.Add("P_SO_ID", soIDV);
                        odp.Add("P_SO_LINE_ID", soLineID);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            rcrt = connection.QueryFirstOrDefault<string>(sqlstr, odp);
                        }
                        if (rcrt == null)
                        {
                            sqlstr = "INSERT INTO CONFIG_COMM_ATTRB_OF_SO_LINE(SO_ID,SO_LINE_ID,ATTRB_CODE,ATTRB_AN_VALUE) " +
                                $"VALUES(:P_SO_ID,:P_SO_LINE_ID,'VSW_DETAILS','{vsw_detail}')";
                            using (OracleConnection connection = BaseRepo.GetDBConnection())
                            {
                                connection.Execute(sqlstr, odp);
                            }
                        }
                        else
                        {
                            sqlstr = $"UPDATE CONFIG_COMM_ATTRB_OF_SO_LINE SET ATTRB_AN_VALUE = '{vsw_detail}' " +
                                "WHERE SO_ID = :P_SO_ID " +
                                "AND  SO_LINE_ID = :P_SO_LINE_ID" +
                                "AND  ATTRB_CODE =  'VSW_DETAILS'";
                            using (OracleConnection connection = BaseRepo.GetDBConnection())
                            {
                                connection.Execute(sqlstr, odp);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { logger.Error($"Ошибка при обработке vsw_detail {e.Message}"); }

        }

        public bool AlreadyInsertInSuspended(int iSoID)
        {
            int res = -1;
            string sqlstr = $"SELECT COUNT(*) AS N FROM SALES_ORDER_LINE WHERE SO_ID = {iSoID} AND SO_LINE_STATUS = 5";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                res = connection.ExecuteScalar<int>(sqlstr, null);
            }
            if (res > 0)
                return true;
            return false;
        }

        public TCheckResult AttributeCheck(L4L3SoHeader qryData, TL4MsgInfo l4MsgInfo)
        {
            TCheckResult result = new TCheckResult();
            result.isOK = false;
            L4L3SoLine l4L3SoLine;
            string sqlstr = $"SELECT * FROM L4_L3_SO_LINE WHERE MSG_COUNTER = {qryData.msgCounter}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                l4L3SoLine = connection.QueryFirstOrDefault<L4L3SoLine>(sqlstr, null);
            }
            if (l4L3SoLine == null)
            {
                result.isOK = false;
                result.data = "Заказ не имеет записей в таблице L4_L3_SO_LINE";
                l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                l4MsgInfo.msgReport.remark = result.data;
            }
            else
                result.isOK = true;
            if (!result.isOK)
                check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_ERROR, result.data);
            else
                check.SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS, result.data);
            return result;
        }

        public void BlockForProcess(TL4MsgInfo l4MsgInfo, bool serRSer)
        {
            string sqlstr = "";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            if (serRSer)
                sqlstr = "Select UPDATE_BLOCK_ORDER(1,:msg_counter) FROM DUAL";
            else
                sqlstr = "Select UPDATE_BLOCK_ORDER(0,:msg_counter) FROM DUAL";
            odp.Add("msg_counter", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                connection.Execute(sqlstr, odp);
            }
        }

        public bool CheckInquiryLinesStatus(int iMsgCounter)
        {
            int iCounterSale, iCounterLine;
            bool result = false;
            string sqlstr = $"select count(*) as counter from sales_order_line where  so_id = (select inquiry_id from   L4_L3_SO_HEADER where  msg_counter = {iMsgCounter} ) " +
                $"and so_line_id in (select (so_line_id / 10) from   L4_L3_SO_LINE where  msg_counter = {iMsgCounter} ) and so_line_status  = 8";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                iCounterSale = connection.ExecuteScalar<int>(sqlstr, null);
            }
            sqlstr = $"select count(*) as counter from   L4_L3_SO_LINE where  msg_counter = {iMsgCounter}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                iCounterLine = connection.ExecuteScalar<int>(sqlstr, null);
            }
            if (iCounterLine != 0 && iCounterSale != 0)
                if (iCounterLine == iCounterSale)
                    result = true;
            return result;
        }

        public TL4MsgInfo CheckUpdateOPCODE(TL4MsgInfo l4MsgInfo)
        {
            TL4MsgInfo tl4 = l4MsgInfo;
            int count;
            string sqlstr = $"select count(*) as counters " +
                $"from   l4_l3_event lle_actual, " +
                $"l4_l3_event lle_before, " +
                $"l4_l3_so_header llsh_actual, " +
                $"l4_l3_so_header llsh_before, " +
                $"(select lle_actual.msg_counter, " +
                $"count(*) as counters " +
                $"from l4_l3_event lle_actual, " +
                $"l4_l3_event lle_before, " +
                $"l4_l3_so_header llsh_actual, " +
                $"l4_l3_so_header llsh_before " +
                $"where lle_actual.msg_counter = llsh_actual.msg_counter " +
                $"and lle_before.msg_counter = llsh_before.msg_counter " +
                $"and llsh_before.SO_ID = llsh_actual.so_id " +
                $"and lle_before.op_code = 1 " +
                $"and lle_actual.op_code = 2 " +
                $"and lle_before.msg_status = 2 " +
                $"and lle_actual.msg_counter = {tl4.msgCounter} " +
                $"group by lle_actual.msg_counter) yetinserted " +
                $"where  lle_actual.msg_counter = llsh_actual.msg_counter " +
                $"and lle_before.msg_counter = llsh_before.msg_counter " +
                $"and llsh_before.SO_ID = llsh_actual.so_id " +
                $"and lle_before.op_code = 1 " +
                $"and lle_actual.op_code = 2 " +
                $"and lle_before.msg_status = -1 " +
                $"and    lle_actual.msg_counter = {tl4.msgCounter} " +
                $"and    yetinserted.msg_counter = lle_actual.msg_counter " +
                $"and	   yetinserted.counters = 0 ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            if (count > 0)
            {
                sqlstr = $"update  l4_l3_event set	op_code = 1 where msg_counter = {tl4.msgCounter}";
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    connection.Execute(sqlstr, null);
                }
                tl4.opCode = 1;
            }
            return tl4;
        }

        public void CloseLine(int iSoId, int iSoLineId, int iSoTypeCode)
        {
            throw new NotImplementedException();
        }

        public void CreateNewOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo)
        {
            throw new NotImplementedException();
        }

        public TContractType DecodeContractType(string soid, string strCustomerId)
        {
            OracleDynamicParameters odp = null;
            char flag;
            string str = "select internal_customer_flag " +
                "from customer_catalog " +
                "where  customer_descr_id = p_customer_descr_id";
            odp.Add("p_customer_descr_id", strCustomerId, OracleMappingType.Varchar2);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                flag = connection.QueryFirstOrDefault<char>(str, odp);
            }
            if (flag == 'Y' || soid.Substring(0, 3) == "OMK")
                return TContractType.coInternal;
            return TContractType.coContract;
        }

        public void LoadAttrb(TL4MsgInfoLine tL4MsgInfoLine, string strProductType, int iSoID, int iSoLineID, TCheckRelatedList attrbSO = null, List<string> strArrayOfAttributes = null)
        {
            bool bTollerances = false;
            TCheckRealtedListRepo check;
            string strDecSep, sTmpAttrUnitID, sTmpAttrCode, sTmpAttrCodeL4;
            if (attrbSO == null)
                check = new TCheckRealtedListRepo();
            else
                check = new TCheckRealtedListRepo(attrbSO);
            if (strArrayOfAttributes == null)
                strArrayOfAttributes = new List<string>();
            string sqlstr = "SELECT ATTRB_CODE AS ATTRB_CODE,CONTR_EXT_KEY_FLD_NAME,UNIT_ID " +
                "FROM ATTRB_CONTR_EXT_LINK " +
                "WHERE ATTRB_REASON = 'L4_SOM' " +
                "AND CONTR_EXT_KEY_FLD_NAME IN (" +
                "SELECT COLUMN_NAME " +
                "FROM USER_TAB_COLS " +
                "WHERE TABLE_NAME = 'L4_L3_SO_LINE')";
            List<AttrbContrExtLink> extLink;
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                extLink = connection.Query<AttrbContrExtLink>(sqlstr, null).AsList();
            }
            sqlstr = "select substr(value,1,1) as dec_separator " +
                "from   nls_session_parameters " +
                "where  PARAMETER = 'NLS_NUMERIC_CHARACTERS'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                strDecSep = connection.QueryFirstOrDefault<string>(sqlstr, null);
            }
            foreach (AttrbContrExtLink attrb in extLink)
            {
                ACEandSoLine aceandSo = new ACEandSoLine();
                sTmpAttrCode = attrb.attrbCode;
                sTmpAttrCodeL4 = attrb.contExtKeyName;
                sTmpAttrUnitID = attrb.unitId;
                sqlstr = $"SELECT '{sTmpAttrCode}' AS ATTRB_CODE, " +
                    $"ATTRIBUTE_TYPE AS ATTRB_FORMAT_CODE, " +
                    $"DECODE (TO_CHAR(attribute_type)," +
                    $"'ALPHA', DECODE( GET_AN_CONTROL_VALUE(EXT_NAME,1,attribute_value), 'NO TRANSALTION FOR ATTRIBUTE'," +
                    $", 1, attribute_value), 'NO TRANSALTION FOR ATTRIBUTE'," +
                    $"attribute_value, GET_AN_CONTROL_VALUE(EXT_NAME " +
                    $", 1, attribute_value))" +
                    $",'') AS attrb_an_value," +
                    $"NVL (TO_NUMBER (DECODE (attribute_type, 'NUMERIC', attribute_value, 0)),0) AS attrb_num_value," +
                    $"NVL (DECODE (TO_CHAR(attribute_type),'NUMERIC', DECODE(attrb_mu_code,NULL, TO_CHAR(attribute_value)," +
                    $"DECODE ({sTmpAttrUnitID},{sTmpAttrUnitID},TO_CHAR(attribute_value),mual_converter (" +
                    $"(SELECT interface_id " +
                    $" FROM l3mu_interface_detail " +
                    $" WHERE unit_id = {sTmpAttrUnitID} " +
                    $"AND ROWNUM = 1)," +
                    $"(SELECT quantity_id" +
                    $"FROM l3mu_unit " +
                    $"WHERE unit_id = {sTmpAttrUnitID}), " +
                    $"(SELECT interface_id " +
                    $"FROM l3mu_interface_detail " +
                    $"WHERE unit_id = ATTRB_MU_CODE" +
                    $"AND ROWNUM = 1), " +
                    $"TO_CHAR(attribute_value) " +
                    $"))), '0' ), '0'  ) AS attrb_num_mual_value, " +
                    $"DECODE(TO_CHAR(ATTRIBUTE_TYPE), 'FLAG', TO_CHAR(ATTRIBUTE_VALUE), 'N' )  AS ATTRB_FLAG_VALUE, " +
                    $"DECODE(TO_CHAR(ATTRIBUTE_TYPE), ''DATE'', TO_CHAR(ATTRIBUTE_VALUE), " +
                    $"TO_DATE('01011970','DDMMYYYY') ) AS ATTRB_DATE_VALUE " +
                    $"FROM   (SELECT TTL4.MSG_COUNTER, " +
                    $"TTL4.SO_LINE_ID / 10, " +
                    $"'{sTmpAttrCode}' AS ATTRIBUTE_CODE, " +
                    $"ACE.ATTRB_FORMAT_CODE AS ATTRIBUTE_TYPE, " +
                    $"ACE.ATTRB_EXTERN_NAME AS EXT_NAME, " +
                    $"ACE.ATTRB_MU_CODE, " +
                    $"NVL(ACE.ATTRB_DECIM_NUMB,2) AS ATTRB_DECIM_NUMB," +
                    $"TTL4.{sTmpAttrCodeL4} AS ATTRIBUTE_VALUE" +
                    $"from   L4_L3_SO_LINE TTL4, " +
                    $"ATTRB_CATALOGUE_ENTITY ACE " +
                    $"where '{sTmpAttrCode}'=ace.attrb_code " +
                    $"and  (TTL4.SO_LINE_ID/10)  = {tL4MsgInfoLine.iSOLineID}" +
                    $"and  TTL4.{sTmpAttrCodeL4} <> '-9999' " +
                    $"and  TTL4.MSG_COUNTER  = {tL4MsgInfoLine.tL4MsgInfo.msgCounter} )";
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    aceandSo = connection.QueryFirstOrDefault<ACEandSoLine>(sqlstr, null);
                }
                if (aceandSo != null)
                {
                    strArrayOfAttributes.Add(aceandSo.attrbCode);
                    if (aceandSo.attrbCode == "THICK_PTOL" || aceandSo.attrbCode == "THICK_NTOL")
                    {
                        bTollerances = true;
                        check.Add("THICK_TOL_CODE", "MANUAL");
                    }
                    if (aceandSo.attrbCode == "PRODUCT_QUALITY_CODE")
                    {
                        switch (Convert.ToInt32(aceandSo.attrbAnValue))
                        {
                            case 1:
                                check.Add(aceandSo.attrbCode, "PRIME");
                                break;
                            case 2:
                                check.Add(aceandSo.attrbCode, "SECONDARY");
                                break;
                        }
                    }
                    else if (aceandSo.attrbCode == "EDGE_CONDITION_CODE")
                    {
                        if (aceandSo.attrbAnValue == "О")
                            check.Add(aceandSo.attrbCode, "TRIM");
                        else if (aceandSo.attrbAnValue == "НО")
                            check.Add(aceandSo.attrbCode, "MILL");
                    }
                    else if (aceandSo.attrbCode == "PRODUCT_TYPE")
                        check.Add(aceandSo.attrbCode, strProductType);
                    else
                    {
                        //switch(AttrFor)
                        //{

                        //}
                    }
                }
            }
        }

        public bool OrderCanBeProcess(string sSoDescrID, int iOpCode, string sErrMsg)
        {
            bool bexistorder = OrderExist(sSoDescrID);
            bool res = true;
            switch (iOpCode)
            {
                case L4L3InterfaceServiceConst.OP_CODE_NEW:
                    if (bexistorder)
                    {
                        logger.Error("Заказ уже существует в БД МЕТ2000");
                        res = false;
                    }
                    break;
                case L4L3InterfaceServiceConst.OP_CODE_DEL:
                    if (!bexistorder)
                    {
                        logger.Error("Заказ не существует в БД МЕТ2000");
                        res = false;
                    }
                    break;
                case L4L3InterfaceServiceConst.OP_CODE_UPD:
                    if (!bexistorder)
                    {
                        logger.Error("Заказ не существует в БД МЕТ2000");
                        res = false;
                    }
                    break;
            }
            return res;
        }

        public bool OrderExist(string sSoDescrID)
        {
            int count;
            string sqlstr = $"SELECT COUNT(*) AS HOWMANY FROM SALES_ORDER_HEADER WHERE SO_DESCR_ID = {sSoDescrID}";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                count = connection.ExecuteScalar<int>(sqlstr, null);
            }
            return count > 0;
        }

        public TCheckResult SalesOrderMng(TL4MsgInfo l4MsgInfo)
        {
            TCheckResult checkResult = new TCheckResult();
            checkResult.isOK = false;
            L4L3SoHeaderRepo header = new L4L3SoHeaderRepo();
            L4L3SoHeader soHeader = header.GetData(l4MsgInfo);
            BlockForProcess(l4MsgInfo, true);
            string charval;
            string sqlstr = "SELECT CHAR_VALUE FROM AUX_CONSTANT WHERE CONSTANT_ID='ONE_TO_SEVERAL_ORDER_FROM_SAP'";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                charval = connection.QueryFirstOrDefault<string>(sqlstr, null);
            }
            if (charval != null)
                onetoSeveralirderFromSap = charval;
            else
                onetoSeveralirderFromSap = "N";
            if (onetoSeveralirderFromSap == "Y")
            {
                List<l4sol> l4s = new List<l4sol>();
                sqlstr = "SELECT L4SOL.SO_LINE_ID, L4SOL.SO_LINE_ID/10 as MET_SO_LINE_ID " +
                    "FROM   L4_L3_SO_LINE L4SOL WHERE  L4SOL.MSG_COUNTER = :MSG_COUNTER";
                if (m_strSo_Lines_For_Where != "")
                    sqlstr += $"AND L4SOL.SO_LINE_ID IN {m_strSo_Lines_For_Where}";
                sqlstr += "ORDER BY L4SOL.SO_LINE_ID";
                OracleDynamicParameters odp = new OracleDynamicParameters();
                odp.Add("MSG_COUNTER", l4MsgInfo.msgCounter);
                using (OracleConnection connection = BaseRepo.GetDBConnection())
                {
                    l4s = connection.Query<l4sol>(sqlstr, null).AsList();
                }
                if (l4s != null)
                {
                    foreach (l4sol kSol in l4s)
                    {
                        if (!OrderCanBeProcess(soHeader.soID, l4MsgInfo.opCode, ""))
                        {
                            l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                            l4MsgInfo.msgReport.remark = "";
                        }
                        else if (!OrderCanBeProcess($"{soHeader.soID}_{kSol.metSoLineId}", l4MsgInfo.opCode, ""))
                        {
                            l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                            l4MsgInfo.msgReport.remark = "";
                        }
                        else
                        {
                            m_strSO_Line_Id_Params = kSol.soLineId;
                            m_strSO_Line_Id_MET = kSol.metSoLineId;
                            l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS;
                            l4MsgInfo.msgReport.remark = "";

                            l4MsgInfo = CheckUpdateOPCODE(l4MsgInfo);
                            switch (l4MsgInfo.opCode)
                            {
                                case L4L3InterfaceServiceConst.OP_CODE_NEW:
                                    CreateNewOrder(soHeader, l4MsgInfo);
                                    break;
                                case L4L3InterfaceServiceConst.OP_CODE_DEL:
                                    UpdateOrder(soHeader, l4MsgInfo, true);
                                    break;
                                case L4L3InterfaceServiceConst.OP_CODE_UPD:
                                    UpdateOrder(soHeader, l4MsgInfo);
                                    break;
                                default:
                                    l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                                    l4MsgInfo.msgReport.remark = $"{l4MsgInfo.opCode} неверный код операции";
                                    break;
                            }
                        }
                    }
                }
                checkResult.isOK = true;
                return checkResult;
            }
            else
            {
                if (!OrderCanBeProcess(soHeader.soID, l4MsgInfo.opCode, ""))
                {
                    l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                    l4MsgInfo.msgReport.remark = "Заказ уже принимался по схеме с разбиением на несколько заказов.";
                    return checkResult;
                }
                CheckUpdateOPCODE(l4MsgInfo);
                switch (l4MsgInfo.opCode)
                {
                    case L4L3InterfaceServiceConst.OP_CODE_NEW:
                        CreateNewOrder(soHeader, l4MsgInfo);
                        break;
                    case L4L3InterfaceServiceConst.OP_CODE_DEL:
                        UpdateOrder(soHeader, l4MsgInfo, true);
                        break;
                    case L4L3InterfaceServiceConst.OP_CODE_UPD:
                        UpdateOrder(soHeader, l4MsgInfo);
                        break;
                    default:
                        l4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                        l4MsgInfo.msgReport.remark = $"{l4MsgInfo.opCode} неверный код операции";
                        break;
                }
                BlockForProcess(l4MsgInfo, false);
                checkResult.isOK = true;
                return checkResult;
            }
        }

        public void UpdateOrder(L4L3SoHeader QryData, TL4MsgInfo l4MsgInfo, bool bIsDeletion = false)
        {
            throw new NotImplementedException();
        }
        
    }
}
