using System;
using System.Linq;
using System.Windows.Input;
using WindowsHelper.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.ClipboardManager
{
    public class ClipboardManagerViewModel : ViewModelBase
    {
        private readonly ClipboardObjectViewModel NO_DATA_ITEM = new ClipboardObjectViewModel("No Data available", ClipboardObjectType.NoData);

        private bool _closing;

        public ClipboardManagerViewModel()
        {
            if (!IsInDesignMode)
                throw new InvalidOperationException("This Constructor is for Design Time Usage only!");

            ClipboardManager = new ClipboardManager();
            ClipboardManager.ClipboardObjects.Add(NO_DATA_ITEM);
            ClipboardManager.SavedClipboardObjects.Add(new ClipboardObjectViewModel("Saved Entry", ClipboardObjectType.String));
        }

        public ClipboardManagerViewModel(ClipboardManager clipboardManager, ClipboardManagerWindow window)
        {
            ClipboardManager = clipboardManager;
            ClipboardManagerWindow = window;

            if (!ClipboardManager.ClipboardObjects.Any())
            {
                ClipboardManager.ClipboardObjects.Add(NO_DATA_ITEM);
            }

            PasteEvent.Paste += OnPasteEvent;
        }


        public ClipboardManager ClipboardManager { get; set; }
        private ClipboardManagerWindow ClipboardManagerWindow { get; set; }


        #region Commands


        public ICommand PreviewKeyDownCommand => new RelayCommand<KeyEventArgs>(PreviewKeyDown);
        public ICommand DeactivatedCommand => new RelayCommand(Deactivated);

        #endregion Commands


        #region Methods

        private void PreviewKeyDown(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Escape:
                {
                    Close();
                    break;
                }
                case Key.Down:
                {
                    if (!ClipboardManager.ClipboardObjects.Any(i => i.IsEnabled))
                        break;

                    ClipboardObjectViewModel selectedItem = ClipboardManager.ClipboardObjects.FirstOrDefault(i => i.IsSelected);
                    if (selectedItem == null)
                    {
                        ClipboardObjectViewModel firstItem = ClipboardManager.ClipboardObjects.FirstOrDefault(i => i.IsEnabled);
                        if (firstItem == null)
                            break;

                        firstItem.IsSelected = true;
                    }
                    else
                    {
                        int indexOfSelectedItem = ClipboardManager.ClipboardObjects.IndexOf(selectedItem);
                        if (indexOfSelectedItem + 2 <= ClipboardManager.ClipboardObjects.Count)
                        {
                            ClipboardObjectViewModel newSelectedItem = ClipboardManager.ClipboardObjects[indexOfSelectedItem + 1];
                            newSelectedItem.IsSelected = true;
                        }
                    }

                    break;
                }
                case Key.Up:
                {
                    if (!ClipboardManager.ClipboardObjects.Any(i => i.IsEnabled))
                        break;

                    ClipboardObjectViewModel selectedItem = ClipboardManager.ClipboardObjects.FirstOrDefault(i => i.IsSelected);
                    if (selectedItem == null)
                        break;

                    int indexOfSelectedItem = ClipboardManager.ClipboardObjects.IndexOf(selectedItem);
                    if (indexOfSelectedItem > 0)
                    {
                        ClipboardObjectViewModel newSelectedItem = ClipboardManager.ClipboardObjects[indexOfSelectedItem - 1];
                        newSelectedItem.IsSelected = true;
                    }

                    break;
                }
                case Key.Enter:
                {
                    ClipboardObjectViewModel selectedItem = ClipboardManager.ClipboardObjects.FirstOrDefault(i => i.IsSelected);
                    selectedItem?.ClickedCommand.Execute(null);

                    break;
                }
            }
        }

        private void Deactivated()
        {
            Close();
        }

        private void Close()
        {
            if (_closing)
                return;

            _closing = true;
            ClipboardManagerWindow.Close();
        }

        private void OnPasteEvent(object sender, EventArgs args)
        {
            Close();
        }

        #endregion Methods
    }
}