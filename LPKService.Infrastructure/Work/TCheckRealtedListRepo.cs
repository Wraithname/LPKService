using LPKService.Domain.Models.Work;
namespace LPKService.Infrastructure.Work
{
    public class TCheckRealtedListRepo
    {
        private TCheckRelatedList realtedList;
        /// <summary>
        /// Конструткор создания листа
        /// </summary>
        public TCheckRealtedListRepo()
        {
            realtedList = new TCheckRelatedList();
        }
        /// <summary>
        /// Контруктор присвоения получаемого листа
        /// </summary>
        /// <param name="realtedList"></param>
        public TCheckRealtedListRepo(TCheckRelatedList realtedList)
        {
            this.realtedList = realtedList;
        }
        /// <summary>
        /// Добавление значение в лист
        /// </summary>
        /// <param name="attrb_code">код аттрибута</param>
        /// <param name="codeType">код типа</param>
        public void Add(string attrb_code,string codeType)
        {
            realtedList.attrb_code.Add(attrb_code);
            realtedList.codeType.Add(codeType);
        }
        /// <summary>
        /// Получение чек-листа
        /// </summary>
        /// <returns>Лист аттрибутов и кодов</returns>
        public TCheckRelatedList GetList()
        {
            return realtedList;
        }
    }
}
