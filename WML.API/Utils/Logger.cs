using System;

namespace WorldMachineLoader.API.Utils
{
    /// <summary>
    /// Provides logging functionality with different log levels and colored console output.
    /// </summary>
    public class Logger
    {
        private string LoggerName { get; }

        /// <summary>
        /// Specifies the log level.
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Informational messages.
            /// </summary>
            Info,
            /// <summary>
            /// Warning messages.
            /// </summary>
            Warn,
            /// <summary>
            /// Error messages.
            /// </summary>
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
        /// <param name="logLevel">The log level.</param>
        public void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timestamp} [{logLevel.ToString().ToUpper()}] [{LoggerName}] {message}";

            ConsoleColor ogColor = Console.ForegroundColor;
            switch (logLevel)
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
