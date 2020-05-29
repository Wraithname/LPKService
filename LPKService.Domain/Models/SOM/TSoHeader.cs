using System;
using System.Collections.Generic;

namespace LPKService.Domain.Models.SOM
{
    public class TSoHeader
    {
        public int m_iSoID { get; set; }
        public string m_sSoDescrID { get; set; }
        public int m_iCustSoldDescrID { get; set; }
        public string m_strCustPO { get; set; }
        public DateTime m_dCustPODate { get; set; }
        public string m_strInquirtNumber { get; set; }
        public DateTime m_dInquiryDate { get; set; }
        public string m_iInsertUser { get; set; }
        public DateTime m_dInsertDate { get; set; }
        public string m_iUpdateUser { get; set; }
        public int m_iOpCode { get; set; }
        public enum ContractType { coInternal, coContract }
        public List<TSoLine> m_Lines { get; set; }
        public int m_iLinesCount { get; set; }
        public string m_strDescription { get; set; }
    }
}
