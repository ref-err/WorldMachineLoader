using System;

namespace WorldMachineLoader.Utils
{
    public static class Logger
    {
        private static readonly string loggerName = "WML";
        
        public enum LogLevel
        {
            Info,
            Warn,
            Error
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timestamp} [{level.ToString().ToUpper()}] [{loggerName}] {message}";

            ConsoleColor ogColor = Console.ForegroundColor;
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(logMessage);
            Console.ForegroundColor = ogColor;
        }
    }
}
