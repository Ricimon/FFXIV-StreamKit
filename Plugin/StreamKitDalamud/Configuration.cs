using Dalamud.Configuration;
using Dalamud.Plugin;
using StreamKit;
using StreamKit.Log;
using System;

namespace StreamKitDalamud
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool PrintLogsToChat { get; set; } = true;

        public int MinimumVisibleLogLevel { get; set; } = LogLevel.Info.Ordinal;

        public ObsConfiguration ObsConfiguration { get; set; } = new ObsConfiguration();

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}
