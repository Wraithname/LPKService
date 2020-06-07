using Repository;

namespace LPKService.Domain.Models.SOM
{
    /// <summary>
    /// Модель таблицы ATTRB_CONTR_EXT_LINK
    /// </summary>
    public class AttrbContrExtLink:BaseModel
    {
        [Column("ATTRB_CODE")]
        public string attrbCode { get; set; }
        [Column("CONTR_EXT_KEY_FLD_NAME")]
        public string contExtKeyName { get; set; }
        [Column("UNIT_ID")]
        public string unitId { get; set; }
    }
}
