using Reactive.Bindings;
using System;
using System.Reactive;

namespace StreamKitDalamud.UI.View
{
    public interface IMainWindow : IPluginUIView
    {
        public bool ObsConnected { get; set; }
        public bool ShowObsAuthError { get; set; }
        public bool IsCharacterAlive { get; set; }

        public IReactiveProperty<string> ObsWebsocketUrl { get; }
        public IReactiveProperty<string> ObsWebsocketPassword { get; }
        public IReactiveProperty<bool> ObsConnectOnStartup { get; }

        public IObservable<Unit> ObsConnectRequest { get; }

        public IReactiveProperty<string> AliveStateSourceName { get; }
        public IReactiveProperty<string> DeadStateSourceName { get; }

        public IObservable<Unit> TestAlive { get; }
        public IObservable<Unit> TestDead { get; }

        public IReactiveProperty<bool> PrintLogsToChat { get; }
        public IReactiveProperty<int> MinimumVisibleLogLevel { get; }
    }
}
