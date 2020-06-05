using System.Collections.Generic;

namespace LPKService.Domain.Models.Work
{
    /// <summary>
    /// Класс получения кода аттирбутов и их тип
    /// </summary>
    public class TCheckRelatedList
    {
        public List<string> attrb_code { get; set; }
        public List<string> codeType { get; set; }
    }
}
