using System;

namespace WindowsHelper.Events
{
    public class SpotifyPageSelectedEvent
    {
        public static event EventHandler SpotifyPageSelected;

        public static void RaiseSpotifyPageSelectedEvent(object sender)
        {
            SpotifyPageSelected?.Invoke(sender, null);
        }    
    }
}