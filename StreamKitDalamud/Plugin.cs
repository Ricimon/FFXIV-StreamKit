using Dalamud.Game.Command;
using Dalamud.Plugin;
using OBSWebsocketDotNet;
using StreamKit;
using StreamKit.Log;
using StreamKitDalamud.UI.Presenter;
using System;
using System.Collections.Generic;

namespace StreamKitDalamud
{
    public class Plugin : IDalamudHook
    {
        private const string commandName = "/streamkit";
        private const string debugCommandName = "/streamkitdebug";

        private readonly DalamudPluginInterface pluginInterface;
        private readonly CommandManager commandManager;

        private readonly IEnumerable<IDalamudHook> dalamudHooks;
        private readonly ObsConfiguration obsConfiguration;
        private readonly StreamKitController streamKitController;

        private readonly MainWindowPresenter mainWindowPresenter;
        private readonly DebugWindowPresenter debugWindowPresenter;

        private readonly ILogger logger;

        public Plugin(
            DalamudPluginInterface pluginInterface,
            CommandManager commandManager,
            IEnumerable<IDalamudHook> dalamudHooks,
            ObsConfiguration obsConfiguration,
            StreamKitController streamKitController,
            MainWindowPresenter mainWindowPresenter,
            DebugWindowPresenter debugWindowPresenter,
            ILogger logger)
        {
            this.pluginInterface = pluginInterface ?? throw new ArgumentNullException(nameof(pluginInterface));
            this.commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));

            this.dalamudHooks = dalamudHooks ?? throw new ArgumentNullException(nameof(dalamudHooks));
            this.obsConfiguration = obsConfiguration ?? throw new ArgumentNullException(nameof(obsConfiguration));
            this.streamKitController = streamKitController ?? throw new NullReferenceException(nameof(streamKitController));

            this.mainWindowPresenter = mainWindowPresenter ?? throw new ArgumentNullException(nameof(mainWindowPresenter));
            this.debugWindowPresenter = debugWindowPresenter ?? throw new ArgumentNullException(nameof(debugWindowPresenter));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            this.commandManager.RemoveHandler(commandName);
            this.commandManager.RemoveHandler(debugCommandName);

            this.pluginInterface.UiBuilder.OpenConfigUi -= ShowMainWindow;
        }

        public void Initialize()
        {
            foreach(var dalamudHook in this.dalamudHooks)
            {
                dalamudHook.HookToDalamud();
            }

            this.streamKitController.SetupBindings();

            logger.Info("StreamKit initialized");

            if (this.obsConfiguration.ConnectOnStartup)
            {
                try
                {
                    this.streamKitController.ConnectToObs(
                        this.obsConfiguration.ObsWebsocketUrl,
                        this.obsConfiguration.ObsWebsocketPassword);
                }
                catch (AuthFailureException) { }
                catch (ErrorResponseException) { }
            }
        }

        public void HookToDalamud()
        {
            this.commandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the StreamKit configuration window"
            });
            this.commandManager.AddHandler(debugCommandName, new CommandInfo(OnDebugCommand)
            {
                HelpMessage = "Debug commands for StreamKit"
            });

            this.pluginInterface.UiBuilder.OpenConfigUi += ShowMainWindow;
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
