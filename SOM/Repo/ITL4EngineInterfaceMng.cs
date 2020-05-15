using System;
using SOM.Models;
using Work.Models;

namespace SOM.Repo
{
    interface ITL4EngineInterfaceMng
    {
        string DecodeUserToUserId(string pL4UserName);
        TL4EngineInterfaceMng Create(L4L3Customer customer, TL4MsgInfo pL4MsgInfoPtr);
        bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true);
        bool NotifyErrorMessage(string Text, string pErrorCode);
        bool NotifyErrorTree(bool pFatal);
        bool NotifySubErrorMessage(string Text, bool pFatal);
        string GetCreateUserId();
        string GetModUserId();
        DateTime GetCreateDate();
        DateTime GetModDateitme();
        object CheckValue(string strTableNameOrigin, string strFieldNameOrigin, string strAliasFieldNameOrigin,
            bool bIsValidValue, TL4MsgInfo l4MsgInfo, string strTableNameDec, string strFieldNameDec,
            string strWhereOptional = "");
        object CheckValue(string strTableNameOrigin, string strFieldNameOrigin, string strAliasFieldNameOrigin,
            bool bIsValidValue, TL4MsgInfoLine l4MsgInfoLine, string strTableNameDec, string strFieldNameDec,
            string strWhereOptional);
        int GetMsgCounter();
    }
}
