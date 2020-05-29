using LPKService.Domain.Models.Work;
namespace LPKService.Infrastructure.DB
{
    public class TCheckRealtedListRepo
    {
        TCheckRelatedList realtedList;
        public TCheckRealtedListRepo()
        {
            realtedList = new TCheckRelatedList();
        }

        public TCheckRealtedListRepo(TCheckRelatedList realtedList)
        {
            this.realtedList = realtedList;
        }

        public void Add(string attrb_code,string codeType)
        {
            realtedList.attrb_code.Add(attrb_code);
            realtedList.codeType.Add(codeType);
        }
        public TCheckRelatedList GetList()
        {
            return realtedList;
        }
    }
}
