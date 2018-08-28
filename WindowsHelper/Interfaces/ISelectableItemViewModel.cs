using System.Windows.Input;

namespace WindowsHelper.Interfaces
{
    public interface ISelectableItemViewModel
    {
        string Name { get; }
        bool IsEnabled { get; set; }
        bool IsSelected { get; set; }


        #region Commands

        ICommand ClickedCommand { get; }
        ICommand MouseEnterCommand { get; }
        ICommand MouseLeaveCommand { get; }

        #endregion Commands
    }
}