using System;
using System.Threading;
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

            MouseEnter += delegate { MouseOverChanged?.Invoke(this, true); };
            MouseLeave += delegate { MouseOverChanged?.Invoke(this, false); };
        }

        public Timer Timer { get; set; }


        #region Methods

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Timer?.Dispose();
            Close();
        }

        #endregion


        #region Events

        public event EventHandler<bool> MouseOverChanged;

        #endregion Events
    }
}
