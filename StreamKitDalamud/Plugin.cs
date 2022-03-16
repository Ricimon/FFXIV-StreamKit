using Dalamud.Plugin;
using OBSWebsocketDotNet;
using StreamKit;
using StreamKit.Log;
using System;
using System.Collections.Generic;

namespace StreamKitDalamud
{
    public class Plugin
    {
        private readonly DalamudPluginInterface pluginInterface;

        private readonly IEnumerable<IDalamudHook> dalamudHooks;
        private readonly ObsConfiguration obsConfiguration;
        private readonly StreamKitController streamKitController;

        private readonly ILogger logger;

        public Plugin(
            DalamudPluginInterface pluginInterface,
            IEnumerable<IDalamudHook> dalamudHooks,
            ObsConfiguration obsConfiguration,
            StreamKitController streamKitController,
            ILogger logger)
        {
            this.pluginInterface = pluginInterface ?? throw new ArgumentNullException(nameof(pluginInterface));

            this.dalamudHooks = dalamudHooks ?? throw new ArgumentNullException(nameof(dalamudHooks));
            this.obsConfiguration = obsConfiguration ?? throw new ArgumentNullException(nameof(obsConfiguration));
            this.streamKitController = streamKitController ?? throw new NullReferenceException(nameof(streamKitController));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}
