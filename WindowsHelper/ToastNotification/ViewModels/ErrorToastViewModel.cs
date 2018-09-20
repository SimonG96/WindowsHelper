using System;
using WindowsHelper.Interfaces;
using GalaSoft.MvvmLight;

namespace WindowsHelper.ToastNotification.ViewModels
{
    public class ErrorToastViewModel : ViewModelBase, IToast
    {
        public ErrorToastViewModel(Exception exception)
        {
            Exception = exception;
        }

        public ErrorToastViewModel(string message)
        {
            Exception = new Exception(message);
        }


        private Exception Exception { get; set; }


        public string Message => Exception.Message;
    }
}