using System;

namespace WindowsHelper.Events
{
    public static class MainWindowEnabledEvent
    {
        public static event EventHandler<bool> MainWindowEnabled;


        public static void RaiseMainWindowEnabledEvent(object sender, bool isEnabled)
        {
            MainWindowEnabled?.Invoke(sender, isEnabled);
        }
    }
}