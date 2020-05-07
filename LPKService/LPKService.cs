using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Linq;
using Logger;
using Oracle.ManagedDataAccess.Client;
namespace LPKService
{
    public partial class LPKService : ServiceBase
    {

        private Log logger = LogFactory.GetLogger(nameof(LPKService));
        Thread ThreadWrok;
        AutoResetEvent ShutdownEvent = new AutoResetEvent(false);
        ManualResetEvent PauseEvent = new ManualResetEvent(false);
        private int timeout;
        public LPKService()
        {
            InitializeComponent();
            base.CanPauseAndContinue = true;
            
        }
#if DEBUG
        // Воспомогательный метод для отладки сервиса
        internal void ConsoleApp(string[] args)
        {
            // Ожидание ctrl+c
            //var exitEvent = new ManualResetEvent(false);
            //Console.WriteLine("Press CTRL+C for exit");
            //Console.CancelKeyPress += (sender, eventArgs) =>
            //{
            //    eventArgs.Cancel = true;
            //    exitEvent.Set();
            //};

            OracleConnection conn = Config.DBOracleUtils.GetDBConnection();

            Console.WriteLine("Get Connection: " + conn);
            try
            {
                conn.Open();

                Console.WriteLine("Successful Connection");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("## ERROR: " + ex.Message);
                Console.Read();
                return;
            }
            logger.Debug("Запуск как консольное приложение");
            OnStart(args);
            //exitEvent.WaitOne();
            OnStop();
            logger.Debug("Завершение работы консольного приложения");
        }
#endif
        protected override void OnStart(string[] args)
        {
            // TODO: Добавьте код для запуска службы.
        }

        protected override void OnStop()
        {
            // TODO: Добавьте код, выполняющий подготовку к остановке службы.
        }
    }
}
