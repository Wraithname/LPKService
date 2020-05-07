using System;

namespace Logger
{
    interface ILogger
    {
        void Trace(string text);
        void Trace(string tag, string text);
        void Debug(string text);
        void Debug(string tag, string text);
        void Info(string text);
        void Info(string tag, string text);
        void Warning(string text);
        void Warning(string tag, string text);
        void Error(string text);
        void Error(string tag, string text);
        void Error(string text, Exception e);
        void Error(string tag, string text, Exception e);
        void Fatal(string text);
        void Fatal(string tag, string text);
    }
}
