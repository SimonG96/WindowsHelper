using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.Settings
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private bool _isSelected;

        public SettingsPageViewModel(ISettings settings)
        {
            Settings = settings;
            SettingsEntries = CreateSettingsEntries();
            PageName = settings.Name; //TODO: find a way to get a nicer name
        }

        
        private ISettings Settings { get; }

        public string PageName { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public ObservableCollection<SettingsEntryViewModel> SettingsEntries { get; set; }



        #region Commands

        
        public ICommand TabClickedCommand => new RelayCommand(TabClicked);


        #endregion Commands



        #region Methods


        private ObservableCollection<SettingsEntryViewModel> CreateSettingsEntries()
        {
            ObservableCollection<SettingsEntryViewModel> settingsEntries = new ObservableCollection<SettingsEntryViewModel>();

            var properties = Settings.GetType().GetProperties().Where(p => p.IsDefined(typeof(SettingsPropertyAttribute), false));
            foreach (var property in properties)
            {
                SettingsPropertyAttribute attribute = (SettingsPropertyAttribute) property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(SettingsPropertyAttribute));
                if (attribute == null)
                    continue;

                if (!attribute.SetManual)
                    continue;

                settingsEntries.Add(new SettingsEntryViewModel(property.Name, property.PropertyType, property.GetValue(Settings)));
            }

            return settingsEntries;
        }

        private void TabClicked()
        {
            IsSelected = true;
            TabSelectedEvent?.Invoke(this, null);
        }

        #endregion Methods



        #region Events

        public static event EventHandler TabSelectedEvent;

        #endregion Events
    }
}