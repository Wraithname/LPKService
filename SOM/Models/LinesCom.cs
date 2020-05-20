﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace SOM.Models
{
    public class LinesCom
    {
        public int keyMsgCounter { get; set; }
        public int msgCounter { get; set; }
        public string soLineId { get; set; }
        public string soId { get; set; }
        public int soTypeCode { get; set; }
        public int soLineStatus { get; set; }
        public int soLineCreditStatus { get; set; }
        public DateTime dueDeliveryDate { get; set; }
        public string productType { get; set; }
        public int soQualityCode { get; set; }
        public float soQty { get; set; }
        public float soQtyMax { get; set; }
        public float soQtyMin { get; set; }
        public string grade { get; set; }
        public string chemNorm { get; set; }
        public string productDimensionNorm { get; set; }
        public string productMechanicalNorm { get; set; }
        public float thick { get; set; }
        public float width { get; set; }
        public float thickNtol { get; set; }
        public float thickPtol { get; set; }
        public float widthNtol { get; set; }
        public float widthPtol { get; set; }
        public float diameterExternalMax { get; set; }
        public float intervalDiameter { get; set; }
        public float pieceWeight { get; set; }
        public float pieceMinWeight { get; set; }
        public float pieceMaxWeight { get; set; }
        public float cMin { get; set; }
        public float mnMin { get; set; }
        public float siMin { get; set; }
        public float sMin { get; set; }
        public float pMin { get; set; }
        public float crMin { get; set; }
        public float niMin { get; set; }
        public float cuMin { get; set; }
        public float moMin { get; set; }
        public float n2Min { get; set; }
        public float asMin { get; set; }
        public float tiMin { get; set; }
        public float alMin { get; set; }
        public float vMin { get; set; }
        public float ndMin { get; set; }
        public float bMin { get; set; }
        public float cMax { get; set; }
        public float mnMax { get; set; }
        public float siMax { get; set; }
        public float sMax { get; set; }
        public float pMax { get; set; }
        public float crMax { get; set; }
        public float niMax { get; set; }
        public float cuMax { get; set; }
        public float moMax { get; set; }
        public float n2Max { get; set; }
        public float asMax { get; set; }
        public float tiMax { get; set; }
        public float alMax { get; set; }
        public float vMax { get; set; }
        public float ndMax { get; set; }
        public float bMax { get; set; }
        public float ceqMin { get; set; }
        public float ceqMax { get; set; }
        public float pcmMin { get; set; }
        public float pcmMax { get; set; }
        public string soIdErp { get; set; }
        public string soLineIdErp { get; set; }
        public string strengthClass { get; set; }
        public float length { get; set; }
        public float lengthPtol { get; set; }
        public float lengthNtol { get; set; }
        public float packMaxWeight { get; set; }
        public float packMinWeight { get; set; }
        public float slitWidth { get; set; }
        public float slitWidthPtol { get; set; }
        public float slitWidthNtol { get; set; }
        public string edgeConitionCode { get; set; }
        public int orderStatus { get; set; }
        public string matnr { get; set; }
        public string matnrSlab { get; set; }
        public string matnrSteel { get; set; }
        public string matnrHrc { get; set; }
        public string dUdvyas { get; set; }
        public string lineNote { get; set; }
        public string additionalParamsData { get; set; }
        public string countryDestination { get; set; }
        public char useVd { get; set; }
        public string okpo { get; set; }
        public string rwCustomerCode { get; set; }
        public string gradeCategory { get; set; }
        public string gradeInternal { get; set; }
        public string headerNote { get; set; }
        public string type { get; set; }
        public int msgStatus { get; set; }
        public string msgRemark { get; set; }
        public int soLineMet { get; set; }
        public string prod_met { get; set; }
        public int opCode { get; set; }
        public string lineNoter { get; set; }
    }
}
