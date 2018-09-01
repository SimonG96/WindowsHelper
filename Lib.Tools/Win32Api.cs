using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Lib.Tools
{
    public static class Win32Api
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref W32Point pt);


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref W32MonitorInfo lpmi);


        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromPoint(W32Point pt, uint dwFlags);


        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);


        [DllImport("user32.dll")]
        public static extern bool GetCaretPos(out Point lpPoint);


        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);


        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hWnd);


        [DllImport("user32.dll")] //CharSet = CharSet.Auto
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();


        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(UInt32 idAttach, UInt32 idAttachTo, bool fAttach);


        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();


        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out Point position);


        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);


        [DllImport("user32.dll", EntryPoint = "GetGUIThreadInfo")]
        public static extern bool GetGUIThreadInfo(uint tId, out W32GuiThreadInfo threadInfo);



        [StructLayout(LayoutKind.Sequential)]
        public struct W32Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct W32MonitorInfo
        {
            public int Size;
            public W32Rect Monitor;
            public W32Rect WorkArea;
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct W32Rect
        {
            public uint Left;
            public uint Top;
            public uint Right;
            public uint Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct W32GuiThreadInfo
        {
            public uint CbSize;
            public uint Flags;
            public IntPtr HWndActiv;
            public IntPtr HWndFocus;
            public IntPtr HWndCapture;
            public IntPtr HWndMenuOwner;
            public IntPtr HWndMoveSize;
            public IntPtr HWndCaret;
            public W32Rect RcCaret;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct W32MinMaxInfo
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        }

        public static class MessageId
        {
            public const int WM_CLIPBOARDUPDATE = 0x031D;
            public const int WM_GETMINMAXINFO = 0x0024;
            public const int WM_HOTKEY = 0x0312;
            public const int WM_PASTE = 0x0302;
        }

        public static class HotkeyId
        {
            public const int OPEN_HOTKEY_ID = 9000;
            public const int PASTE_HOTKEY_ID = 9001;
        }

        public static class KeyCodes
        {
            #region MOD_Keys

            public const uint MOD_ALT = 0x0001;
            public const uint MOD_CONTROL = 0x0002;
            public const uint MOD_SHIFT = 0x0004;
            public const uint MOD_WIN = 0x0008;

            #endregion MOD_Keys

            #region VK_Keys

            public const uint VK_BACK = 0x08;
            public const uint VK_TAB = 0x09;
            public const uint VK_RETURN = 0x0D;
            public const uint VK_ESCAPE = 0x1B;
            public const uint VK_SPACE = 0x20;
            public const uint VK_PRIOR = 0x21;
            public const uint VK_NEXT = 0x22;
            public const uint VK_END = 0x23;
            public const uint VK_LEFT = 0x25;
            public const uint VK_UP = 0x26;
            public const uint VK_RIGHT = 0x27;
            public const uint VK_DOWN = 0x28;
            public const uint VK_INSERT = 0x2D;
            public const uint VK_DELETE = 0x2E;
            public const uint VK_0 = 0x30;
            public const uint VK_1 = 0x31;
            public const uint VK_2 = 0x32;
            public const uint VK_3 = 0x33;
            public const uint VK_4 = 0x34;
            public const uint VK_5 = 0x35;
            public const uint VK_6 = 0x36;
            public const uint VK_7 = 0x37;
            public const uint VK_8 = 0x38;
            public const uint VK_9 = 0x39;
            public const uint VK_A = 0x41;
            public const uint VK_B = 0x42;
            public const uint VK_C = 0x43;
            public const uint VK_D = 0x44;
            public const uint VK_E = 0x45;
            public const uint VK_F = 0x46;
            public const uint VK_G = 0x47;
            public const uint VK_H = 0x48;
            public const uint VK_I = 0x49;
            public const uint VK_J = 0x4A;
            public const uint VK_K = 0x4B;
            public const uint VK_L = 0x4C;
            public const uint VK_M = 0x4D;
            public const uint VK_N = 0x4E;
            public const uint VK_O = 0x4F;
            public const uint VK_P = 0x50;
            public const uint VK_Q = 0x51;
            public const uint VK_R = 0x52;
            public const uint VK_S = 0x53;
            public const uint VK_T = 0x54;
            public const uint VK_U = 0x55;
            public const uint VK_V = 0x56;
            public const uint VK_W = 0x57;
            public const uint VK_X = 0x58;
            public const uint VK_Y = 0x59;
            public const uint VK_Z = 0x5A;

            #endregion VK_Keys

            #region Methods

            public static uint? GetKeyCodeForKey(Key key)
            {
                var fields = Type.GetType(nameof(KeyCodes))?.GetFields();
                if (fields == null)
                    return null;

                var field = fields.FirstOrDefault(f => f.Name.Equals($"VK_{Enum.GetName(typeof(Key), key)}"));
                if (field == null)
                    return null;

                return (uint?) field.GetValue(key);
            }

            #endregion Methods
        }
    }
}