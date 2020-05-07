using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPKService.SDK
{
    public interface IWorker
    {
        void Load();
        void Unload();
        void DoWork();
    }
}