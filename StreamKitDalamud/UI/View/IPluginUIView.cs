﻿namespace StreamKitDalamud.UI.View
{
    public interface IPluginUIView
    {
        public bool Visible { get; set; }

        public void Draw();
    }
}
