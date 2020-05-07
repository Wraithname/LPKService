using System;
using System.ServiceProcess;
using Logger;

namespace LPKService
{
   public partial class Program
    {
        
        static void Main(string[] args)
        {
#if DEBUG
            //Запуск как консольное
            if (Environment.UserInteractive)
            {
                LPKService service1 = new LPKService();
                service1.ConsoleApp(args);
            }
            else
            {
#endif
                //Запуск как сервис
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new LPKService()
                };
                ServiceBase.Run(ServicesToRun);
#if DEBUG
            }
#endif
        }
    }
}
