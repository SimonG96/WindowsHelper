using System.Windows;
using Lib.NotifyIconWpf;
using Lib.Tools;

namespace WindowsHelper
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs args)
        {
            RegistryHelper.Initialize("WindowsHelper");

            base.OnStartup(args);
            _notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs args)
        {
            _notifyIcon.Dispose();
            base.OnExit(args);
        }
    }
}
