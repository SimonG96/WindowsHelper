using WindowsHelper.Settings;

namespace WindowsHelper.Interfaces
{
    public interface IWindowSettings : ISettings
    {
        double WindowHeight { get; set; }
        double WindowWidth { get; set; }
        double WindowTop { get; set; }
        double WindowLeft { get; set; }
    }
}