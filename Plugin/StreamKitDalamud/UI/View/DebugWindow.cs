using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using ImGuiNET;
using System;
using System.Numerics;

namespace StreamKitDalamud.UI.View
{
    public class DebugWindow : IPluginUIView
    {
        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get => this.visible;
            set => this.visible = value;
        }

        private readonly ChatGui chatGui;

        // Direct application logic is being placed into this UI script because this is debug UI
        public DebugWindow(ChatGui chatGui)
        {
            this.chatGui = chatGui ?? throw new ArgumentNullException(nameof(chatGui));
        }

        public void Draw()
        {
            if (!Visible)
            {
                return;
            }

            var minWindowSize = new Vector2(375, 330);
            ImGui.SetNextWindowSize(minWindowSize, ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(minWindowSize, new Vector2(float.MaxValue, float.MaxValue));
            if (ImGui.Begin("StreamKit Debug", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                DrawContents();
            }
            ImGui.End();
        }

        private void DrawContents()
        {
            if (ImGui.Button("Echo some text"))
            {
                chatGui.PrintChat(new XivChatEntry
                {
                    Message = "Test",
                    Type = XivChatType.Debug
                });
            }
            if (ImGui.Button("Echo some error text"))
            {
                chatGui.PrintChat(new XivChatEntry
                {
                    Message = "Error",
                    Type = XivChatType.ErrorMessage
                });
            }
        }
    }
}
