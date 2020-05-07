using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Logger
{
    public class FileLog: Log
    {
        private static object locker = new Object();
        protected string LoggerFileName { get; set; }
        public string LineFormat { get; set; }

        /// <summary>
        /// Логгирование в файл
        /// </summary>
        /// <param name="name">Имя логгера</param>
        /// <param name="logLevel">Уровень логгирования</param>
        /// <param name="filename">Имя файла лога</param>
        /// <param name="lineFormat">Формат выводимой строки. См.<seealso cref="Log.FormatLine(string, LogLevel, string)"/></param>
        public FileLog(
            string name,
            LogLevel logLevel,
            string filename,
            string lineFormat
        ) : base(name, logLevel)
        {
            LineFormat = lineFormat;
            LoggerFileName = string.IsNullOrEmpty(filename) ? loggerName : filename;
        }
        public string FormatLine(string message, LogLevel logLevel, string format, int callDepth = 1)
        {
            Regex re = new Regex("[{][^}]+}");
            StringBuilder sb = new StringBuilder(format);
            string oldValue = "";
            string newValue = "";
            string[] mods;
            foreach (Match m in re.Matches(format))
            {
                newValue = "";
                oldValue = m.ToString().Trim(new[] { '{', '}' });
                mods = oldValue.Split('|');
                // метка времени
                if (oldValue.StartsWith("datetime"))
                {
                    // формат даты всегда должен идти после параметра
                    foreach (string mod in mods)
                    {
                        if (mod.StartsWith("'") && mod.EndsWith("'"))
                        {
                            newValue = DateTime.Now.ToString(mod.Trim('\''));
                        }
                    }
                    newValue = newValue == "" ? DateTime.Now.ToString() : newValue;
                }
                // само сообщение
                else if (oldValue.StartsWith("message")) newValue = message;
                // уровень логгирования
                else if (oldValue.StartsWith("loglevel")) newValue = logLevel.ToString();
                // вызывающий метод
                else if (oldValue.StartsWith("method"))
                {
                    StackFrame sf = new StackFrame(callDepth, true);
                    newValue = sf.GetMethod().ToString();
                }
                // номер строки откуда вызвано логгирование
                else if (oldValue.StartsWith("linenumber"))
                {
                    StackFrame sf = new StackFrame(callDepth, true);
                    newValue = sf.GetFileLineNumber().ToString();
                }
                // имя файла вызывающего метода
                else if (oldValue.StartsWith("filename"))
                {
                    StackFrame sf = new StackFrame(callDepth, true);
                    newValue = Path.GetFileName(sf.GetFileName());
                }
                // Применяем модиифкаторы
                foreach (string mod in mods)
                {
                    // первый элемент всегда выводимое значение, его не обрабатываем
                    if (mod == mods[0]) continue;
                    // -25 дополнить пробелами слева до 25 символов
                    if (mod.StartsWith("-"))
                    {
                        int.TryParse(mod.Substring(1), out int count);
                        newValue = newValue.PadLeft(count);
                    }
                    // +12 Дополнить пробелами справа до 12 символов
                    else if (mod.StartsWith("+"))
                    {
                        int.TryParse(mod.Substring(1), out int count);
                        newValue = newValue.PadRight(count);
                    }
                    // +10 Дополнить пробелами справа и слева до 10 символов
                    else if (mod.StartsWith("*"))
                    {
                        int.TryParse(mod.Substring(1), out int count);
                        newValue = PadBoth(newValue, count);
                    }
                    // В верхний регистр
                    else if (mod.StartsWith("^"))
                    {
                        newValue = newValue.ToUpper();
                    }
                    // В нижний регистр
                    else if (mod.StartsWith("_"))
                    {
                        newValue = newValue.ToLower();
                    }
                }
                sb.Replace($"{{{oldValue}}}", newValue);
            }
            return sb.ToString();
        }

        private string PadBoth(string source, int length)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);
        }

        protected void WriteLine(string text, bool append = true)
        {
            try
            {
                lock (locker)
                {
                    using (StreamWriter writer = new StreamWriter(LoggerFileName, File.Exists(LoggerFileName), Encoding.UTF8))
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            writer.WriteLine(text);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        protected void WriteFormattedLog(LogLevel level, string text)
        {
            WriteLine(FormatLine(text, level, LineFormat, 4));
        }

        public override void Trace(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.TRACE)) return;
            WriteFormattedLog(LogLevel.TRACE, $"[{tag}] {text}");
        }

        public override void Debug(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.DEBUG)) return;
            WriteFormattedLog(LogLevel.DEBUG, $"[{tag}] {text}");
        }

        public override void Info(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.INFO)) return;
            WriteFormattedLog(LogLevel.INFO, $"[{tag}] {text}");
        }

        public override void Warning(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.WARNING)) return;
            WriteFormattedLog(LogLevel.WARNING, $"[{tag}] {text}");
        }

        public override void Error(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.ERROR)) return;
            WriteFormattedLog(LogLevel.ERROR, $"[{tag}] {text}");
        }

        public override void Error(string tag, string text, Exception e)
        {
            if (!IsEnableLogLevel(LogLevel.ERROR)) return;
            WriteFormattedLog(LogLevel.ERROR, $"[{tag}] {text} ({e.Message}) ");
        }

        public override void Fatal(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.FATAL)) return;
            WriteFormattedLog(LogLevel.FATAL, $"[{tag}] {text}");
        }

        public override void Fatal(string tag, string text, Exception e)
        {
            if (!IsEnableLogLevel(LogLevel.FATAL)) return;
            WriteFormattedLog(LogLevel.FATAL, $"[{tag}] {text} ({e.Message}) ");
        }
    }
}
