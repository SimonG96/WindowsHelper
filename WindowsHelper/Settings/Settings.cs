using System;
using System.Windows;
using System.Windows.Input;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using Lib.Tools;
using Lib.Tools.Logging;

namespace WindowsHelper.Settings
{
    public class Settings : IWindowSettings
    {
        private readonly KeyCombination _defaultKeyCombination = new KeyCombination(Key.LeftAlt, Key.Space);


        public Settings(object parent) //TODO: Don't set default values if user settings are saved
        {
            Parent = parent;

            KeyCombination = _defaultKeyCombination;

            SettingsEntryViewModel.SettingsEntryValueChanged += OnSettingsEntryValueChanged;
        }

        public string Name => "Settings";
        public object Parent { get; }



        [SettingsProperty(true, false)]
        public double WindowHeight { get; set; }

        [SettingsProperty(true, false)]
        public double WindowWidth { get; set; }

        [SettingsProperty(true, false)]
        public double WindowTop { get; set; }

        [SettingsProperty(true, false)]
        public double WindowLeft { get; set; }


        [SettingsProperty]
        public bool IsActivated { get; set; } = true;


        [SettingsProperty]
        public KeyCombination KeyCombination { get; set; }


        [SettingsProperty]
        public ICommand OpenLogFileFolderCommand => new RelayCommand(Log.OpenLogFileFolder);

        [SettingsProperty]
        public ICommand ClearLogFilesCommand => new RelayCommand(Log.ClearLogFiles);


        #region Methods

        private void OnSettingsEntryValueChanged(object sender, EventArgs args)
        {
            SettingsEntryViewModel.SetPropertyForSettingsEntry(sender, this);
        }

        public void OnSettingsWindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (!(sender is SettingsWindow settingsWindow))
                return;

            if (settingsWindow.WindowState != WindowState.Normal)
                return;

            WindowHeight = args.NewSize.Height;
            WindowWidth = args.NewSize.Width;
        }

        public void OnSettingsWindowLocationChanged(object sender, EventArgs args)
        {
            if (!(sender is SettingsWindow settingsWindow))
                return;

            if (settingsWindow.WindowState != WindowState.Normal)
                return;

            WindowTop = settingsWindow.Top;
            WindowLeft = settingsWindow.Left;
        }

        #endregion Methods
    }
}