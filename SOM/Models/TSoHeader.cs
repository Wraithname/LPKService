﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SOM.Models
{
    public class TSoHeader
    {
        [Key]
        public int m_iSoID { get; set; }
        public string m_sSoDescrID { get; set; }
        public int m_iCustSoldDescrID { get; set; }
        public string m_strCustPO { get; set; }
        public DateTime m_dCustPODate { get; set; }
        public string m_strInquirtNumber { get; set; }
        public DateTime m_dInquryNumber { get; set; }
        public string m_iInsertUser { get; set; }
        public DateTime m_dInsertDate { get; set; }
        public string m_iUpdateUser { get; set; }
        public int m_iOpCode { get; set; }
        //m_coContractType : TContractType;
        public List<TSoLine> m_Lines { get; set; }
        public int m_iLinesCount { get; set; }
        public string m_strDescription { get; set; }
    }
}
