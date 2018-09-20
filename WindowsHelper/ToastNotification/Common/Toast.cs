using System;
using System.Threading;
using System.Windows;
using WindowsHelper.ToastNotification.Enums;
using WindowsHelper.ToastNotification.ViewModels;
using Lib.Tools;

namespace WindowsHelper.ToastNotification.Common
{
    public static class Toast
    {
        private const int SHORT_TIMED_TOAST = 4000; //TODO: Add these two constants to settings? Add times for each type to the right settings
        private const int LONG_TIMED_TOAST = 10000;

        public static void ShowInfo(string message, ToastBehaviour behaviour)
        {
            ToastWindowViewModel toastWindowViewModel = new ToastWindowViewModel(behaviour, ToastType.Info)
            {
                Toast = new InfoToastViewModel(message)
            };

            ShowToast(toastWindowViewModel);
        }

        public static void ShowWarning(ToastBehaviour behaviour)
        {
            ToastWindowViewModel toastWindowViewModel = new ToastWindowViewModel(behaviour, ToastType.Warning)
            {
                Toast = new WarningToastViewModel()
            };

            ShowToast(toastWindowViewModel);
        }

        public static void ShowError(Exception exception, ToastBehaviour behaviour)
        {
            ToastWindowViewModel toastWindowViewModel = new ToastWindowViewModel(behaviour, ToastType.Error)
            {
                Toast = new ErrorToastViewModel(exception)
            };

            ShowToast(toastWindowViewModel);
        }

        public static void ShowError(string message, ToastBehaviour behaviour)
        {
            ToastWindowViewModel toastWindowViewModel = new ToastWindowViewModel(behaviour, ToastType.Error)
            {
                Toast = new ErrorToastViewModel(message)
            };

            ShowToast(toastWindowViewModel);
        }

        public static void ShowSpotify(ToastBehaviour behaviour)
        {
            ToastWindowViewModel toastWindowViewModel = new ToastWindowViewModel(behaviour, ToastType.Spotify)
            {
                Toast = new SpotifyToastViewModel()
            };

            ShowToast(toastWindowViewModel);
        }


        private static void ShowToast(ToastWindowViewModel toastWindowViewModel) //TODO: can there be more than one toast at a time? if not a handling for this case is needed, maybe add to an itemsource
        {
            ToastWindow toastWindow = new ToastWindow
            {
                DataContext = toastWindowViewModel,
                Visibility = Visibility.Hidden
            };

            toastWindow.Show();

            Win32Api.W32Rect monitor = WindowHelper.GetPrimaryMonitor();
            toastWindow.Left = monitor.Right - toastWindow.Width;
            toastWindow.Top = monitor.Bottom - toastWindow.Height;

            toastWindow.Visibility = Visibility.Visible;

            if (toastWindowViewModel.Behaviour == ToastBehaviour.TimedShort)
            {
                toastWindow.Timer = new Timer(HideToast, toastWindow, SHORT_TIMED_TOAST, 100);
                toastWindow.MouseOverChanged += delegate(object sender, bool isMouseOver)
                {
                    if (isMouseOver)
                    {
                        toastWindow.Timer.Change(Timeout.Infinite, Timeout.Infinite);
                        toastWindow.Opacity = 1;
                    }
                    else
                    {
                        toastWindow.Timer.Change(SHORT_TIMED_TOAST, 100);
                    }
                };
            }
            else if (toastWindowViewModel.Behaviour == ToastBehaviour.TimedLong)
            {
                toastWindow.Timer = new Timer(HideToast, toastWindow, LONG_TIMED_TOAST, 100);
                toastWindow.MouseOverChanged += delegate (object sender, bool isMouseOver)
                {
                    if (isMouseOver)
                    {
                        toastWindow.Timer.Change(Timeout.Infinite, Timeout.Infinite);
                        toastWindow.Opacity = 1;
                    }
                    else
                    {
                        toastWindow.Timer.Change(LONG_TIMED_TOAST, 100);
                    }
                };
            }
        }

        private static void HideToast(object state)
        {
            if (!(state is ToastWindow toastWindow))
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                double newOpacity = toastWindow.Opacity - 0.05;
                toastWindow.Opacity = newOpacity;

                if (newOpacity <= 0 || toastWindow.Visibility == Visibility.Collapsed)
                {
                    toastWindow.Visibility = Visibility.Collapsed;
                    toastWindow.Close();
                }
            });
        }
    }
}