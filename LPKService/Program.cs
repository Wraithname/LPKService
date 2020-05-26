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
#if DEBUG
            //Запуск как консольное
            if (Environment.UserInteractive)
            {
                LPKService service1 = new LPKService(working);
                service1.ConsoleApp(args);
            }
            else
            {
#endif
                //Запуск как сервис
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new LPKService(working)
                };
                ServiceBase.Run(ServicesToRun);
#if DEBUG
            }
#endif
        }
    }
}
