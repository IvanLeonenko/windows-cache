
using System;

namespace Framework.Cache
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string format, params object[] args);

        void Debug(string message);
        void Debug(string format, params object[] args);

        void Warning(string message);
        void Warning(string format, params object[] args);
        void Warning(string message, Exception exception);

        void Error(string message);
        void Error(string format, params object[] args);
        void Error(string message, Exception exception);
        
    }
}
