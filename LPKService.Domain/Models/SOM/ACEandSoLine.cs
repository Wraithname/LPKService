using Repository;

namespace LPKService.Domain.Models.SOM
{
    /// <summary>
    /// Модель таблицы ATTRB_CATALOGUE_ENTITY union SoLine
    /// </summary>
    public class ACEandSoLine:BaseModel
    {
        [Column("ATTRB_CODE")]
        public string attrbCode { get; set; }
        [Column("ATTRB_FORMAT_CODE")]
        public string attrbFormatCode { get; set; }
        [Column("attrb_an_value")]
        public string attrbAnValue { get; set; }
        [Column("attrb_num_value")]
        public string attrbNumValue { get; set; }
        [Column("attrb_num_mual_value")]
        public string attrbNumMualValue { get; set; }
        [Column("ATTRB_FLAG_VALUE")]
        public string attrbFlagValue { get; set; }
        [Column("ATTRB_DATE_VALUE")]
        public string attrbDateValue { get; set; }
        [Column("MSG_COUNTER")]
        public int msgCounter { get; set; }
        [Column("SO_LINE_ID")]
        public string soLineId { get; set; }
        [Column("ATTRIBUTE_CODE")]
        public string attributeCode { get; set; }
        [Column("ATTRIBUTE_TYPE")]
        public string attributeType { get; set; }
        [Column("EXT_NAME")]
        public string extName { get; set; }
        [Column("ATTRB_MU_CODE")]
        public string attrbMuCode { get; set; }
        [Column("ATTRB_DECIM_NUMB")]
        public string attrbDecimNumb { get; set; }
        [Column("ATTRIBUTE_VALUE")]
        public string attributeValue { get; set; }
    }
}
