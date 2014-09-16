using NLog;
using System;

namespace Framework.Cache.Desktop
{
    public class NLogCacheLogger : ILogger
    {
        private readonly Logger _logger;
        
        public NLogCacheLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
