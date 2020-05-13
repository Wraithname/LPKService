using System;
using System.ServiceProcess;
using System.Timers;
using System.Runtime.InteropServices;
using System.Configuration;
using Logger;
using Oracle.ManagedDataAccess.Client;
using Repository;
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
        private Log logger = LogFactory.GetLogger(nameof(LPKService));
        private int timeout;
        Timer timer = new Timer();
        public LPKService()
        {

            InitializeComponent();
            base.CanPauseAndContinue = true;
            
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
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
            OracleConnection conn = BaseRepo.GetDBConnection();
            logger.Debug("Запуск как консольное приложение");

            logger.Debug("Завершение работы консольного приложения");
        }
#endif
        protected override void OnStart(string[] args)
        {
            logger.Info("Запуск сервиса");
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 20000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            var config = ConfigurationManager.AppSettings;
            int.TryParse(config.Get("WorkerTimeout") ?? "20000", out timeout);
            timer.Interval = timeout; // 20 секунд
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnContinue()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 20000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            logger.Info("Возобновление обработки");
            base.OnContinue();
        }
        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 20000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            logger.Info("Сервис остановлен");
            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        protected override void OnPause()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 20000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            logger.Info("Приостановление обработки");
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnPause();
            
        }
    }
}
