using System;
using System.Windows;
using System.Windows.Input;
using WindowsHelper.Events;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lib.Tools;

namespace WindowsHelper.ClipboardManager
{
    public class ClipboardObjectViewModel : ViewModelBase, ISelectableItemViewModel
    {
        private bool _isSelected;
        private bool _isEnabled = true;
        private bool _isPinned;
        private bool _isPopupOpen;
        private bool _isPinnable;

        public ClipboardObjectViewModel(object data, ClipboardObjectType type)
        {
            Data = data;
            ClipboardObjectType = type;

            if (type == ClipboardObjectType.NoData)
                IsEnabled = false;

            if (type == ClipboardObjectType.String)
                IsPinnable = true;
        }


        public string Name => Data.ToString();
        public object Data { get; set; }
        public ClipboardObjectType ClipboardObjectType { get; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
                IsClipboardObjectSelectedChanged?.Invoke(this, value);
            }
        }

        public bool IsPinnable
        {
            get => _isPinnable;
            set
            {
                _isPinnable = value;
                RaisePropertyChanged(() => IsPinnable);
            }
        }

        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                _isPinned = value;
                RaisePropertyChanged(() => IsPinned);
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                RaisePropertyChanged(() => IsPopupOpen);
            }
        }


        #region Commands

        public ICommand ClickedCommand => new RelayCommand(Paste);
        public ICommand MouseEnterCommand => new RelayCommand(MouseEnter);
        public ICommand MouseLeaveCommand => new RelayCommand(MouseLeave);
        public ICommand PinCommand => new RelayCommand(Pin);
        public ICommand DeleteCommand => new RelayCommand(Delete);

        #endregion Commands
        

        
        #region Methods

        private void Paste()
        {
            if (ClipboardHelper.SetDataObject(Data, ClipboardManager.OPEN_CLIPBOARD_TRIES))
                PasteEvent.RaisePasteEvent(this);
        }

        private void MouseEnter()
        {
            IsSelected = true;
            IsPopupOpen = true;
        }

        private void MouseLeave()
        {
            IsSelected = false;
            IsPopupOpen = false;
        }

        private void Pin()
        {
            IsPinned = !IsPinned;
            PinStateChanged?.Invoke(this, IsPinned);
        }

        private void Delete()
        {
            ObjectDeleted?.Invoke(this, null);
        }

        #endregion Methods


        #region Events

        public static event EventHandler<bool> IsClipboardObjectSelectedChanged; //TODO: Implement if needed, remove if not
        public static event EventHandler<bool> PinStateChanged;
        public static event EventHandler ObjectDeleted;

        #endregion Events
    }
}