using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work
{
    interface IPartitionOrder
    {
        bool PartitionOfOrder(int MsgCounter);
    }
    class PartitionOrder : IPartitionOrder
    {
        public bool PartitionOfOrder(int MsgCounter)
        {
            throw new NotImplementedException();
        }
    }
}
