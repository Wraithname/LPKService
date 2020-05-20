using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;
using Repository.Models;

namespace SOM.Infostraction
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
        public TL4EngineInterfaceMngRepo(L4L3Customer customer, TL4MsgInfo pL4MsgInfoPtr)
        {
            this.tL4Engine = new TL4EngineInterfaceMng();
            this.l4MsgInfo = new TL4MsgInfo();
            tL4Engine.m_QryData = customer;
            tL4Engine.m_L4MsgInfoPtr = pL4MsgInfoPtr;
        }
        //Узнать про функцию
        public string DecodeUserToUserId(string pL4UserName)
        {
            string res="";
            if (pL4UserName == "")
            {
                NotifyErrorMessage("TL4EngineInterfaceMng.DecodeUserNameToUserId: pL4UserName = ''''.");
                return res;
            }
            else
            {
                try
                {
                    return res;
                }
                catch { return res; }
            }
        }

        public DateTime GetCreateDate()
        {
            throw new NotImplementedException();
        }
        //Узнать про функцию (Точнее какой набор данных хранит в себе TL4EngineInterfaceMng)
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

        public bool NotifyErrorTree(bool pFatal)
        {
            throw new NotImplementedException();
        }

        public bool NotifySubErrorMessage(string Text, bool pFatal)
        {
            throw new NotImplementedException();
        }
    }
}
