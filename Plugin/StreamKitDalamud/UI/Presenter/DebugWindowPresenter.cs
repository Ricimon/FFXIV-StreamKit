using StreamKitDalamud.UI.View;
using System;

namespace StreamKitDalamud.UI.Presenter
{
    public class DebugWindowPresenter : IPluginUIPresenter
    {
        public IPluginUIView View { get; init; }

        // There's supposed to be an IDebugWindow, but it's a debug window so whatever
        public DebugWindowPresenter(DebugWindow view)
        {
            this.View = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void SetupBindings()
        {

        }
    }
}
