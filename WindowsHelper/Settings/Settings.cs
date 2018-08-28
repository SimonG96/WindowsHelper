using System;
using System.Windows.Input;
using WindowsHelper.Interfaces;
using Lib.Tools;

namespace WindowsHelper.Settings
{
    public class Settings : ISettings
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



        [SettingsProperty]
        public bool IsActivated { get; set; } = true;


        [SettingsProperty]
        public KeyCombination KeyCombination { get; set; }


        #region Methods

        private void OnSettingsEntryValueChanged(object sender, EventArgs args)
        {
            SettingsEntryViewModel.SetPropertyForSettingsEntry(sender, this);
        }

        #endregion Methods
    }
}