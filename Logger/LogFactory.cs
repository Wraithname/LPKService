using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Logger
{
    public class LogFactory
    {
        private static Dictionary<string, Log> loggers = new Dictionary<string, Log>();
        private const string DEFAULT_LINE_FORMAT = "[{datetime|'yyyy.MM.dd HH:mm:ss'}] [{loglevel|*7}] [{method}] {message}";
        private const string DEFAULT_LOG_EXT = ".log";
        private const LogLevel DEFAULT_LOG_LEVEL = LogLevel.INFO;
        private const string DEFAULT_LOG_DIR = "logs";
        private const LogType DEFAULT_LOG_TYPE = LogType.File;
        private const RotatePeriod DEFAULT_ROTATE_PERIOD = RotatePeriod.Day;

        /// <summary>
        /// Типы доступных логгеров
        /// <list type="table">
        /// <listheader>
        /// <term>Значение</term>
        /// <description>Описание</description>
        /// </listheader>
        /// <item>
        /// <term>File</term>
        /// <description>Писать лог в один файл</description>
        /// </item>
        /// <item>
        /// <term>RotateFile</term>
        /// <description>Лог файл будет изменяться согласно настройкам</description>
        /// </item>
        /// <item>
        /// <term>WindowsEvents</term>
        /// <description>Логгирование в журнал событий Windows</description>
        /// </item>
        /// <item>
        /// <term>Database</term>
        /// <description>Запись лога в БД</description>
        /// </item>
        /// </list>
        /// </summary>
        public enum LogType { File, RotateFile, WindowsEvents, Database };

        /// <summary>
        /// Воспомогательный метод для получения имени файла лога для файлового логгера
        /// </summary>
        /// <returns>Путь с именем файла и расширением</returns>
        private static string GetDefaultFileName()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Diagnostics.Process.GetCurrentProcess().ProcessName + DEFAULT_LOG_EXT);
        }

        /// <summary>
        /// Воспомогательный метод для получения директории для логов
        /// </summary>
        /// <returns>Путь к папке логов</returns>
        private static string GetDefaultLogDir()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_LOG_DIR);
        }

        /// <summary>
        /// Файловый логгер
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <returns>Экземпляр <seealso cref="FileLog"/></returns>
        public static FileLog MakeFileLogger(string name, LogLevel logLevel)
        {
            string filename = GetDefaultFileName();
            return MakeFileLogger(name, logLevel, filename);
        }

        /// <summary>
        /// Файловый логгер
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="fileName">Имя файла лога</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <returns>Экземпляр <seealso cref="FileLog"/></returns>
        public static FileLog MakeFileLogger(string name, LogLevel logLevel, string filename)
        {
            return MakeFileLogger(name, logLevel, filename, DEFAULT_LINE_FORMAT);
        }

        /// <summary>
        /// Файловый логгер
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <param name="fileName">Имя файла лога</param>
        /// <param name="lineFormat">Формат выводимой строки <seealso cref="FileLog.FormatLine(string, LogLevel, string, int)"/></param>
        /// <returns>Экземпляр <seealso cref="FileLog"/></returns>
        public static FileLog MakeFileLogger(string name, LogLevel logLevel, string fileName, string lineFormat)
        {
            return new FileLog(name, logLevel, fileName, lineFormat);
        }

        /// <summary>
        /// Логгер Windows Events
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <returns>Экземпляр <seealso cref="WindowsEventLog"/></returns>
        public static Logger.Logs.WindowsEventLog MakeEventLog(string name)
        {
            return new Logger.Logs.WindowsEventLog(name) { LogLevel = DEFAULT_LOG_LEVEL };
        }

        /// <summary>
        /// Логгер Windows Events
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <returns>Экземпляр <seealso cref="WindowsEventLog"/></returns>
        public static Logger.Logs.WindowsEventLog MakeEventLog(string name, LogLevel logLevel)
        {
            return new Logger.Logs.WindowsEventLog(name) { LogLevel = logLevel };
        }

        /// <summary>
        /// Логгер для БД
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <returns>Экземпляр <seealso cref="DatabaseLog"/></returns>
        public static DatabaseLog MakeDatabaseLogger(string name)
        {
            return MakeDatabaseLogger(name, DEFAULT_LOG_LEVEL);
        }

        /// <summary>
        /// Логгер для БД
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <returns>Экземпляр <seealso cref="DatabaseLog"/></returns>
        public static DatabaseLog MakeDatabaseLogger(string name, LogLevel logLevel)
        {
            return new DatabaseLog(name) { LogLevel = logLevel };
        }

        /// <summary>
        /// Файловый логгер с ротацией файла лога
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <returns>Экземпляр <seealso cref="RotationFileLog"/></returns>
        public static RotationFileLog MakeRotationFileLogger(string name)
        {
            return MakeRotationFileLogger(name, DEFAULT_LOG_LEVEL, DEFAULT_ROTATE_PERIOD, GetDefaultLogDir(), DEFAULT_LINE_FORMAT);
        }

        /// <summary>
        /// Файловый логгер с ротацией файла лога
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <returns>Экземпляр <seealso cref="RotationFileLog"/></returns>
        public static RotationFileLog MakeRotationFileLogger(string name, LogLevel logLevel)
        {
            return MakeRotationFileLogger(name, logLevel, DEFAULT_ROTATE_PERIOD, GetDefaultLogDir(), DEFAULT_LINE_FORMAT);
        }

        /// <summary>
        /// Файловый логгер с ротацией файла лога
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <param name="rotatePeriod">Период пересоздания файла лога. См.<seealso cref="RotatePeriod"/></param>
        /// <returns>Экземпляр <seealso cref="RotationFileLog"/></returns>
        public static RotationFileLog MakeRotationFileLogger(string name, LogLevel logLevel, RotatePeriod rotatePeriod)
        {
            return MakeRotationFileLogger(name, logLevel, rotatePeriod, GetDefaultLogDir(), DEFAULT_LINE_FORMAT);
        }

        /// <summary>
        /// Файловый логгер с ротацией файла лога
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <param name="rotatePeriod">Период пересоздания файла лога. См.<seealso cref="RotatePeriod"/></param>
        /// <param name="logDir">Папка хранения лог файлов</param>
        /// <returns>Экземпляр <seealso cref="RotationFileLog"/></returns>
        public static RotationFileLog MakeRotationFileLogger(string name, LogLevel logLevel, RotatePeriod rotatePeriod, string logDir)
        {
            return MakeRotationFileLogger(name, logLevel, rotatePeriod, logDir, DEFAULT_LINE_FORMAT);
        }

        /// <summary>
        /// Файловый логгер с ротацией файла лога
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования. См.<seealso cref="LogLevel"/></param>
        /// <param name="rotatePeriod">Период пересоздания файла лога. См.<seealso cref="RotatePeriod"/></param>
        /// <param name="logDir">Папка хранения лог файлов</param>
        /// <param name="lineFormat">Формат строки. См.<seealso cref="FileLog.FormatLine(string, LogLevel, string, int)"/></param>
        /// <returns>Экземпляр <seealso cref="RotationFileLog"/></returns>
        public static RotationFileLog MakeRotationFileLogger(string name, LogLevel logLevel, RotatePeriod rotatePeriod, string logDir, string lineFormat)
        {
            return new RotationFileLog(name, logLevel, rotatePeriod, logDir, lineFormat, System.Diagnostics.Process.GetCurrentProcess().ProcessName, DEFAULT_LOG_EXT);
        }

        /// <summary>
        /// Ищет логгер по переданному имени <paramref name="name"/> или создает новый
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <returns><seealso cref="Log"/> - экземпляр логгера</returns>
        public static Log GetLogger(string name)
        {
            if (!loggers.TryGetValue(name, out Log log))
            {
                var config = ConfigurationSettings.AppSettings;
                Enum.TryParse(config.Get("LogLevel") ?? $"{DEFAULT_LOG_LEVEL}", out LogLevel logLevel);
                Enum.TryParse(config.Get("LogType") ?? $"{DEFAULT_LOG_TYPE}", out LogType logType);
                string format = config.Get("LogFormat") ?? DEFAULT_LINE_FORMAT;
                string filename = config.Get("LogFile") ?? GetDefaultFileName();
                string logDir = config.Get("LogDir") ?? GetDefaultLogDir();
                Enum.TryParse(config.Get("RotatePeriod") ?? $"{DEFAULT_ROTATE_PERIOD}", out RotatePeriod rotatePeriod);
                switch (logType)
                {
                    case LogType.RotateFile:
                        log = MakeRotationFileLogger(name, logLevel, rotatePeriod, logDir, format);
                        break;
                    case LogType.WindowsEvents:
                        log = MakeEventLog(name, logLevel);
                        break;
                    case LogType.Database:
                        log = MakeDatabaseLogger(name, logLevel);
                        break;
                    case LogType.File:
                    default:
                        log = MakeFileLogger(name, logLevel, filename, format);
                        break;
                }
                loggers.Add(name, log);
            }
            return log;
        }
    }
}
