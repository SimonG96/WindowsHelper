using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lib.Tools
{
    public static class ProcessHelper
    {
        public static string GetActiveProcessName()
        {
            const int nChars = 256;
            //int handle = 0;
            StringBuilder buffer = new StringBuilder(nChars);
            IntPtr handle = Win32Api.GetForegroundWindow();

            if (Win32Api.GetWindowText(handle, buffer, nChars) <= 0) //Active Window has no Title Info
                return String.Empty;

            uint dwCaretId = Win32Api.GetWindowThreadProcessId(handle, out uint lpdwProcessId);
            uint dwCurrentId = (uint) Thread.CurrentThread.ManagedThreadId; //TODO: Why is this done?

            return Process.GetProcessById((int) lpdwProcessId).ProcessName;
        }
    }
}