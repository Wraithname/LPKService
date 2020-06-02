using System;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.Work;

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
        public TL4EngineInterfaceMngRepo(TL4MsgInfo l4MsgInfo)
        {
            this.l4MsgInfo = l4MsgInfo;
        }
        public TL4EngineInterfaceMngRepo(object customer, TL4MsgInfo pL4MsgInfoPtr)
        {
            this.tL4Engine = new TL4EngineInterfaceMng();
            this.l4MsgInfo = new TL4MsgInfo();
            tL4Engine.m_QryData = customer;
            tL4Engine.m_L4MsgInfoPtr = pL4MsgInfoPtr;
        }
        public string DecodeUserToUserId(string pL4UserName)
        {
            throw new NotImplementedException();
        }
        public DateTime GetCreateDate()
        {
            throw new NotImplementedException();
        }
        public string GetCreateUserId()
        {
            return DecodeUserToUserId("BCKPROC");
        }

        public DateTime GetModDateitme()
        {
            throw new NotImplementedException();
        }

        public string GetModUserId()
        {
            return GetCreateUserId();
        }

        public int GetMsgCounter()
        {
            return l4MsgInfo.msgCounter;
        }

        public bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true)
        {
            if (pFatal)
                l4MsgInfo.msgReport.status = -1;
            l4MsgInfo.msgReport.remark = "";
            l4MsgInfo.msgReport.remark = ((l4MsgInfo.msgReport.remark)+Caption+Text).Substring(0,msgRemarkMaxLen);
            return !pFatal;
        }

        public bool NotifyErrorMessage(string text, string pErrorCode)
        {
            return NotifyErrorMessage(text, "", true);
        }
    }
}
