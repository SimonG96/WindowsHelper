using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsHelper.Interfaces;
using WindowsHelper.ToastNotification.Enums;
using GalaSoft.MvvmLight;

namespace WindowsHelper.ToastNotification.ViewModels
{
    public class ToastWindowViewModel : ViewModelBase
    {
        private string _title;
        private ToastBehaviour _behaviour;
        private ToastType _type;

        public ToastWindowViewModel()
        {
            if (!IsInDesignMode)
                throw new InvalidOperationException("This Constructor is for Design Time Usage only!");

            Toast = new InfoToastViewModel("This is an Info Toast");
        }

        public ToastWindowViewModel(ToastBehaviour behaviour, ToastType type)
        {
            Behaviour = behaviour;
            Type = type;
        }


        public string Title //TODO: Is currently never set
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public ToastBehaviour Behaviour
        {
            get => _behaviour;
            set
            {
                _behaviour = value;
                RaisePropertyChanged(() => Behaviour);
            }
        }

        public ToastType Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        public IToast Toast { get; set; }


        public ControlTemplate IconTemplate
        {
            get
            {
                switch (Type)
                {
                    case ToastType.Info:
                        return Application.Current.FindResource("Info") as ControlTemplate;
                    case ToastType.Warning:
                        return Application.Current.FindResource("Warning") as ControlTemplate;
                    case ToastType.Error:
                        return Application.Current.FindResource("Error") as ControlTemplate;
                    case ToastType.Spotify:
                        return Application.Current.FindResource("SpotifyIcon") as ControlTemplate;
                    default:
                        throw new InvalidOperationException($"No Template for type {Type}");
                }
            }
        }

        public SolidColorBrush IconForeground
        {
            get
            {
                switch (Type)
                {
                    case ToastType.Info:
                        return Application.Current.FindResource("InfoForeground") as SolidColorBrush;
                    case ToastType.Warning:
                        return Application.Current.FindResource("WarningForeground") as SolidColorBrush;
                    case ToastType.Error:
                        return Application.Current.FindResource("ErrorForeground") as SolidColorBrush;
                    case ToastType.Spotify:
                        return Application.Current.FindResource("SpotifyForeground") as SolidColorBrush;
                    default:
                        return Application.Current.FindResource("LightForeground") as SolidColorBrush;
                }
            }
        }


        #region Methods



        #endregion Methods
    }
}