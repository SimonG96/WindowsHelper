using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using WindowsHelper.ToastNotification.Common;
using WindowsHelper.ToastNotification.Enums;
using Lib.Tools.Logging;

namespace WindowsHelper.Common
{
    public static class ExceptionHandler
    {
        public static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args) //TODO: set handled?
        {
            Log.Write(args.Exception);
            Toast.ShowError(args.Exception, ToastBehaviour.TimedLong);
        }

        public static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args) //TODO: set observed?
        {
            Log.Write(args.Exception);
            Toast.ShowError(args.Exception, ToastBehaviour.TimedLong);
        }

        public static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs args) //TODO: handle isTerminating
        {
            if (args.ExceptionObject is Exception exception)
            {
                Log.Write(exception);
                Toast.ShowError(exception, ToastBehaviour.TimedLong);
            }
            else
            {
                string message = "An unhandled Exception occurred!";
                Log.Write(message, LogLevel.Error);
                Toast.ShowError(message, ToastBehaviour.TimedLong);
            }
        }

        public static void HandleException(Exception exception)
        {
            Log.Write(exception);
            Toast.ShowError(exception, ToastBehaviour.TimedLong); //TODO: Choose when to use which behaviour
        }
    }
}