using LPKService.Domain.Models.Work;
using LPKService.Domain.Models.SOM;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса TSoLineRepo
    /// </summary>
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
    /// <summary>
    /// Статус заказа
    /// </summary>
    public enum TLineStatus { IsClosed, IsOpened }
}
