using OBSWebsocketDotNet;
using StreamKit.Extensions;
using StreamKit.Log;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace StreamKit
{
    public class StreamKitController : IDisposable
    {
        private readonly Character character;
        private readonly OBSWebsocket obs;
        private readonly ObsConfiguration obsConfiguration;
        private readonly ILogger logger;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public StreamKitController(
            Character character,
            OBSWebsocket obs,
            ObsConfiguration obsConfiguration,
            ILogger logger)
        {
            this.character = character ?? throw new ArgumentNullException(nameof(character));
            this.obs = obs ?? throw new ArgumentNullException(nameof(obs));
            this.obsConfiguration = obsConfiguration ?? throw new ArgumentNullException(nameof(obsConfiguration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            this.disposables.Dispose();
            this.obs.Disconnect();
        }

        public void SetupBindings()
        {
            this.obs.ConnectedAsObservable()
                .Subscribe(_ => this.logger.Info("OBS connected"))
                .DisposeWith(this.disposables);

            this.obs.DisconnectedAsObservable()
                .Subscribe(_ => this.logger.Info("OBS disconnected"))
                .DisposeWith(this.disposables);

            // Bind character state changes to OBS effects
            this.character.IsAliveObservable.Subscribe(SetObsCharacterAliveState);
        }

        /// <summary>
        /// Throws the same exceptions as OBSWebsocket.Connect()
        /// </summary>
        public void ConnectToObs(string url, string password)
        {
            try
            {
                this.obs.Connect(url, password);
            }
            catch (AuthFailureException ex)
            {
                this.logger.Error("OBS authentication failed.");
                throw ex;
            }
            catch (ErrorResponseException ex)
            {
                this.logger.Error("OBS connect failed: " + ex.Message);
                throw ex;
            }
        }

        public void SetObsCharacterAliveState(bool alive)
        {
            if (!this.obs.IsConnected)
            {
                return;
            }

            var sceneName = this.obs.GetCurrentScene().Name;

            void TrySetSourceVisibility(string sourceName, bool state)
            {
                if (this.obs.TryGetSceneItemProperties(
                    sourceName,
                    sceneName,
                    out var source))
                {
                    source.Visible = state;
                    this.obs.SetSceneItemProperties(source, sceneName);
                }
            }

            TrySetSourceVisibility(this.obsConfiguration.AliveStateSourceName, alive);
            TrySetSourceVisibility(this.obsConfiguration.DeadStateSourceName, !alive);
        }
    }
}
