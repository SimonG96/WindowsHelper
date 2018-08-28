using System;

namespace WindowsHelper.Events
{
    public static class OpenPasteWindowEvent
    {
        public static event EventHandler OpenPasteWindow;

        public static void RaiseOpenPasteWindowEvent(object sender)
        {
            OpenPasteWindow?.Invoke(sender, null);
        }
    }
}