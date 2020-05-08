using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using SOM.Models;
using SOM.Infostraction;
using Work.Models;
using Work;

namespace SOM.Repo
{
    class TSoLineRepo : ITSoLine
    {
        public TSoLine Create(TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate)
        {
            TSoLine newData = new TSoLine();
            L4L3InterfaceUtility inter = new L4L3InterfaceUtility();
            bool bVerifyData = true;
            TL4MsgInfoLine m_L4MsgInfoLine = new TL4MsgInfoLine();
            //код присвоения результата запроса
            m_L4MsgInfoLine.tL4MsgInfo.msgCounter=0;
            m_L4MsgInfoLine.tL4MsgInfo.msgReport.status = 0;
            m_L4MsgInfoLine.tL4MsgInfo.msgReport.remark = "";
            newData.m_iSoLineID = 0;
            TL4EngineInterfaceMng m_eimOELineInterface = new TL4EngineInterfaceMng(m_L4MsgInfoLine.tL4MsgInfo);
            newData.m_sSoDescrID = "0";
            if (lbIsUpdate)
                newData.m_iSoID = inter.GetSoIdFromDescr(newData.m_sSoDescrID);
            m_L4MsgInfoLine.sSoDescrIDInfoLine = newData.m_sSoDescrID;
            newData.m_L4MsgInfoLine = m_L4MsgInfoLine;
            //Дописать функции после оформления главных функций SOManagment
            //DecodeContractType
            newData.m_iShipToCode = iShipToCode;
            if (bVerifyData)
                //newData.m_iSoTypeCode=m_eimOELineInterface.CheckValue
                bVerifyData = true;
            if (newData.m_iSoTypeCode == 2)
                newData.m_iSoTypeCode = 4;
            if (bVerifyData)
                //newData.m_iCreditStatus=m_eimOELineInterface.CheckValue
                bVerifyData = true;
            if (bVerifyData)
            {
                newData.m_iOrderLineStatus = -1;

            }
            return newData;
        }

        public string GetMsgStatus(TL4MsgInfoLine l4MsgInfo)
        {
            return l4MsgInfo.tL4MsgInfo.msgReport.remark;
        }
        public void UpdateMsgStatus(TL4MsgInfo l4MsgInfo)
        {
            TL4MsgInfoLineRepo msgInfoLine = new TL4MsgInfoLineRepo();
            msgInfoLine.UpdateMsgStatus(l4MsgInfo);
        }
    }
}
