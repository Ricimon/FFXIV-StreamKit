using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace StreamKitDalamud
{
    public class DalamudServices
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [PluginService] public DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] public CommandManager CommandManager { get; private set; }
        [PluginService] public ClientState ClientState { get; private set; }
        [PluginService] public ChatGui ChatGui { get; private set; }
        [PluginService] public Condition Condition { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
