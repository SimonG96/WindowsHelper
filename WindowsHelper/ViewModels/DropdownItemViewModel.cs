using System;
using System.Windows.Input;
using WindowsHelper.Inputs;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WindowsHelper.ViewModels
{
    public class DropdownItemViewModel : ViewModelBase, ISelectableItemViewModel
    {
        private bool _isEnabled = true;
        private bool _isSelected;


        public DropdownItemViewModel(string name)
        {
            Name = name;

            if (Enum.TryParse(name, true, out MenuInputs input))
            {
                ClickedCommand = InputHelper.GetCommandForInput(input, this);
            }
            else
            {
                IsEnabled = false;
            }
        }

        public string Name { get; set; }

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
                IsDropdownItemSelectedChanged?.Invoke(this, value);
            }
        }


        #region Commands


        public ICommand ClickedCommand { get; }
        public ICommand MouseEnterCommand => new RelayCommand(MouseEnter);
        public ICommand MouseLeaveCommand => new RelayCommand(MouseLeave);



        #endregion Commands



        #region Methods



        private void MouseEnter()
        {
            IsSelected = true;
        }

        private void MouseLeave()
        {
            IsSelected = false;
        }

        #endregion Methods


        #region Events


        public static event EventHandler<bool> IsDropdownItemSelectedChanged;


        #endregion Events
    }
}