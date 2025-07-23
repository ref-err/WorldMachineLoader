using System;

namespace WorldMachineLoader.API.Utils
{
    public class Logger
    {
        private string LoggerName { get; }
        
        /// <summary>
        /// Specifies the log level.
        /// </summary>
        public enum LogLevel
        {
            Info,
            Warn,
            Error
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="loggerName">The name of a logger.</param>
        public Logger(string loggerName)
        {
            LoggerName = loggerName;
        }

        /// <summary>
        /// Logs the specified message with the specified log level type.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The log level.</param>
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timestamp} [{level.ToString().ToUpper()}] [{LoggerName}] {message}";

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
