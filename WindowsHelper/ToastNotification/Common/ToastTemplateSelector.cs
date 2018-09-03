using System;
using System.Windows;
using System.Windows.Controls;
using WindowsHelper.ToastNotification.Enums;

namespace WindowsHelper.ToastNotification.Common
{
    public class ToastTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InfoTemplate { private get; set; }
        public DataTemplate WarningTemplate { private get; set; }
        public DataTemplate ErrorTemplate { private get; set; }
        public DataTemplate SpotifyTemplate { private get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            if (!(item is  ToastWindowViewModel toastWindowViewModel))
                throw new InvalidOperationException("Item is not a ToastWindowViewModel");

            switch (toastWindowViewModel.Type)
            {
                case ToastType.Info:
                    return InfoTemplate;
                case ToastType.Warning:
                    return WarningTemplate;
                case ToastType.Error:
                    return ErrorTemplate;
                case ToastType.Spotify:
                    return SpotifyTemplate;
                default:
                    throw new NotImplementedException(); //TODO: What to do in this case?
            }
        }
    }
}
