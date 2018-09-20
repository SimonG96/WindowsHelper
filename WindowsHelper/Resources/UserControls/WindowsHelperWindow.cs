using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WindowsHelper.Interfaces;
using Lib.Tools;

namespace WindowsHelper.Resources.UserControls
{
    public class WindowsHelperWindow : Window
    {
        private SizeChangedEventHandler _onSizeChanged;
        private EventHandler _onLocationChanged;

        private bool _minimizing;

        protected WindowsHelperWindow(IWindowSettings settings, SizeChangedEventHandler onSizeChanged, EventHandler onLocationChanged)
        {
            Settings = settings;
            _onSizeChanged = onSizeChanged;
            _onLocationChanged = onLocationChanged;

            //SizeChanged += _onSizeChanged;
            //LocationChanged += _onLocationChanged;
            Loaded += OnLoaded;
        }


        private IWindowSettings Settings { get; set; }

        //private bool Minimizing
        //{
        //    get => _minimizing;
        //    set
        //    {
        //        _minimizing = value;

        //        //if (_minimizing)
        //        //{
        //        //    SizeChanged -= _onSizeChanged;
        //        //    LocationChanged -= _onLocationChanged;
        //        //}
        //        //else
        //        //{
        //        //    SizeChanged += _onSizeChanged;
        //        //    LocationChanged += _onLocationChanged;
        //        //}
        //    }
        //}


        #region IconTemplate

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(ControlTemplate), typeof(WindowsHelperWindow), new PropertyMetadata(OnIconTemplateChanged));

        public ControlTemplate IconTemplate
        {
            get => (ControlTemplate) GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }

        private static void OnIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion IconTemplate



        #region WindowButtons

        private void OnMinimize(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnRestore(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Close();
        }

        #endregion WindowButtons


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("minimizeButton") is Button minimizeButton)
                minimizeButton.Click += OnMinimize;

            if (GetTemplateChild("restoreButton") is Button restoreButton)
                restoreButton.Click += OnRestore;

            if (GetTemplateChild("closeButton") is Button closeButton)
                closeButton.Click += OnClose;

            if (GetTemplateChild("moveHandler") is Grid moveHandler)
                moveHandler.MouseLeftButtonDown += MoveWindow;
        }

        

        protected override void OnSourceInitialized(EventArgs args)
        {
            base.OnSourceInitialized(args);

            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(windowHandle);
            hWndSource?.AddHook(HWndHook);
        }

        private IntPtr HWndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32Api.MessageId.WM_GETMINMAXINFO:
                {
                    if (_minimizing)
                    {
                        handled = true;
                        break;
                    }

                    if (WindowState == WindowState.Maximized)
                    {
                        WindowHelper.Maximize(this, hWnd);
                        handled = true;
                    }
                    else if (WindowState == WindowState.Normal)
                    {
                        Minimize();
                        handled = true;
                    }

                    break;
                }
            }

            return IntPtr.Zero;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            SetDefaultSettings();
            Loaded -= OnLoaded; //Only do that on the first start -> unsubscribe from the Loaded event again
        }

        private void SetDefaultSettings()
        {
            if (WindowState != WindowState.Normal)
                return;

            Settings.WindowLeft = Left;
            Settings.WindowTop = Top;
            Settings.WindowWidth = Width;
            Settings.WindowHeight = Height;
        }

        private void Minimize()
        {
            _minimizing = true;

            Left = Settings.WindowLeft;
            Top = Settings.WindowTop;
            Width = Settings.WindowWidth;
            Height = Settings.WindowHeight;

            _minimizing = false;
        }
    }
}