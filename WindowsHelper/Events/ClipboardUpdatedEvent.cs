using System;

namespace WindowsHelper.Events
{
    public static class ClipboardUpdatedEvent
    {
        public static event EventHandler ClipboardUpdated;


        public static void RaiseClipboardUpdatedEvent(object sender)
        {
            ClipboardUpdated?.Invoke(sender, null);
        }
    }
}