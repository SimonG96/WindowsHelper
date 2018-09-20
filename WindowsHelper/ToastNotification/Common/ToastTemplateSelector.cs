using System;
using System.Windows;
using System.Windows.Controls;
using WindowsHelper.Interfaces;
using WindowsHelper.ToastNotification.ViewModels;

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

            if (!(item is IToast toast))
                throw new InvalidOperationException("Item is not a ToastWindowViewModel");

            if (toast is InfoToastViewModel)
                return InfoTemplate;
            else if (toast is WarningToastViewModel)
                return WarningTemplate;
            else if (toast is ErrorToastViewModel)
                return ErrorTemplate;
            else if (toast is SpotifyToastViewModel)
                return SpotifyTemplate;
            else
                throw new NotImplementedException(); //TODO: What to do in this case?
        }
    }
}