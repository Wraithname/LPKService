using LPKService.Repository;

namespace LPKService.Domain.Models.SOM
{
    public class AuxConstant:BaseModel
    {
        [Column("INTEGER_VALUE", "Значение целочисленного типа")]
        public int integerVal { get; }
        [Column("CHAR_VALUE", "Строковое значение")]
        public string charVal { get; }
        [Column("FLOAT_VALUE", "Значение типа 'Число с плавающей запятой'")]
        public float floatVal { get; }
    }
}
