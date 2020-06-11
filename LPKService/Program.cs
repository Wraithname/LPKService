using System;
using System.ServiceProcess;
using Work;

namespace LPKService
{
   public partial class Program
    {
        static void Main(string[] args)
        {
            IServiceWork working = new ServiceWorker();

                //Запуск как сервис
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new LPKService(working)
                };
                ServiceBase.Run(ServicesToRun);
        }
    }
}
