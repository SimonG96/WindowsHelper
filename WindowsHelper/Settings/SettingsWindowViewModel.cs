using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.Settings
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private SettingsPageViewModel _selectedSettingsPage;

        public SettingsWindowViewModel()
        {
            if (!IsInDesignMode)
                throw new InvalidOperationException("This Constructor is for Design Time Usage only!");
        }

        public SettingsWindowViewModel(List<ISettings> settings)
        {
            Settings = settings;
            SettingsPages = CreateSettingsPages();

            SettingsPageViewModel.TabSelectedEvent += OnTabSelected;
        }


        private List<ISettings> Settings { get; set; }

        public ObservableCollection<SettingsPageViewModel> SettingsPages { get; set; }

        public SettingsPageViewModel SelectedSettingsPage
        {
            get => _selectedSettingsPage;
            set
            {
                _selectedSettingsPage = value;
                _selectedSettingsPage.IsSelected = true;
                RaisePropertyChanged(() => SelectedSettingsPage);
            }
        }


        #region Methods


        private ObservableCollection<SettingsPageViewModel> CreateSettingsPages()
        {
            ObservableCollection<SettingsPageViewModel> settingsPages = new ObservableCollection<SettingsPageViewModel>();

            foreach (var settings in Settings)
            {
                SettingsPageViewModel settingsPage = new SettingsPageViewModel(settings);
                settingsPages.Add(settingsPage);

                if (settings is Settings)
                    SelectedSettingsPage = settingsPage;
            }

            return settingsPages;
        }


        private void OnTabSelected(object sender, EventArgs args)
        {
            foreach (var page in SettingsPages.Where(p => p.IsSelected))
            {
                if (page.Equals(sender))
                {
                    SelectedSettingsPage = page;
                    continue;
                }

                page.IsSelected = false;
            }
        }

        #endregion Methods
    }
}