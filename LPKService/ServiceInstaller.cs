using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace LPKService
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
            serviceProcessInstaller1.Account = ServiceAccount.LocalService;
            //Режим запуска службы Manual, Authomatic (автоматический запуск при загрузке системы) и Disabled (отключено)
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            serviceInstaller1.ServiceName = "LPKService";
            serviceInstaller1.DisplayName = "Служба LPKService";
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
