﻿
namespace Repository.WorkModels
{
    public class JoinedModel
    {
        [Column("COUNT(POS_NUM_ID)", "Количество позиций")]
        public int count { get; set; }
        [Column("POS_NUM_ID", "Номер позиции")]
        public int posNumId { get; set; }
    }
}
