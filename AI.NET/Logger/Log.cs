using log4net;

namespace AI.NET.Logger
{
    internal static class Log
    {
        private static readonly ILog log = LogManager.GetLogger("AI.NET");
        public static void Info(string message, Exception? ex = null)
        {
            if (ex is null)
                log.Info(message);
            else
                log.Info(message, ex);
        }
        public static void Debug(string message, Exception? ex = null)
        {
            if (ex is null)
                log.Debug(message);
            else
                log.Debug(message, ex);
        }
        public static void Warn(string message, Exception? ex = null)
        {
            if (ex is null)
                log.Warn(message);
            else
                log.Warn(message, ex);
        }
        public static void Error(string message, Exception? ex = null)
        {
            if (ex is null)
                log.Error(message);
            else
                log.Error(message, ex);
        }
    }
}
