using Repository;

namespace LPKService.Domain.Models
{
    /// <summary>
    /// Модель для получения данных по флагу
    /// </summary>
    public class VecAuto:BaseModel
    {
        [Column("VEHICLE_ID")]
        public string vehicleId { get; set; }
        [Column("AUTO_FLG")]
        public string autoFlg { get; set; }
    }
}
