using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Lib.Tools.Logging
{
    public class Log : IDisposable
    {
        private static readonly string _logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowsHelper", "Logs");
        private static readonly string _logFileName = $"WindowsHelper_Log_{DateTime.Now:yyyy_MM_dd}_{DateTime.Now:hh_mm_ss}.txt";

        private static readonly object _lockObject = new object();
        private static StreamWriter _logWriter;

        private Timer _timer;


        public Log() //TODO: Add possibility to clear log files to settings, add possibility to show log file folder to settings
        {
            Init();
        }


        public static void Write(string text, LogLevel logLevel = LogLevel.Info)
        {
            lock (_lockObject)
            {
                _logWriter?.WriteLine($"{DateTime.Now:u}: [{logLevel}] {text}");
            }
        }

        public static void Write(Exception ex)
        {
            Write(ex.Message, LogLevel.Error);
            Write("");
            Write(ex.StackTrace, LogLevel.Error);
        }

        public static void Write(AggregateException ex)
        {
            Write(ex.Message, LogLevel.Error);
            Write("");
            Write(ex.StackTrace, LogLevel.Error);
            Write("");

            foreach (var innerException in ex.InnerExceptions)
            {
                Write(innerException);
                Write("");
            }
        }

        public static void ClearLogFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(_logFilePath);
            foreach (var file in directory.EnumerateFiles())
            {
                if (file.Name.Equals(_logFileName))
                    continue;

                file.Delete();
            }
        }

        public static void OpenLogFileFolder()
        {
            Process.Start(_logFilePath);
        }


        private void Init()
        {
            Directory.CreateDirectory(_logFilePath);
            _logWriter = new StreamWriter(Path.Combine(_logFilePath, _logFileName), true);
            _timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private void TimerCallback(object state)
        {
            Flush();
        }

        private void Flush()
        {
            lock (_lockObject)
            {
                _logWriter?.Flush();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            Flush();
            _logWriter.Dispose();
        }
    }
}