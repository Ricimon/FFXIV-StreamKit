using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using StreamKit.Log;

namespace StreamKitDalamud.Log
{
    public class XivChatLogger : ILogger
    {
        private readonly ChatGui chatGui;
        private readonly Configuration configuration;

        public XivChatLogger(ChatGui chatGui, Configuration configuration)
        {
            this.chatGui = chatGui;
            this.configuration = configuration;
        }

        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void Fatal(string message)
        {
            Log(LogLevel.Fatal, message);
        }

        private void Log(LogLevel logLevel, string message)
        {
            if (!this.configuration.PrintLogsToChat)
            {
                return;
            }

            if (logLevel.Ordinal < this.configuration.MinimumVisibleLogLevel)
            {
                return;
            }

            XivChatType chatType;

            if (logLevel.Ordinal <= LogLevel.Warn.Ordinal)
            {
                chatType = XivChatType.Debug;
            }
            else
            {
                chatType = XivChatType.ErrorMessage;
            }

            this.chatGui.PrintChat(new XivChatEntry
            {
                Message = $"{logLevel} | {message}",
                Type = chatType
            });
        }
    }
}
