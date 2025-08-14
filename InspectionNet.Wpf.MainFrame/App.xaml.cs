using System.Windows;

namespace InspectionNet.Wpf.MainFrame
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper.ConfigureServices();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Bootstrapper.CleanupServices();
            base.OnExit(e);
        }
    }
}
