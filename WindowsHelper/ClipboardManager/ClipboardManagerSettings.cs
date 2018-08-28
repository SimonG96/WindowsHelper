using System;
using System.Windows.Input;
using WindowsHelper.Interfaces;
using WindowsHelper.Settings;
using Lib.Tools;

namespace WindowsHelper.ClipboardManager
{
    public class ClipboardManagerSettings : ISettings
    {
        private const int DEFAULT_MAX_ENTRIES = 10;

        private readonly KeyCombination _defaultPasteKeyCombination = new KeyCombination(Key.LeftAlt, Key.V);


        public ClipboardManagerSettings(object parent) //TODO: Don't set default values if user settings are saved
        {
            Parent = parent;

            PasteKeyCombination = _defaultPasteKeyCombination;
            MaxEntries = DEFAULT_MAX_ENTRIES;

            SettingsEntryViewModel.SettingsEntryValueChanged += OnSettingsEntryValueChanged;
        }


        public string Name => "Clipboard Manager";
        public object Parent { get; }

        [SettingsProperty]
        public KeyCombination PasteKeyCombination { get; set; }

        [SettingsProperty]
        public int MaxEntries { get; set; }



        #region Methods

        private void OnSettingsEntryValueChanged(object sender, EventArgs args)
        {
            SettingsEntryViewModel.SetPropertyForSettingsEntry(sender, this);
        }

        #endregion Methods
    }
}