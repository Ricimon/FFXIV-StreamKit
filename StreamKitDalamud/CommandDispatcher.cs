using Dalamud.Game.Command;
using StreamKitDalamud.UI.Presenter;
using System;

namespace StreamKitDalamud
{
    public class CommandDispatcher : IDalamudHook
    {
        private const string commandName = "/streamkit";
        private const string debugCommandName = "/streamkitdebug";

        private readonly CommandManager commandManager;
        private readonly MainWindowPresenter mainWindowPresenter;
        private readonly DebugWindowPresenter debugWindowPresenter;

        public CommandDispatcher(
            CommandManager commandManager,
            MainWindowPresenter mainWindowPresenter,
            DebugWindowPresenter debugWindowPresenter)
        {
            this.commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            this.mainWindowPresenter = mainWindowPresenter ?? throw new ArgumentNullException(nameof(mainWindowPresenter));
            this.debugWindowPresenter = debugWindowPresenter ?? throw new ArgumentNullException(nameof(debugWindowPresenter));
        }

        public void Dispose()
        {
            this.commandManager.RemoveHandler(commandName);
            this.commandManager.RemoveHandler(debugCommandName);
        }

        public void HookToDalamud()
        {
            this.commandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the StreamKit configuration window"
            });
#if DEBUG
            this.commandManager.AddHandler(debugCommandName, new CommandInfo(OnDebugCommand)
            {
                HelpMessage = "Debug commands for StreamKit"
            });
#endif
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            ShowMainWindow();
        }

        private void OnDebugCommand(string command, string args)
        {
            this.debugWindowPresenter.View.Visible = true;
        }

        private void ShowMainWindow()
        {
            this.mainWindowPresenter.View.Visible = true;
        }
    }
}
