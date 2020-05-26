using LPKService.Domain.Models;


namespace LPKService.Domain.Interfaces
{
    public interface ITSoLine
    {
        TSoLine Create(LinesCom line,TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate);
        void UpdateMsgStatus();
        string GetMsgStatus();
        int CheckValueType(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        int CheckValueCredit(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        int CheckValueProductType(bool bisValid, TL4MsgInfoLine l4MsgInfo);
        TLineStatus GetLineStatus(int linestatus);
    }
    public enum TLineStatus { IsClosed, IsOpened }
}
