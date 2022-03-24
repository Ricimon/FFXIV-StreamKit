using StreamKitDalamud.UI.View;

namespace StreamKitDalamud.UI.Presenter
{
    public interface IPluginUIPresenter
    {
        IPluginUIView View { get; }

        void SetupBindings();
    }
}
