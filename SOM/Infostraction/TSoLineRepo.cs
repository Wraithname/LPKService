using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using SOM.Models;
using Work.Models;

namespace SOM.Repo
{
    class TSoLineRepo:TSoLine
    {
        //Создание и заполнение таблицы
        public TSoLine Create(DataTable tbl, TL4MsgInfo l4MsgInfo, int iCustomerID, int iShipToCode, bool lbIsUpdate)
        {
            //bool bVerifyData = true;
            TL4MsgInfoLine m_L4MsgInfoLine = new TL4MsgInfoLine();
            TL4MsgInfo tL4MsgInfo = new TL4MsgInfo();
            tL4MsgInfo.msgCounter = Convert.ToInt32(tbl.Rows[0].ToString());
            tL4MsgInfo.msgReport.status = Convert.ToInt32(tbl.Rows[1].ToString());
            tL4MsgInfo.msgReport.remark = "";
            m_iSoLineID= Convert.ToInt32(tbl.Rows[3].ToString());
            return null;
        }
        
    }
}
