using System;
using Repository;
namespace SOM.Models
{
    public class TSoLine
    {
        public int m_iSoID { get; set; }
        public string m_sSoDescrID { get; set; }
        public int m_iSoLineID { get; set; }
        public int m_iCustShiptoDescrID { get; set; }
        public int m_iShipToCode { get; set; }
        public int m_iSoTypeCode { get; set; }
        public int m_iOpCode { get; set; }
        public int m_iCreditStatus { get; set; }
        public DateTime m_dDueDelivery { get; set; }
        public int m_iDeliveryPeriodNumID { get; set; }
        public string m_strProductCode { get; set; }
        public string m_strDescription { get; set; }
        public string m_iInsertUser { get; set; }
        public DateTime m_dInsertDate { get; set; }
        public string m_iUpdateUser { get; set; }
        public DateTime m_dUpdateDate { get; set; }
        public int m_iOrderLineStatus { get; set; }
        public TL4MsgInfoLine m_L4MsgInfoLine { get; set; }
        //m_eimOELineInterface: TL4EngineInterfaceMng;

    }
}
