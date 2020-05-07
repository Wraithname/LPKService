using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    [System.Flags]
    public enum LogLevel
    {
        TRACE,
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL,
        NOLOG
    }
    public abstract class Log
    {

        /// <summary>
        /// Имя логгера
        /// </summary>
        protected readonly string loggerName;
        /// <summary>
        ///     <para>Уровень логгирования</para>
        ///     <see cref="Logger.LogLevel"/>
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Базовый конструктор для реализации различных типов логов
        /// </summary>
        /// <param name="name">Тег по умолчанию</param>
        /// <param name="logLevel">Разрешенный уровень логгирования</param>
        public Log(string name, LogLevel logLevel = LogLevel.DEBUG)
        {
            LogLevel = logLevel;
            loggerName = name;
        }

        /// <summary>
        /// Проверяет разрешен ли указанный уровень логгирования для вывода
        /// </summary>
        /// <param name="logLevel">Уровень логгирования. См. <seealso cref="Logger.LogLevel"/></param>
        /// <returns>True если уровень логгирования больше или равен разрешенному уровню</returns>
        public bool IsEnableLogLevel(LogLevel logLevel)
        {
            return logLevel >= LogLevel;
        }
        /// <summary>
        /// Трассировка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Trace(string text)
        {
            Trace(loggerName, text);
        }
        /// <summary>
        /// Трассировка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Trace(string tag, string text);
        /// <summary>
        /// Отладка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Debug(string text)
        {
            Debug(loggerName, text);
        }
        /// <summary>
        /// Отладка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Debug(string tag, string text);
        /// <summary>
        /// Информация
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Info(string text)
        {
            Info(loggerName, text);
        }
        /// <summary>
        /// Информация
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Info(string tag, string text);
        /// <summary>
        /// Предупреждение
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Warning(string text)
        {
            Warning(loggerName, text);
        }
        /// <summary>
        /// Предупреждение
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Warning(string tag, string text);
        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Error(string text)
        {
            Error(loggerName, text);
        }
        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Error(string tag, string text);
        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        /// <param name="e">Исключение приведшее к ошибке</param>
        public void Error(string text, Exception e)
        {
            Error(loggerName, text, e);
        }
        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        /// <param name="e">Исключение приведшее к ошибке</param>
        public abstract void Error(string tag, string text, Exception e);
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        public void Fatal(string text)
        {
            Fatal(loggerName, text);
        }
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        public abstract void Fatal(string tag, string text);
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        /// <param name="text">Выводимое сообщение</param>
        /// <param name="e">Исключение приведшее к ошибке</param>
        public void Fatal(string text, Exception e)
        {
            Fatal(loggerName, text, e);
        }
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        /// <param name="tag">Тег сообщения</param>
        /// <param name="text">Выводимое сообщение</param>
        /// <param name="e">Исключение приведшее к ошибке</param>
        public abstract void Fatal(string tag, string text, Exception e);
    }
}
