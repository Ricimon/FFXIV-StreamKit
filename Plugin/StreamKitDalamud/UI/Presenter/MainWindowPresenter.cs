using OBSWebsocketDotNet;
using Reactive.Bindings;
using StreamKit;
using StreamKit.Extensions;
using StreamKit.Log;
using StreamKitDalamud.UI.View;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace StreamKitDalamud.UI.Presenter
{
    public class MainWindowPresenter : IPluginUIPresenter, IDisposable
    {
        public IPluginUIView View => this.view;

        private readonly IMainWindow view;
        private readonly Configuration configuration;
        private readonly OBSWebsocket obs;
        private readonly Character character;
        private readonly StreamKitController streamKitController;
        private readonly ILogger logger;

        private readonly CompositeDisposable disposables = new();

        public MainWindowPresenter(
            IMainWindow view,
            Configuration configuration,
            OBSWebsocket obs,
            Character character,
            StreamKitController streamKitController,
            ILogger logger)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.obs = obs ?? throw new ArgumentNullException(nameof(obs));
            this.character = character ?? throw new ArgumentNullException(nameof(character));
            this.streamKitController = streamKitController ?? throw new ArgumentNullException(nameof(streamKitController));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            this.disposables.Dispose();
        }

        public void SetupBindings()
        {
            BindVariables();
            BindActions();
        }

        private void BindVariables()
        {
            this.obs.ConnectedAsObservable()
                .Subscribe(_ => this.view.ObsConnected = true)
                .DisposeWith(this.disposables);

            this.obs.DisconnectedAsObservable()
                .Subscribe(_ => this.view.ObsConnected = false)
                .DisposeWith(this.disposables);

            var obsConfiguration = this.configuration.ObsConfiguration;

            Bind(this.view.ObsWebsocketUrl,
                s => obsConfiguration.ObsWebsocketUrl = s, obsConfiguration.ObsWebsocketUrl);
            Bind(this.view.ObsWebsocketPassword,
                s => obsConfiguration.ObsWebsocketPassword = s, obsConfiguration.ObsWebsocketPassword);

            this.view.ObsWebsocketUrl.Subscribe(_ => this.view.ShowObsAuthError = false);
            this.view.ObsWebsocketPassword.Subscribe(_ => this.view.ShowObsAuthError = false);

            Bind(this.view.ObsConnectOnStartup,
                b => obsConfiguration.ConnectOnStartup = b, obsConfiguration.ConnectOnStartup);

            Bind(this.view.AliveStateSourceName,
                s => obsConfiguration.AliveStateSourceName = s, obsConfiguration.AliveStateSourceName);
            Bind(this.view.DeadStateSourceName,
                s => obsConfiguration.DeadStateSourceName = s, obsConfiguration.DeadStateSourceName);

            this.character.IsAliveObservable.Subscribe(alive => this.view.IsCharacterAlive = alive)
                .DisposeWith(this.disposables);

            Bind(this.view.PrintLogsToChat,
                b => this.configuration.PrintLogsToChat = b, this.configuration.PrintLogsToChat);
            Bind(this.view.MinimumVisibleLogLevel,
                i => this.configuration.MinimumVisibleLogLevel = i, this.configuration.MinimumVisibleLogLevel);
        }

        private void BindActions()
        {
            this.view.ObsConnectRequest.Subscribe(_ =>
            {
                this.view.ShowObsAuthError = false;
                if (!this.obs.IsConnected)
                {
                    try
                    {
                        this.streamKitController.ConnectToObs(
                            this.view.ObsWebsocketUrl.Value,
                            this.view.ObsWebsocketPassword.Value);
                    }
                    catch (AuthFailureException)
                    {
                        this.view.ShowObsAuthError = true;
                    }
                    catch (ErrorResponseException) { }
                }
                else
                {
                    this.obs.Disconnect();
                }
            });

            this.view.TestAlive.Subscribe(
                _ => this.streamKitController.SetObsCharacterAliveState(true));
            this.view.TestDead.Subscribe(
                _ => this.streamKitController.SetObsCharacterAliveState(false));
        }

        private void Bind<T>(
            IReactiveProperty<T> reactiveProperty,
            Action<T> dataUpdateAction,
            T initialValue)
        {
            if (initialValue != null)
            {
                reactiveProperty.Value = initialValue;
            }
            reactiveProperty.Subscribe(v =>
            {
                dataUpdateAction(v);
                this.configuration.Save();
            });
        }
    }
}
