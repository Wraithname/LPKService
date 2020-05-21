using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using SOM.Models;
using Repository;
using Repository.WorkModels;
using SOM.Repo;
using Logger;
using SOM;

namespace SOM.Infostraction
{
    class TSoHeaderRepo: ITSoHeader
    {
        private Log logger = LogFactory.GetLogger(nameof(L4L3SoHeader));
        public char oneToSeveralOrderFromSap='Y';
        public string m_strSO_Line_Id_MET = "";
        public string m_strSO_Line_Id_Params = "";
        SOManagment som = new SOManagment();
        TSoHeader soHeader ;
        TSoLine templine;
        TSoLineRepo lineRepo = new TSoLineRepo();
        public TSoHeaderRepo()
        {
            this.soHeader = new TSoHeader();
            this.templine = new TSoLine();
        }
        public TSoLine GetLine(int iIndex)
        {
            TSoLine line = new TSoLine();
            line = soHeader.m_Lines[iIndex];
            return line;
        }
        public string GetCustIDFromL4CustID(int l4CustID, OracleDynamicParameters odp = null)
        {
            string cust;
            StringBuilder stm = new StringBuilder(@"SELECT CUSTOMER_DESCR_ID FROM CUSTOMER_CATALOG WHERE CUSTOMER_ID =" + l4CustID + "");
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                cust = conn.QueryFirstOrDefault<string>(stm.ToString(), odp);
            }
            if (cust!=null)
                return cust;
            else
                return (-1).ToString();
        }
        public bool ExistCustomer(string m_iCustSoldDescrID, OracleDynamicParameters odp = null)
        {
            string cust;
            StringBuilder stm = new StringBuilder(@"SELECT EXPIRATION_DATE FROM CUSTOMER_CATALOG WHERE  CUSTOMER_DESCR_ID = " + m_iCustSoldDescrID + "");
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                cust = conn.QueryFirstOrDefault<string>(stm.ToString(), odp);
            }
            if (cust != null)
            {
                if (Convert.ToDateTime(cust) <= DateTime.Now)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public int CheckValue(bool bisValid, TL4MsgInfo l4MsgInfo)
        {
            bisValid = false;
            int res = 0;
            List<int> values = new List<int>();
            string str = "SELECT CUSTOMER_ID AS CUSTOMER_ID" +
                "FROM L4_L3_SO_HEADER " +
                "WHERE MSG_COUNTER=:MSG_COUNTER";
            OracleDynamicParameters odp = new OracleDynamicParameters();
            odp.Add("MSG_COUNTER", l4MsgInfo.msgCounter);
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                values = connection.Query<int>(str, odp).AsList();
            }
            if (values.Count > 0)
            {
                foreach (int val in values)
                {
                    if (val != 0)
                    {
                        int count = 0;
                        str = "SELECT COUNT(*) AS COUNTER " +
                            "FROM CUSTOMER_CATALOG " +
                            "WHERE CUSTOMER_DESCR_ID=:CUSTOMER_DESCR_ID";
                        odp = new OracleDynamicParameters();
                        odp.Add("CUSTOMER_DESCR_ID", val);
                        using (OracleConnection connection = BaseRepo.GetDBConnection())
                        {
                            count = connection.QueryFirstOrDefault<int>(str, odp);
                        }
                        if (count > 0)
                        {
                            bisValid = true;
                            res = val;
                        }
                        else
                        {
                            bisValid = false;
                            break;
                        }
                    }
                }
            }
            if (!bisValid)
                logger.Error($"Value L4_L3_SO_HEADER.CUSTOMER_ID is not valid");
            return res;
        }
        public void Add(TSoLine lines)
        {
            soHeader.m_iLinesCount++;
            soHeader.m_Lines.Add(lines);
        }
        public TSoHeader Create(L4L3SoHeader l4l3soheader,TL4MsgInfo l4MsgInfo, TL4EngineInterfaceMng eimOrderEntry, int iShipToCode, bool bVerifyData, bool bisUpdate = false)
        {
            //string strProva;
            TL4EngineInterfaceMngRepo mngRepo = new TL4EngineInterfaceMngRepo(l4MsgInfo);
            bool bIsValid=true;
            TDeleteResponse deleteResponse=new TDeleteResponse();
            try
            {
                soHeader.m_iLinesCount = 0;
                if (bIsValid)
                    soHeader.m_iCustSoldDescrID = CheckValue(bIsValid, l4MsgInfo);
                if(!ExistCustomer(soHeader.m_iCustSoldDescrID.ToString()))
                {
                    mngRepo.NotifyErrorMessage($"Заказчик САП {l4l3soheader.customerId} неверен или срок валидности закончился");
                    bIsValid = false; 
                }
                if(bIsValid)
                {
                    if (oneToSeveralOrderFromSap == 'Y')
                        soHeader.m_sSoDescrID = l4l3soheader.soID + "_" + m_strSO_Line_Id_MET;
                    else
                        soHeader.m_sSoDescrID = l4l3soheader.soID;
                
                if (l4l3soheader.customerPo.Length > 0)
                    soHeader.m_strCustPO = l4l3soheader.customerPo;
                else
                {
                    mngRepo.NotifyErrorMessage("'CUSTOMER_PO' поле не может быть пустым");
                    bIsValid = false;
                }
                if (l4l3soheader.customerPoDate.ToString().Length > 0)
                    soHeader.m_dCustPODate = l4l3soheader.customerPoDate;
                else
                    soHeader.m_dCustPODate = l4l3soheader.insertDate;
                if (l4l3soheader.inquiryRefNumber.Length > 0)
                    soHeader.m_strInquirtNumber = l4l3soheader.inquiryRefNumber;
                else
                    soHeader.m_strInquirtNumber = soHeader.m_strCustPO;
                if (l4l3soheader.inquiryRefDate.ToString().Length > 0)
                    soHeader.m_dInquiryDate = l4l3soheader.inquiryRefDate;
                else
                    soHeader.m_dInquiryDate = soHeader.m_dCustPODate;
                }
                if(bIsValid)
                {
                    soHeader.m_iInsertUser = mngRepo.GetCreateUserId();
                    if (soHeader.m_iInsertUser == "")
                        bIsValid = false;
                }
                if (bIsValid)
                    if (l4l3soheader.insertDate.ToString().Length > 0)
                        soHeader.m_dInsertDate = l4l3soheader.insertDate;
                if(bIsValid)
                {
                    soHeader.m_iUpdateUser = mngRepo.GetModUserId();
                    if (soHeader.m_iUpdateUser == "")
                        bIsValid = false;
                }
                if (bIsValid)
                    if (l4MsgInfo.opCode > 0)
                        soHeader.m_iOpCode = l4MsgInfo.opCode;
                if (bIsValid)
                {
                    switch (som.DecodeContractType(soHeader.m_sSoDescrID, l4l3soheader.customerId.ToString()))
                    {
                        case TContractType.coInternal:
                            if(som.IsCustomerInternal(soHeader.m_iCustSoldDescrID))
                            {
                                mngRepo.NotifyErrorMessage("Заказ обозначен как внутренний, но имеет внешнего заказчика");
                                bIsValid = false;
                            }
                            break;
                    }
                }
                if(bIsValid)
                {
                    List<LinesCom> lines = new List<LinesCom>();
                    string str = "SELECT L4SOL.* " +
                        "L4EVE.MSG_STATUS " +
                        "L4EVE.MSG_REMARK, " +
                        "L4SOL.SO_LINE_ID/10 AS SO_LINE_MET, " +
                        "PTC.PRODUCT_TYPE AS PROD_MET, " +
                        "L4EVE.OP_CODE, " +
                        "L4SOL.LINE_NOTE " +
                        "FROM   L4_L3_SO_LINE L4SOL, " +
                        "PRODUCT_TYPE_CATALOGUE PTC " +
                        "L4_L3_EVENT L4EVE " +
                        $"WHERE  L4SOL.MSG_COUNTER = :{l4MsgInfo.msgCounter}" +
                        "AND  L4SOL.MSG_COUNTER = L4EVE.MSG_COUNTER " +
                        "AND  ptc.SAP_CODE = l4sol.PRODUCT_TYPE";
                    if (oneToSeveralOrderFromSap == 'Y')
                        str += $"AND  L4SOL.SO_LINE_ID = '{m_strSO_Line_Id_Params}'";
                    str += "ORDER BY L4SOL.SO_LINE_ID ";
                    using (OracleConnection connection = BaseRepo.GetDBConnection())
                    {
                        lines = connection.Query<LinesCom>(str, null).AsList();
                    }
                    foreach(LinesCom line in lines)
                    {
                        templine = lineRepo.Create(line, l4MsgInfo, soHeader.m_iCustSoldDescrID, iShipToCode, bisUpdate);
                        templine.m_strDescription = line.lineNote;
                        if(!bVerifyData)
                        {
                            mngRepo.NotifyErrorMessage($"Ошибка в строке {templine.m_iSoLineID} {templine.m_L4MsgInfoLine.tL4MsgInfo.msgReport.remark}");
                            templine.m_L4MsgInfoLine.tL4MsgInfo.msgReport.status = L4L3InterfaceServiceConst.MSG_STATUS_ERROR;
                            Add(templine);
                            break;
                        }
                        Add(templine);
                    }
                }    
            }
            catch {  }
            return soHeader;
        }
        public string GetLineMsgStatus(TSoHeader ret)
        {
            TSoLineRepo func = new TSoLineRepo();
            string strMessages="",strLineMessage;
            foreach(TSoLine line in ret.m_Lines)
            {
                strLineMessage = func.GetMsgStatus(line.m_L4MsgInfoLine);
                if (strLineMessage.ToCharArray().Length > 0 && (strMessages.ToCharArray().Length + strLineMessage.ToCharArray().Length + 3) > 4000)
                    strMessages = strMessages + " - " + strLineMessage;
            }
            return strMessages;
        }
        public void LinesUpdateMsgStatus(TSoHeader ret)
        {
            TSoLineRepo func = new TSoLineRepo();
            foreach(TSoLine line in ret.m_Lines)
            {
                func.UpdateMsgStatus(line.m_L4MsgInfoLine.tL4MsgInfo);
            }
        }
        public bool CheckLineNumeration(int iCounter,OracleDynamicParameters odp=null)
        {
            string solineid;
            string stm = @"SELECT SO_LINE_ID / 10 AS SO_LINE_ID FROM L4_L3_SO_LINE WHERE  MSG_COUNTER = "+iCounter.ToString()+ " ORDER BY SO_LINE_ID ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                solineid=connection.QueryFirstOrDefault<string>(stm, odp);
            }
            int i = 1;
            while (true)
            {
                if (i != Convert.ToInt32(solineid))
                    return false;
                i++;
            }
        }
    }
}
