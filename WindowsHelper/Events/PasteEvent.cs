using System;

namespace WindowsHelper.Events
{
    public static class PasteEvent
    {
        public static event EventHandler Paste;

        public static void RaisePasteEvent(object sender)
        {
            Paste?.Invoke(sender, null);
        }
    }
}