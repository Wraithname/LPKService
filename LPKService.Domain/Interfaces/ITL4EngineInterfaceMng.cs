using System;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса TL4EngineInterfaceMng
    /// </summary>
    public interface ITL4EngineInterfaceMng
    {
        string DecodeUserToUserId(string pL4UserName);
        bool NotifyErrorMessage(string Text, string Caption = "", bool pFatal = true);
        bool NotifyErrorMessage(string Text, string pErrorCode);
        string GetCreateUserId();
        string GetModUserId();
        DateTime GetCreateDate();
        DateTime GetModDateitme();
        int GetMsgCounter();
    }
}
