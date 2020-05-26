using System;

namespace LPKService.Domain.Interfaces
{
    public interface ITL4EngineInterfaceMng
    {
        string DecodeUserToUserId(string pL4UserName);
        bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true);
        bool NotifyErrorMessage(string Text, string pErrorCode);
        bool NotifyErrorTree(bool pFatal);
        bool NotifySubErrorMessage(string Text, bool pFatal);
        string GetCreateUserId();
        string GetModUserId();
        DateTime GetCreateDate();
        DateTime GetModDateitme();
        int GetMsgCounter();
    }
}
