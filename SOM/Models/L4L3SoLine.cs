using System;
using Repository;

namespace SOM.Models
{
    public class L4L3SoLine
    {
        [Column("KEY_MSG_COUNTER", "Счетчик сообщений")]
        public int keyMsgCounter { get; set; }
        [Column("MSG_COUNTER", "Счетчик сообщений")]
        public int msgCounter { get; set; }
        [Column("SO_LINE_ID", "ИД линии заказа")]
        public string soLineId { get; set; }
        [Column("SO_ID", "ИД заказа")]
        public string soId { get; set; }
        [Column("SO_TYPE_CODE", "Код типа коммерческого заказа")]
        public int soTypeCode { get; set; }
        [Column("SO_LINE_STATUS", "Статус строки заказа")]
        public int soLineStatus{ get; set; }
        [Column("SO_LINE_CREDIT_STATUS", "Статус оплаты строки заказа")]
        public int soLineCreditStatus { get; set; }
        [Column("DUE_DELIVERY_DATE", "Конечная дата поставки заказа")]
        public DateTime dueDeliveryDate { get; set; }
        [Column("PRODUCT_TYPE", "Тип продукта")]
        public string productType { get; set; }
        [Column("SO_QUALITY_CODE", "Уровень качества")]
        public int soQualityCode { get; set; }
        [Column("SO_QTY", "Заказное кол-во")]
        public float soQty{ get; set; }
        [Column("SO_QTY_MAX", "Макс. заказное кол-во")]
        public float soQtyMax{ get; set; }
        [Column("SO_QTY_MIN", "Мин. заказное кол-во")]
        public float soQtyMin { get; set; }
        [Column("GRADE", "Марка стали")]
        public string grade { get; set; }
        [Column("CHEM_NORM", "НТД химии")]
        public string chemNorm { get; set; }
        [Column("PRODUCT_DIMENSION_NORM", "НТД геометрии")]
        public string productDimensionNorm { get; set; }
        [Column("PRODUCT_MECHANICAL_NORM", "НТД")]
        public string productMechanicalNorm { get; set; }
        [Column("THICK", "Толщина")]
        public float thick { get; set; }
        [Column("WIDTH", "Ширина")]
        public float width { get; set; }
        [Column("THICK_NTOL", "Допуск толщины отрицательный")]
        public float thickNtol { get; set; }
        [Column("THICK_PTOL", "Допуск толщины положительный")]
        public float thickPtol { get; set; }
        [Column("WIDTH_NTOL", "Допуск ширины отрицательный")]
        public float widthNtol { get; set; }
        [Column("WIDTH_PTOL", "Допуск ширины положительный")]
        public float widthPtol { get; set; }
        [Column("DIAMETER_EXTERNAL_MAX", "Максимальный внешний диаметр")]
        public float diameterExternalMax { get; set; }
        [Column("INTERVAL_DIAMETER", "Внутренний диаметр")]
        public float intervalDiameter { get; set; }
        [Column("PIECE_WEIGHT", "Вес рулона")]
        public float pieceWeight { get; set; }
        [Column("PIECE_MIN_WEIGHT", "Минимальный вес рулона")]
        public float pieceMinWeight { get; set; }
        [Column("PIECE_MAX_WEIGHT", "Максимальный вес рулона")]
        public float pieceMaxWeight { get; set; }
        [Column("C_MIN", "Мин.знач. C")]
        public float cMin { get; set; }
        [Column("MN_MIN", "Мин.знач. Mn")]
        public float mnMin { get; set; }
        [Column("SI_MIN", "Мин.знач. Si")]
        public float siMin { get; set; }
        [Column("S_MIN", "Мин.знач. S")]
        public float sMin { get; set; }
        [Column("P_PIN", "Мин.знач. P")]
        public float pMin { get; set; }
        [Column("CR_MIN", "Мин.знач. Cr")]
        public float crMin { get; set; }
        [Column("NI_MIN", "Мин.знач. Ni")]
        public float niMin { get; set; }
        [Column("CU_MIN", "Мин.знач. Cu")]
        public float cuMin { get; set; }
        [Column("MO_MIN", "Мин.знач. Mo")]
        public float moMin { get; set; }
        [Column("N2_MIN", "Мин.знач. N")]
        public float n2Min { get; set; }
        [Column("AS_MIN", "Мин.знач. As")]
        public float asMin { get; set; }
        [Column("TI_MIN", "Мин.знач. Ti")]
        public float tiMin { get; set; }
        [Column("AL_MIN", "Мин.знач. Al")]
        public float alMin { get; set; }
        [Column("V_MIN", "Мин.знач. V")]
        public float vMin { get; set; }
        [Column("ND_MIN", "Мин.знач. Nb")]
        public float ndMin { get; set; }
        [Column("B_MIN", "Мин.знач. B")]
        public float bMin { get; set; }
        [Column("C_MAX", "Макс.знач. C")]
        public float cMax { get; set; }
        [Column("MN_MAX", "Макс.знач. Mn")]
        public float mnMax { get; set; }
        [Column("SI_MAX", "Макс. знач. Si")]
        public float siMax { get; set; }
        [Column("S_MAX", "Макс.знач. S")]
        public float sMax { get; set; }
        [Column("P_MAX", "Макс.знач. P")]
        public float pMax { get; set; }
        [Column("CR_MAX", "Макс.знач. Cr")]
        public float crMax { get; set; }
        [Column("NI_MAX", "Макс.знач. Ni")]
        public float niMax { get; set; }
        [Column("CU_MAX", "Макс.знач. Cu")]
        public float cuMax { get; set; }
        [Column("MO_MAX", "Макс.знач. Mo")]
        public float moMax { get; set; }
        [Column("N2_MAX", "Макс.знач. N")]
        public float n2Max { get; set; }
        [Column("AS_MAX", "Макс.знач. As")]
        public float asMax { get; set; }
        [Column("TI_MAX", "Макс.знач. Ti")]
        public float tiMax { get; set; }
        [Column("AL_MAX", "Макс.знач. Al")]
        public float alMax { get; set; }
        [Column("V_MAX", "Макс.знач. V")]
        public float vMax { get; set; }
        [Column("ND_MAX", "Макс.знач. Nb")]
        public float ndMax { get; set; }
        [Column("B_MAX", "Макс.знач. B")]
        public float bMax { get; set; }
        [Column("CEQ_MIN", "Мин.знач. углеродного эквивалента")]
        public float ceqMin { get; set; }
        [Column("CEQ_MAX", "Макс.знач. углеродного эквивалента")]
        public float ceqMax { get; set; }
        [Column("PCM_MIN", "Мин. коэф. растрескивания")]
        public float pcmMin { get; set; }
        [Column("PCM_MAX", "Макс. коэф. растрескивания")]
        public float pcmMax { get; set; }
        [Column("SO_ID_ERP", "ID заказа из ERP")]
        public string soIdErp { get; set; }
        [Column("SO_LINE_ID_ERP", "ID линии заказа из ERP")]
        public string soLineIdErp { get; set; }
        [Column("STRENGTH_CLASS", "Класс прочности")]
        public string strengthClass { get; set; }
        [Column("LENGTH", "Длина")]
        public float length { get; set; }
        [Column("LENGHT_PTOL", "Длина положительный допуск")]
        public float lengthPtol { get; set; }
        [Column("LENGTH_NTOL", "Длина отрицательный допуск")]
        public float lengthNtol { get; set; }
        [Column("PACK_MAX_WEIGHT", "Макс. вес пачки")]
        public float packMaxWeight { get; set; }
        [Column("PACK_MIB_WEIGHT", "Мин. вес пачки")]
        public float packMinWeight { get; set; }
        [Column("SLIT_WIDTH", "Ширина связки")]
        public float slitWidth { get; set; }
        [Column("SLIT_WIDTH_PTOL", "Допуск ширины связки полож.")]
        public float slitWidthPtol { get; set; }
        [Column("SLIT_WIDTH_NTOL", "Допуск ширины связки отриц.")]
        public float slitWidthNtol { get; set; }
        [Column("EDGE_CONITION_CODE", "Тип обработки торцов")]
        public string edgeConitionCode { get; set; }
        [Column("ORDER_STATUS", "Статус заказа")]
        public int orderStatus { get; set; }
        [Column("MATNR", "Номенклатурный номер")]
        public string matnr { get; set; }
        [Column("MATNR_SLAB", "Номенклатура сляба")]
        public string matnrSlab { get; set; }
        [Column("MATNR_STEEL", "Номенклатура стали")]
        public string matnrSteel { get; set; }
        [Column("MATNR_HRC", "Номенклатура рулона")]
        public string matnrHrc { get; set; }
        [Column("D_UDVYAS", "Не используется")]
        public string dUdvyas { get; set; }
        [Column("LINE_NOTE", "Заметки строки заказа")]
        public string lineNote { get; set; }
        [Column("ADDITIONAL_PARAMS_DATA", "Дополнения к заказу")]
        public string additionalParamsData { get; set; }
        [Column("COUNTRY_DESTINATION", "Страна назначения")]
        public string countryDestination { get; set; }
        [Column("USE_VD", "Использовать вакуумный дегазатор")]
        public char useVd { get; set; }
        [Column("OKPO", "Код ОКПО")]
        public string okpo { get; set; }
        [Column("RW_CUSTOMER_CODE", "ЖД код заказчика")]
        public string rwCustomerCode { get; set; }
        [Column("GRADE_CATEGORY", "Категория марки стали")]
        public string gradeCategory { get; set; }
        [Column("GRADE_INTERNAL", "Внутренняя марка стали")]
        public string gradeInternal { get; set; }
        [Column("HEADER_NOTE", "Примечание заказа")]
        public string headerNote { get; set; }
        [Column("TYPE", "Вид заказа из SAP (сбытовой / перемещение)")]
        public string type { get; set; }
    }
}
