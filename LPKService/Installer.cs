using System.ComponentModel;
using System.ServiceProcess;

namespace LPKService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller spi;
        private ServiceInstaller si;
        public Installer()
        {
            InitializeComponent();
            spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;
            si = new ServiceInstaller();
            //Режим запуска службы Manual, Authomatic (автоматический запуск при загрузке системы) и Disabled (отключено)
            si.StartType = ServiceStartMode.Manual;
            si.ServiceName = "LPKService";
            si.DisplayName = "Служба LPKService";
            Installers.Add(si);
            Installers.Add(spi);
        }
    }
}
