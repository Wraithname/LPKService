using System;

namespace Logger
{
    public class DatabaseLog:Log
    {
        public DatabaseLog(string defaultTag, LogLevel logLevel = LogLevel.DEBUG) : base(defaultTag, logLevel)
        {
            throw new NotImplementedException();
        }

        public override void Debug(string tag, string text)
        {
            throw new NotImplementedException();
        }

        public override void Error(string tag, string text)
        {
            throw new NotImplementedException();
        }

        public override void Error(string tag, string text, Exception e)
        {
            throw new NotImplementedException();
        }

        public override void Fatal(string tag, string text)
        {
            throw new NotImplementedException();
        }

        public override void Fatal(string tag, string text, Exception e)
        {
            throw new NotImplementedException();
        }

        public override void Info(string tag, string text)
        {
            throw new NotImplementedException();
        }

        public override void Trace(string tag, string text)
        {
            throw new NotImplementedException();
        }

        public override void Warning(string tag, string text)
        {
            throw new NotImplementedException();
        }
    }
}
