using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WindowsHelper.Common;
using Lib.NotifyIconWpf;
using Lib.Tools;
using Lib.Tools.Logging;

namespace WindowsHelper
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notifyIcon;
        private Log _log;

        protected override void OnStartup(StartupEventArgs args)
        {
            try
            {
                _log = new Log();
                Log.Write($"Windows Helper {Assembly.GetExecutingAssembly().GetName().Version}");
                Log.Write($"Written by SimonG");
                Log.Write("");
                Log.Write($"OS: {Environment.OSVersion}");
                Log.Write($"User: {Environment.UserName} @ {Environment.UserDomainName}");

                DispatcherUnhandledException += ExceptionHandler.OnDispatcherUnhandledException;
                TaskScheduler.UnobservedTaskException += ExceptionHandler.OnUnobservedTaskException;
                AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.OnCurrentDomainUnhandledException;

                RegistryHelper.Initialize("WindowsHelper");
                _notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");

                base.OnStartup(args);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                ExceptionHandler.HandleException(ex);
            }
        }

        protected override void OnExit(ExitEventArgs args)
        {
            _notifyIcon.Dispose();
            _log.Dispose();
            base.OnExit(args);
        }
    }
}
