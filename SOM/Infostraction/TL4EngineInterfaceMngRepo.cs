using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Models;
using SOM.Repo;
using Work.Models;

namespace SOM.Infostraction
{
    class TL4EngineInterfaceMngRepo : ITL4EngineInterfaceMng
    {
        public object CheckValue(string strTableNameOrigin, string strFieldNameOrigin, string strAliasFieldNameOrigin, bool bIsValidValue, TL4MsgInfo l4MsgInfo, string strTableNameDec, string strFieldNameDec, string strWhereOptional = "")
        {
            throw new NotImplementedException();
        }

        public object CheckValue(string strTableNameOrigin, string strFieldNameOrigin, string strAliasFieldNameOrigin, bool bIsValidValue, TL4MsgInfoLine l4MsgInfoLine, string strTableNameDec, string strFieldNameDec, string strWhereOptional)
        {
            throw new NotImplementedException();
        }

        public TL4EngineInterfaceMng Create(TL4MsgInfo pL4MsgInfoPtr)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public DateTime GetModDateitme()
        {
            throw new NotImplementedException();
        }

        public string GetModUserId()
        {
            throw new NotImplementedException();
        }

        public int GetMsgCounter()
        {
            throw new NotImplementedException();
        }

        public bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true)
        {
            throw new NotImplementedException();
        }

        public bool NotifyErrorMessage(string Text, string pErrorCode)
        {
            throw new NotImplementedException();
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
