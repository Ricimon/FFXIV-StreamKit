using Dalamud.IoC;
using Dalamud.Plugin;
using Ninject;
using StreamKit.Ninject;
using StreamKitDalamud.Ninject;

namespace StreamKitDalamud
{
    public sealed class PluginInitializer : IDalamudPlugin
    {
        public string Name => "StreamKit";

        private readonly IKernel kernel;

        public PluginInitializer(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            var services = pluginInterface.Create<DalamudServices>();

            this.kernel = new StandardKernel(
                new PluginModule(services!),
                new StreamKitModule());

            // Entrypoint
            this.kernel.Get<Plugin>().Initialize();
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }
}
