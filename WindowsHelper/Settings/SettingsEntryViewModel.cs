using System;
using System.CodeDom;
using System.Windows.Input;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lib.Tools;

namespace WindowsHelper.Settings
{
    public class SettingsEntryViewModel : ViewModelBase
    {
        private string _name;
        private Type _entryType;
        private object _value;


        public SettingsEntryViewModel(string name, Type type, object value)
        {
            Name = name;
            EntryType = type;
            Value = value;
        }


        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public Type EntryType
        {
            get => _entryType;
            set
            {
                _entryType = value;
                RaisePropertyChanged(() => EntryType);
            }
        }

        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
                SettingsEntryValueChanged?.Invoke(this, null);
            }
        }


        #region Commands

        public ICommand IncreaseStepCommand => new RelayCommand(IncreaseStep);
        public ICommand DecreaseStepCommand => new RelayCommand(DecreaseStep);
        public ICommand PreviewKeyDownCommand => new RelayCommand<KeyEventArgs>(PreviewKeyDown);

        #endregion


        #region Methods

        public static void SetPropertyForSettingsEntry(object sender, ISettings settings)
        {
            if (!(sender is SettingsEntryViewModel settingsEntry))
                return;

            var property = settings.GetType().GetProperty(settingsEntry.Name);
            if (property == null)
                return;

            property.SetValue(settings, settingsEntry.Value);
        }

        private void IncreaseStep()
        {
            Value = Value.Increment();
        }

        private void DecreaseStep()
        {
            Value = Value.Decrement();
        }

        private void PreviewKeyDown(KeyEventArgs args)
        {
            if (EntryType == typeof(KeyCombination))
                return;

            Value = KeyCombination.GetKeyCombinationForPressedKeys();

            args.Handled = true;
        }

        #endregion Methods


        #region Events

        public static event EventHandler SettingsEntryValueChanged;

        #endregion Events
    }
}