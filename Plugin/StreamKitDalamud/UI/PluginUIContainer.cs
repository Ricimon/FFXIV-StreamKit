using Dalamud.Plugin;
using StreamKitDalamud.UI.Presenter;
using System;
using System.Collections.Generic;

namespace StreamKitDalamud.UI
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    public class PluginUIContainer : IDalamudHook
    {
        private readonly IPluginUIPresenter[] pluginUIPresenters;
        private readonly MainWindowPresenter mainWindowPresenter;
        private readonly DalamudPluginInterface pluginInterface;

        public PluginUIContainer(
            IPluginUIPresenter[] pluginUIPresenters,
            MainWindowPresenter mainWindowPresenter,
            DalamudPluginInterface pluginInterface)
        {
            this.pluginUIPresenters = pluginUIPresenters ?? throw new ArgumentNullException(nameof(pluginUIPresenters));
            this.mainWindowPresenter = mainWindowPresenter ?? throw new ArgumentNullException(nameof(mainWindowPresenter));
            this.pluginInterface = pluginInterface ?? throw new ArgumentNullException(nameof(pluginInterface));

            foreach (var pluginUIPresenter in this.pluginUIPresenters)
            {
                pluginUIPresenter.SetupBindings();
            }
        }

        public void Dispose()
        {
            this.pluginInterface.UiBuilder.Draw -= Draw;
            this.pluginInterface.UiBuilder.OpenConfigUi -= ShowMainWindow;
        }

        public void HookToDalamud()
        {
            this.pluginInterface.UiBuilder.Draw += Draw;
            this.pluginInterface.UiBuilder.OpenConfigUi += ShowMainWindow;
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            foreach (var pluginUIPresenter in this.pluginUIPresenters)
            {
                pluginUIPresenter.View.Draw();
            }
        }

        private void ShowMainWindow()
        {
            this.mainWindowPresenter.View.Visible = true;
        }
    }
}
