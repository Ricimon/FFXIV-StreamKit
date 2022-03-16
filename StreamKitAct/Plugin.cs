using Advanced_Combat_Tracker;
using System;
using System.Windows.Forms;

namespace StreamKitAct
{
    public class Plugin : IActPluginV1
    {
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginStatusText.Text = "Ready!";
        }

        public void DeInitPlugin()
        {

        }
    }
}
