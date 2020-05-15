using System;
using Repository;

namespace Shipping.Models
{
    public class L4L3RmAndMatCat
    {
        [Column("MATERIAL_ID", "ИД материала")]
        public string materialId { get; set; }
        [Column("MATERIAL_CODE", "Код материала")]
        public string materialCode { get; set; }
        [Column("MATERIAL_NAME", "Название материала")]
        public string materialName { get; set; }
        [Column("ACTUAL_QTY", "Актуальное количество")]
        public float actualqty { get; set; }
        [Column("MATERIAL_AMOUNT", "Количество материала")]
        public float materialAmount { get; set; }
        [Column("MOVMENT_QTY", "Перемещаемое количество")]
        public float movmetnqty { get; set; }
        [Column("MOVEMENT_DATETIME", "Дата перемещения")]
        public DateTime movementdatetime { get; set; }
    }
}
