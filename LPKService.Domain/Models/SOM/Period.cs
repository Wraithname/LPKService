using Repository;
using System;

namespace LPKService.Domain.Models.SOM
{
    /// <summary>
    /// Модель таблицы для получения периода
    /// </summary>
    public class Period:BaseModel
    {
        [Column("PERIOD")]
        public int period { get; }
        [Column("PERIOD_ID")]
        public int periodId { get; }
        [Column("STOP_DATE")]
        public DateTime stopDate { get; }
    }
}
