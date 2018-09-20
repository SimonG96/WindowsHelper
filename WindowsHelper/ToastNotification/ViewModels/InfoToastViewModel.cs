using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.ToastNotification.ViewModels
{
    public class InfoToastViewModel : ViewModelBase, IToast
    {
        private string _message;


        public InfoToastViewModel(string message)
        {
            Message = message;
        }


        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }
    }
}