using System.Windows;

namespace WindowsHelper.ToastNotification
{
    /// <summary>
    /// Interaktionslogik für ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        public ToastWindow()
        {
            InitializeComponent();
        }

        #region Methods

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Close();
        }

        #endregion
    }
}
