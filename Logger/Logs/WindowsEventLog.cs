using System;
using System.Diagnostics;

namespace Logger.Logs
{
    public class WindowsEventLog:Log
    {
        private readonly EventLog eventLog;

        public WindowsEventLog(string tag) : base(tag)
        {
            eventLog = new EventLog(loggerName) { Source = tag };
        }

        public override void Trace(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.TRACE)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry($"[TRACE]\n{text}", EventLogEntryType.Information, (int)LogLevel.TRACE);
        }

        public override void Debug(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.DEBUG)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry($"[DEBUG]\n{text}", EventLogEntryType.Information, (int)LogLevel.DEBUG);
        }

        public override void Info(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.INFO)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry(text, EventLogEntryType.Information, (int)LogLevel.INFO);
        }

        public override void Warning(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.WARNING)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry(text, EventLogEntryType.Warning, (int)LogLevel.WARNING);
        }


        public override void Error(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.ERROR)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry(text, EventLogEntryType.Error, (int)LogLevel.FATAL);
        }

        public override void Error(string tag, string text, Exception e)
        {
            if (!IsEnableLogLevel(LogLevel.ERROR)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry($"{text}\n{e.Message}", EventLogEntryType.Error, (int)LogLevel.ERROR);
        }

        public override void Fatal(string tag, string text)
        {
            if (!IsEnableLogLevel(LogLevel.FATAL)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry($"[FATAL]\n{text}", EventLogEntryType.Error, (int)LogLevel.ERROR);
        }

        public override void Fatal(string tag, string text, Exception e)
        {
            if (!IsEnableLogLevel(LogLevel.FATAL)) return;
            eventLog.Source = tag;
            eventLog.WriteEntry($"[FATAL]\n{text}\n{e.Message}", EventLogEntryType.Error, (int)LogLevel.ERROR);
        }
    }
}
