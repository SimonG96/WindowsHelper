using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WindowsHelper.Events;
using WindowsHelper.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Lib.Tools;
using Application = System.Windows.Application;

namespace WindowsHelper
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_CLIPBOARDUPDATE = 0x031D;
        private const int WM_HOTKEY = 0x0312;
        private const int WM_PASTE = 0x0302;

        private IntPtr _windowHandle;
        private HwndSource _hwndSource;


        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();

            Messenger.Default.Register<string>(this, "SetFocus", SetFocus);
            Messenger.Default.Register<string>(this, "Paste", Paste);
        }


        #region Methods


        protected override void OnSourceInitialized(EventArgs args)
        {
            base.OnSourceInitialized(args);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _hwndSource = HwndSource.FromHwnd(_windowHandle);
            _hwndSource?.AddHook(HwndHook);

            //Win32Api.RegisterHotKey(_windowHandle, OPEN_HOTKEY_ID, MOD_SHIFT, VK_SPACE); //TODO: Use hotkey from settings
            Win32Api.RegisterHotKey(_windowHandle, Win32Api.HotkeyId.OPEN_HOTKEY_ID, Win32Api.KeyCodes.MOD_ALT, Win32Api.KeyCodes.VK_SPACE);
            Win32Api.RegisterHotKey(_windowHandle, Win32Api.HotkeyId.PASTE_HOTKEY_ID, Win32Api.KeyCodes.MOD_ALT, Win32Api.KeyCodes.VK_V);
            Win32Api.AddClipboardFormatListener(_windowHandle);
        }

        private IntPtr HwndHook(IntPtr hwndm, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                {
                    switch (wParam.ToInt32())
                    {
                        case Win32Api.HotkeyId.OPEN_HOTKEY_ID:
                        {
                            int vkey = ((int) lParam >> 16) & 0xFFFF;
                            if (vkey == Win32Api.KeyCodes.VK_SPACE)
                            {
                                MainWindowEnabledEvent.RaiseMainWindowEnabledEvent(this, true);
                            }

                            handled = true;
                            break;
                        }
                        case Win32Api.HotkeyId.PASTE_HOTKEY_ID:
                        {
                            int vkey = ((int) lParam >> 16) & 0xFFFF;
                            if (vkey == Win32Api.KeyCodes.VK_V)
                            {
                                OpenPasteWindowEvent.RaiseOpenPasteWindowEvent(this);
                            }

                            handled = true;
                            break;
                        }
                    }

                    break;
                }
                case WM_CLIPBOARDUPDATE:
                {
                    ClipboardUpdatedEvent.RaiseClipboardUpdatedEvent(this);
                    break;
                }
            }

            return IntPtr.Zero;
        }



        private void SetFocus(string msg)
        {
            if (msg.Equals("TextBox"))
            {
                TextBox.Focus();
            }
        }

        private void Paste(string msg)
        {
            //KeyEventArgs ctrlDownArgs = new KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice, PresentationSource.FromVisual(this), 0, Key.LeftCtrl);
            //ctrlDownArgs.RoutedEvent = Keyboard.KeyDownEvent;

            //KeyEventArgs vDownArgs = new KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice, PresentationSource.FromVisual(this), 0, Key.V);
            //vDownArgs.RoutedEvent = Keyboard.KeyDownEvent;

            //InputManager.Current.ProcessInput(ctrlDownArgs);
            //InputManager.Current.ProcessInput(vDownArgs);

            Window activeWindow = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (!window.IsActive)
                    continue;

                activeWindow = window;
                break;
            }

            if (activeWindow != null)
                activeWindow.WindowState = WindowState.Minimized;

            //var processes = Process.GetProcesses();
            //var active = processes.Where(p => !p.MainWindowTitle.IsNullOrEmpty());
            //var chrome = active.FirstOrDefault(a => a.ProcessName.Contains("chrome"));

            //SendMessage(_windowHandle, WM_PASTE, 0, IntPtr.Zero);
            //SetForegroundWindow(chrome.MainWindowHandle);
            //var hWnd = GetForegroundWindow();
            //var source = HwndSource.FromHwnd(hWnd);
            //var wnd = (Window) source.RootVisual;
            SendKeys.SendWait("^v"); //TODO: Find a way to Paste to another application without using windows forms
            //SendMessage(chrome.MainWindowHandle, WM_PASTE, 0, IntPtr.Zero);
        }


        protected override void OnClosed(EventArgs args)
        {
            _hwndSource.RemoveHook(HwndHook);
            Win32Api.UnregisterHotKey(_windowHandle, Win32Api.HotkeyId.OPEN_HOTKEY_ID);

            base.OnClosed(args);
        }


        #endregion Methods
    }
}
