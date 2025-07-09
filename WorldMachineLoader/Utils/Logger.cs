using System;

namespace WorldMachineLoader.Utils
{
    public static class Logger
    {
        private const string LoggerName = "WML";

        public enum LogLevel
        {
            Info,
            Warn,
            Error
        }
		//реферр нахера тут так много брейков
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string timestamp = DateTime.Now:yyyy-MM-dd HH:mm:ss;
            string logMessage = $"{timestamp} [{level.ToString().ToUpper()}] [{LoggerName}] {message}";

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            Console.WriteLine(logMessage);
            Console.ForegroundColor = originalColor;
        }

        private static ConsoleColor GetColorForLevel(LogLevel level) => level switch
        {
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warn => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }
}