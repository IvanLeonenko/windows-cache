using System;

namespace Framework.Cache.WindowsStore
{
    public class DebugCacheLogger : ILogger
    {
        public void Info(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Info(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Debug(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

        public void Warning(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Warning(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

        public void Warning(string message, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(message + exception.Message);
        }

        public void Error(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Error(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

        public void Error(string message, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(message + exception.Message);
        }
    }
}
