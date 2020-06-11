using System.ServiceProcess;
using System.Timers;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;
using NLog;

namespace LPKService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00025301,
        SERVICE_START_PENDING = 0x00025302,
        SERVICE_STOP_PENDING = 0x00025303,
        SERVICE_RUNNING = 0x00025304,
        SERVICE_CONTINUE_PENDING = 0x00025305,
        SERVICE_PAUSE_PENDING = 0x00025306,
        SERVICE_PAUSED = 0x00025307,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
    public partial class LPKService : ServiceBase
    {
        private Logger logger=LogManager.GetLogger(nameof(LPKService));
        private int timeout;
        Thread workthread;
        AutoResetEvent ShutdownEvent = new AutoResetEvent(false);
        ManualResetEvent PauseEvent = new ManualResetEvent(false);
        private readonly Work.IServiceWork working;
        public LPKService(Work.IServiceWork working)
        {
            InitializeComponent();
            this.working = working;
            base.CanPauseAndContinue = true;
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
#if DEBUG
        // Воспомогательный метод для отладки сервиса
        internal void ConsoleApp(string[] args)
        {
            //logger.Debug("Запуск как консольное приложение");
            using (OracleConnection conn = BaseRepo.GetDBConnection())
            {
                logger.Debug("Запуск сервиса");
                Console.WriteLine(conn.ExecuteScalar<string>("SELECT user_id FROM all_users where username='OMK'", null));
                Console.WriteLine("Success");
                Console.ReadKey();
            }
                
        }
#endif
        /// <summary>
        /// Запуск сервиса
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            logger.Info("Запуск сервиса");
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 5000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            var config = ConfigurationManager.AppSettings;
            int.TryParse(config.Get("WorkerTimeout") ?? "20000", out timeout);
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            workthread = new Thread(PeriodExecuteStart) {IsBackground=true };
            workthread.Start();
        }
        /// <summary>
        /// Функция для периодического запуска
        /// </summary>
        private void PeriodExecuteStart()
        {
            while (!ShutdownEvent.WaitOne(timeout))
            {
                if (!PauseEvent.WaitOne(0))
                {
                    logger.Debug($"Запуска обработчика каждые {timeout / 1000} сек.");
                    working.MngLoop();
                }
            }
        }
        /// <summary>
        /// Продолжение работы сервиса
        /// </summary>
        protected override void OnContinue()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 5000;
            PauseEvent.Reset();
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            logger.Info("Возобновление обработки");
            base.OnContinue();
        }
        /// <summary>
        /// Остановка работы сервиса
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 5000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            ShutdownEvent.Set();
            workthread?.Join(10000);
            logger.Info("Сервис остановлен");
            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        /// <summary>
        /// Приостановка работы сервиса
        /// </summary>
        protected override void OnPause()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 5000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            logger.Info("Приостановление обработки");
            PauseEvent.Set();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnPause();
            
        }
    }
}
