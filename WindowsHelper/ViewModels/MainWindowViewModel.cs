using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WindowsHelper.ClipboardManager;
using WindowsHelper.Events;
using WindowsHelper.Inputs;
using WindowsHelper.Interfaces;
using WindowsHelper.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Lib.Tools;

namespace WindowsHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        // TODO:
        //  - change Style of DropdownItems (Buttons, MouseOver, isEnabled ...)
        //  - (d) add possibility to use keyboard when moving through any dropdown list
        //  - add settings and settings window
        //  - move all resources to extra class in resources folder (Brushes, Templates?, ...)
        //  - add grip or something similar to move the main window?
        //  - save settings
        //  - (d) add clear button to textbox
        //  - try and use existing BooleanToVisibility Converter (check if it's default is collapsed or hidden for false) (seems to be collapsed which would be good)
        //  - add each Plugin name to possible inputs
        //  - add settings for each plugin (even disabled plugins)
        //  - add search to settings
        //  - rename ISettings and the Settings classes to something that has a singular
        //  - (d) Move Win32 stuff in extra class (Win32Api or similar)
        //  - make version that is able to update automatically
        //  - add icons to notify Icon context menu?
        //
        // MouseOver/Selection via Keyboard:
        //  - (d) MouseOver has to change a Property 'Selected'
        //  - (d) Keys have to change the same property
        //
        // Git/GitHub:
        //  - make Lib.Tools a Git Submodule?
        //  - fork NotifyIcon?
        //
        // ClipboardManager:
        //  - (d) Make Entries saveable
        //  - (d) add paste command
        //  - handle multiline entries (show more than 1 line in tooltip?)
        //  - handle entries that are to long (maybe show also a tooltip? this could get to long too though)
        //  - save all entries on closing? (needs init at start)
        //  - add search box to search old entries?
        //  - add possibility to edit pinned entries in settings?
        //  - add try/catch around clipboard methods -> application should not crash when accessing the clipboard fails
        //
        // Spotify:
        //  - use .net Api from GitHub
        //  - use Toastify aswell?
        //
        // Additional Plugins:
        //  - Spotify Viewer/Player (use Thumbnail/Taskbar Toolbar)
        //-----------------------------------------------------------------------------

        private Visibility _isTextboxVisible = Visibility.Collapsed;
        private string _textboxText;
        private bool _isDropdownOpen;
        private ObservableCollection<DropdownItemViewModel> _dropdownItems;

        public MainWindowViewModel()
        {
            if (IsInDesignMode)
            {
                IsTextboxVisible = Visibility.Visible;
                IsDropdownOpen = true;
                DropdownItems = new ObservableCollection<DropdownItemViewModel>
                {
                    new DropdownItemViewModel("Settings")
                };
            }

            DropdownItems = new ObservableCollection<DropdownItemViewModel>();
            DropdownItems.EnableCollectionSynchronization();

            Settings = new Settings.Settings(this);
            //List<ISettings> settingsList = new List<ISettings>()
            //{
            //    Settings
            //};

            //TODO: Add List of Plugins and call init()), maybe work with attributes here aswell and find all plugins from existing classes?;
            Plugins = new List<IPlugin>
            {
                new ClipboardManager.ClipboardManager(),
                new Spotify.Spotify()
            };

            foreach (var plugin in Plugins)
            {
                plugin.Init(); //TODO: Remove Plugin if init() is false?
                //settingsList.Add(plugin.Settings);
            }

            //SettingsWindowViewModel = new SettingsWindowViewModel(settingsList);

            MainWindowEnabledEvent.MainWindowEnabled += OnMainWindowEnabledEvent;
            OpenPasteWindowEvent.OpenPasteWindow += OnOpenPasteWindow;
            CloseRequestedEvent.CloseRequested += OnCloseRequested;
            PasteEvent.Paste += OnPaste;
            DropdownItemViewModel.IsDropdownItemSelectedChanged += OnIsDropdownItemSelectedChanged;
            ShowSettingsWindowEvent.ShowSettingsWindow += OnShowSettingsWindow;
        }


        public string TextboxText
        {
            get => _textboxText;
            set
            {
                _textboxText = value;
                MaintainDropdownItems(_textboxText);

                RaisePropertyChanged(() => TextboxText);
            }
        }

        public Visibility IsTextboxVisible
        {
            get => _isTextboxVisible;
            set
            {
                _isTextboxVisible = value;
                RaisePropertyChanged(() => IsTextboxVisible);
            }
        }

        public bool IsDropdownOpen
        {
            get => _isDropdownOpen;
            set
            {
                _isDropdownOpen = value;
                RaisePropertyChanged(() => IsDropdownOpen);
            }
        }

        public ObservableCollection<DropdownItemViewModel> DropdownItems
        {
            get => _dropdownItems;
            set
            {
                _dropdownItems = value;
                RaisePropertyChanged(() => DropdownItems);
            }
        }

        private List<IPlugin> Plugins { get; set; }
        private Settings.Settings Settings { get; }
        //private SettingsWindowViewModel SettingsWindowViewModel { get; }



        #region Commands


        public ICommand PreviewKeyDownCommand => new RelayCommand<KeyEventArgs>(PreviewKeyDown);
        public ICommand DeactivatedCommand => new RelayCommand(Deactivated);
        public ICommand ClearTextboxCommand => new RelayCommand(ClearTextbox);


        #endregion Commands



        #region Methods


        private void PreviewKeyDown(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Escape:
                {
                    MainWindowEnabledEvent.RaiseMainWindowEnabledEvent(this, false);
                    break;
                }
                case Key.Down:
                {
                    if (!DropdownItems.Any(i => i.IsEnabled))
                        break;

                    DropdownItemViewModel selectedItem = DropdownItems.FirstOrDefault(i => i.IsSelected);
                    if (selectedItem == null)
                    {
                        DropdownItemViewModel firstItem = DropdownItems.FirstOrDefault(i => i.IsEnabled);
                        if (firstItem == null)
                            break;

                        firstItem.IsSelected = true;
                    }
                    else
                    {
                        int indexOfSelectedItem = DropdownItems.IndexOf(selectedItem);
                        if (indexOfSelectedItem + 2 <= DropdownItems.Count)
                        {
                            DropdownItemViewModel newSelectedItem = DropdownItems[indexOfSelectedItem + 1];
                            newSelectedItem.IsSelected = true;
                        }
                    }

                    break;
                }
                case Key.Up:
                {
                    if (!DropdownItems.Any(i => i.IsEnabled))
                        break;

                    DropdownItemViewModel selectedItem = DropdownItems.FirstOrDefault(i => i.IsSelected);
                    if (selectedItem == null)
                        break;

                    int indexOfSelectedItem = DropdownItems.IndexOf(selectedItem);
                    if (indexOfSelectedItem > 0)
                    {
                        DropdownItemViewModel newSelectedItem = DropdownItems[indexOfSelectedItem - 1];
                        newSelectedItem.IsSelected = true;
                    }

                    break;
                }
                case Key.Enter:
                {
                    DropdownItemViewModel selectedItem = DropdownItems.FirstOrDefault(i => i.IsSelected);
                    selectedItem?.ClickedCommand.Execute(null);

                    break;
                }
            }
        }

        private void Deactivated()
        {
            MainWindowEnabledEvent.RaiseMainWindowEnabledEvent(this, false);

            foreach (var plugin in Plugins)
            {
                plugin.DeInit();
            }
        }

        private void ClearTextbox()
        {
            TextboxText = "";
        }

        private void MaintainDropdownItems(string input)
        {
            if (input.IsNullOrEmpty())
            {
                IsDropdownOpen = false;
                DropdownItems = new ObservableCollection<DropdownItemViewModel>();
            }
            else
            {
                IsDropdownOpen = true;
                DropdownItems = InputHelper.CheckInput(input).ToObservableCollection();
            }
        }


        private void OnMainWindowEnabledEvent(object sender, bool isEnabled)
        {
            if (!Settings.IsActivated)
                return;

            if (isEnabled)
            {
                IsTextboxVisible = Visibility.Visible;
                Messenger.Default.Send("TextBox", "SetFocus");
            }
            else
            {
                IsTextboxVisible = Visibility.Collapsed;
                IsDropdownOpen = false;
                ClearTextbox();
            }
        }
        
        //TODO: Move this Function out of MainWindowViewModel -> ClipboardManager: MainWindow should not be responsible for events of plugins
        private void OnOpenPasteWindow(object sender, EventArgs args) //TODO: Check if one paste window is already opened OR use normal textbox at the top to show the items
        {
            if (!Settings.IsActivated)
                return;

            List<ClipboardManagerWindow> openClipboardManagerWindows = Application.Current.Windows.OfType<ClipboardManagerWindow>().ToList();
            foreach (var openClipboardManagerWindow in openClipboardManagerWindows)
            {
                openClipboardManagerWindow.Close();
            }

            var clipboardManagerPlugin = Plugins.FirstOrDefault(p => p.Name.Equals(nameof(ClipboardManager.ClipboardManager))); //TODO: Find a nicer way to do this
            
            if (!(clipboardManagerPlugin is ClipboardManager.ClipboardManager clipboardManager))
                return;

            ClipboardManagerWindow clipboardManagerWindow = new ClipboardManagerWindow();
            clipboardManagerWindow.DataContext = new ClipboardManagerViewModel(clipboardManager, clipboardManagerWindow);
            clipboardManagerWindow.Owner = Application.Current.MainWindow;
            //clipboardManagerWindow.ShowDialog(); //TODO: Maybe use show?
            clipboardManagerWindow.ShowCenteredToMouse();
        }

        private void OnCloseRequested(object sender, EventArgs args)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void OnPaste(object sender, EventArgs args)
        {
            Messenger.Default.Send("Paste", "Paste");
        }

        private void OnIsDropdownItemSelectedChanged(object sender, bool value)
        {
            if (!value)
                return;

            foreach (var item in DropdownItems.Where(i => i.IsSelected))
            {
                if (item.Equals(sender))
                    continue;

                item.IsSelected = false;
            }
        }

        private void OnShowSettingsWindow(object sender, EventArgs args)
        {
            List<ISettings> settings = new List<ISettings>()
            {
                Settings
            };

            foreach (var plugin in Plugins)
            {
                settings.Add(plugin.Settings);
            }

            SettingsWindow settingsWindow = new SettingsWindow();
            SettingsWindowViewModel settingsWindowViewModel = new SettingsWindowViewModel(settings);
            settingsWindow.DataContext = settingsWindowViewModel;
            settingsWindow.Owner = Application.Current.MainWindow;
            settingsWindow.ShowDialog();
        }


        private void Close()
        {
            foreach (var plugin in Plugins)
            {
                plugin.Dispose();
            }
        }

        #endregion Methods
    }
}