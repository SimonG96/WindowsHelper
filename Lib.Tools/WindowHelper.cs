using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Lib.Tools
{
    public static class WindowHelper
    {
        private const int MONITOR_DEFAULT_NEAREST = 0x00000002;

        public static void ShowCenteredToMouse(this Window window)
        {
            //IntPtr hWnd = Win32Api.GetForegroundWindow();

            //uint threadId1 = Win32Api.GetCurrentThreadId();
            //uint threadId2 = Win32Api.GetWindowThreadProcessId(hWnd, out uint processId);

            //if (threadId1 != threadId2)
            //    Win32Api.AttachThreadInput(threadId1, threadId2, true);

            //IntPtr hWndCursor = Win32Api.GetFocus();
            //Win32Api.GetCaretPos(out Point point);

            //Win32Api.ClientToScreen(hWndCursor, ref point);

            Point? caretPosition = GetCaretPosition();

            PrepareWindow(window);
            window.Show();
            ComputeTopLeft(ref window, caretPosition);

            //if (threadId1 != threadId2)
            //    Win32Api.AttachThreadInput(threadId1, threadId2, false);
        }

        public static bool? ShowDialogCenteredToMouse(this Window window) //TODO: If this is still needed it needs to be fixed (size of window is not calculated at this point)
        {
            PrepareWindow(window);
            return window.ShowDialog();
        }

        public static void Maximize(Window window, IntPtr hWnd)
        {
            IntPtr monitor = Win32Api.MonitorFromWindow(hWnd, MONITOR_DEFAULT_NEAREST);

            if (monitor == IntPtr.Zero)
                return;

            Win32Api.W32MonitorInfo monitorInfo = new Win32Api.W32MonitorInfo()
            {
                Size = Marshal.SizeOf(typeof(Win32Api.W32MonitorInfo))
            };

            Win32Api.GetMonitorInfo(monitor, ref monitorInfo);
            Win32Api.W32Rect rcWorkArea = monitorInfo.WorkArea;
            Win32Api.W32Rect rcMonitorArea = monitorInfo.Monitor;
            window.Left = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
            window.Top = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
            window.Width = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
            window.Height = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
        }

        private static void PrepareWindow(Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            //ComputeTopLeft(ref window);
        }

        private static Point? GetCaretPosition() //TODO: Right now this is not really working -> returns always null until fixed or at least better
        {
            return null;

            string activeProcessName = ProcessHelper.GetActiveProcessName();
            if (activeProcessName.IsNullOrEmpty())
                return null;

            Point caretPosition = new Point();
            Win32Api.W32GuiThreadInfo guiInfo = new Win32Api.W32GuiThreadInfo();
            guiInfo.CbSize = (uint) Marshal.SizeOf(guiInfo);

            Win32Api.GetGUIThreadInfo(0, out guiInfo);

            if (guiInfo.RcCaret.Left == 0 && guiInfo.RcCaret.Bottom == 0)
                return null;

            caretPosition.X = guiInfo.RcCaret.Left + 25;
            caretPosition.Y = guiInfo.RcCaret.Bottom + 25;

            Win32Api.ClientToScreen(guiInfo.HWndCaret, out caretPosition);

            return caretPosition;
        }

        private static Win32Api.W32Rect GetMonitor(Win32Api.W32Point position)
        {
            IntPtr monitorHandle = Win32Api.MonitorFromPoint(position, MONITOR_DEFAULT_NEAREST); //0x00000002: return nearest monitor if pt is not contained in any monitor
            Win32Api.W32MonitorInfo monitorInfo = new Win32Api.W32MonitorInfo
            {
                Size = Marshal.SizeOf(typeof(Win32Api.W32MonitorInfo))
            };

            if (!Win32Api.GetMonitorInfo(monitorHandle, ref monitorInfo))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            return monitorInfo.WorkArea;
        }

        private static void ComputeTopLeft(ref Window window, Point? caretPosition) //TODO: don't use ref
        {
            Win32Api.W32Rect monitor;
            double top;
            double left;

            //if caret position can be found open the window there, if not open it at the mouse cursor //TODO: Define in settings where to open?
            if (caretPosition != null)
            {
                monitor = GetMonitor(new Win32Api.W32Point() {X = (int) caretPosition.Value.X, Y = (int) caretPosition.Value.Y});
                top = caretPosition.Value.Y;
                left = caretPosition.Value.X;
            }
            else
            {
                Win32Api.W32Point cursorPosition = new Win32Api.W32Point();
                if (!Win32Api.GetCursorPos(ref cursorPosition))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                monitor = GetMonitor(cursorPosition);

                double offsetX = Math.Round(window.Width / 2);
                double offsetY = Math.Round(window.Height / 2);

                top = cursorPosition.Y - offsetY;
                left = cursorPosition.X - offsetX;
            }

            Rect screen = new Rect(new Point(monitor.Left, monitor.Top), new Point(monitor.Right, monitor.Bottom));
            Rect wnd = new Rect(new Point(left, top), new Point(left + window.Width, top + window.Height));

            window.Top = wnd.Top;
            window.Left = wnd.Left;

            if (!screen.Contains(wnd))
            {
                if (wnd.Top < screen.Top)
                {
                    double diff = Math.Abs(screen.Top - wnd.Top);
                    window.Top = wnd.Top + diff;
                }

                if (wnd.Bottom > screen.Bottom)
                {
                    double diff = wnd.Bottom - screen.Bottom;
                    window.Top = wnd.Top - diff;
                }

                if (wnd.Left < screen.Left)
                {
                    double diff = Math.Abs(screen.Left - wnd.Left);
                    window.Left = wnd.Left + diff;
                }

                if (wnd.Right > screen.Right)
                {
                    double diff = wnd.Right - screen.Right;
                    window.Left = wnd.Left - diff;
                }
            }
        }
    }
}