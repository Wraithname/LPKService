using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Models;

namespace SOM.Models
{ 
    public class TL4EngineInterfaceMng
    {
        private TL4MsgInfo m_L4MsgInfoPtr { get; set; }
        private DataSet m_QryData { get; set; }

        public TL4EngineInterfaceMng(TL4MsgInfo m_L4MsgInfoPtr)
        {
            this.m_L4MsgInfoPtr = m_L4MsgInfoPtr;
        }
    }
}
