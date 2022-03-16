using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Plugin;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using StreamKit;
using StreamKit.Log;
using StreamKitDalamud.Log;
using StreamKitDalamud.UI;
using StreamKitDalamud.UI.Presenter;
using StreamKitDalamud.UI.View;
using System;

namespace StreamKitDalamud.Ninject
{
    public class PluginModule : NinjectModule
    {
        private readonly DalamudServices dalamudServices;

        public PluginModule(DalamudServices dalamudServices)
        {
            this.dalamudServices = dalamudServices ?? throw new ArgumentNullException(nameof(dalamudServices)); ;
        }

        public override void Load()
        {
            // Dalamud services
            Bind<DalamudPluginInterface>().ToConstant(this.dalamudServices.PluginInterface).InTransientScope();
            Bind<CommandManager>().ToConstant(this.dalamudServices.CommandManager).InTransientScope();
            Bind<ChatGui>().ToConstant(this.dalamudServices.ChatGui).InTransientScope();
            Bind<ClientState>().ToConstant(this.dalamudServices.ClientState).InTransientScope();
            Bind<Condition>().ToConstant(this.dalamudServices.Condition).InTransientScope();

            // Plugin classes
            Bind<IDalamudHook, Plugin>().To<Plugin>().InSingletonScope();
            Bind<IDalamudHook>().To<PluginUIContainer>().InSingletonScope();
            Bind<IDalamudHook>().To<ClientStateListener>().InSingletonScope();
            Bind<Configuration>().ToMethod(GetConfiguration).InSingletonScope();
            Bind<ObsConfiguration>().ToConstant(this.Kernel.Get<Configuration>().ObsConfiguration);

            // Views and Presenters
            Bind<IPluginUIView, IMainWindow>().To<MainWindow>();
            Bind<IPluginUIPresenter, MainWindowPresenter>().To<MainWindowPresenter>().InSingletonScope();
            Bind<IPluginUIView, DebugWindow>().To<DebugWindow>().InSingletonScope();
            Bind<IPluginUIPresenter, DebugWindowPresenter>().To<DebugWindowPresenter>().InSingletonScope();

            Bind<ILogger>().To<XivChatLogger>();
        }

        private Configuration GetConfiguration(IContext context)
        {
            var configuration = 
                this.dalamudServices.PluginInterface.GetPluginConfig() as Configuration
                ?? new Configuration();
            configuration.Initialize(this.dalamudServices.PluginInterface);
            return configuration;
        }
    }
}
