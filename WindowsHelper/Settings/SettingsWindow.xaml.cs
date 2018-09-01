using System;
using System.Windows;
using WindowsHelper.Interfaces;
using WindowsHelper.Resources.UserControls;

namespace WindowsHelper.Settings
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : WindowsHelperWindow
    {
        public SettingsWindow(IWindowSettings settings, SizeChangedEventHandler onSizeChanged, EventHandler onLocationChanged)
        : base(settings, onSizeChanged, onLocationChanged)
        {
            InitializeComponent();
        }
    }
}
