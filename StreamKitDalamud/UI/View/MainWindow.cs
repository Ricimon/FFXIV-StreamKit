using ImGuiNET;
using Reactive.Bindings;
using StreamKit.Log;
using System;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace StreamKitDalamud.UI.View
{
    public class MainWindow : IMainWindow
    {
        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get => this.visible;
            set => this.visible = value;
        }

        public bool ObsConnected { get; set; }
        public bool ShowObsAuthError { get; set; }
        public bool IsCharacterAlive { get; set; }

        public IReactiveProperty<string> ObsWebsocketUrl { get; init; }
            = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);
        public IReactiveProperty<string> ObsWebsocketPassword { get; init; }
            = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);
        public IReactiveProperty<bool> ObsConnectOnStartup { get; init; }
            = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        private readonly ISubject<Unit> obsConnectRequest = new Subject<Unit>();
        public IObservable<Unit> ObsConnectRequest => obsConnectRequest.AsObservable();

        public IReactiveProperty<string> AliveStateSourceName { get; init; }
            = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);
        public IReactiveProperty<string> DeadStateSourceName { get; init; }
            = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);

        private readonly ISubject<Unit> testAlive = new Subject<Unit>();
        public IObservable<Unit> TestAlive => testAlive.AsObservable();
        private readonly ISubject<Unit> testDead = new Subject<Unit>();
        public IObservable<Unit> TestDead => testDead.AsObservable();

        public IReactiveProperty<bool> PrintLogsToChat { get; init; }
            = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);
        public IReactiveProperty<int> MinimumVisibleLogLevel { get; init; }
            = new ReactiveProperty<int>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public MainWindow() { }

        public void Draw()
        {
            if (!Visible)
            {
                return;
            }

            var height = 365;
            ImGui.SetNextWindowSize(new Vector2(400, height), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(400, height), new Vector2(float.MaxValue, height));
            if (ImGui.Begin("StreamKit", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                DrawContents();
            }
            ImGui.End();
        }

        private void DrawContents()
        {
            ImGui.Text("OBS Websocket"); // ---------------

            // Connectivity indicator
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            var h = ImGui.GetTextLineHeightWithSpacing() / 2f;
            pos += new Vector2(ImGui.GetWindowSize().X - 110, -h);
            var radius = 0.6f * h;
            var color = this.ObsConnected ? Vector4Colors.Green : Vector4Colors.Red;
            drawList.AddCircleFilled(pos, radius, ImGui.ColorConvertFloat4ToU32(color));
            pos += new Vector2(radius + 3, -h);
            ImGui.SetCursorScreenPos(pos);
            ImGui.Text(this.ObsConnected ? "Connected" : "Not connected");

            var indent = 10;
            ImGui.Indent(indent);

            if (ImGui.BeginTable("ObsWebsocket", 2))
            {
                ImGui.TableSetupColumn("ObsWebsocketCol1", ImGuiTableColumnFlags.WidthFixed, 65);
                ImGui.TableNextRow(); ImGui.TableNextColumn();
                var rightPad = 110;
                var obsWebsocketUrl = this.ObsWebsocketUrl.Value;
                ImGui.Text("IP:PORT"); ImGui.TableNextColumn();
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth() - rightPad);
                var inputFieldFlags = !this.ObsConnected ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly;
                ImGuiExtensions.SetDisabled(this.ObsConnected);
                if (ImGui.InputText("##IP:PORT", ref obsWebsocketUrl, 100, inputFieldFlags))
                {
                    this.ObsWebsocketUrl.Value = obsWebsocketUrl;
                }
                ImGuiExtensions.SetDisabled(false);

                ImGui.TableNextRow(); ImGui.TableNextColumn();
                var obsWebsocketPassword = this.ObsWebsocketPassword.Value;
                ImGui.Text("Password"); ImGui.TableNextColumn();
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth() - rightPad);
                ImGuiExtensions.SetDisabled(this.ObsConnected);
                if (ImGui.InputText("##Password", ref obsWebsocketPassword, 100, ImGuiInputTextFlags.Password | inputFieldFlags))
                {
                    this.ObsWebsocketPassword.Value = obsWebsocketPassword;
                }
                ImGuiExtensions.SetDisabled(false);
                ImGui.EndTable();
            }
            var obsConnectOnStartup = this.ObsConnectOnStartup.Value;
            if (ImGui.Checkbox("Connect on startup", ref obsConnectOnStartup))
            {
                this.ObsConnectOnStartup.Value = obsConnectOnStartup;
            }
            if (ImGui.Button(!this.ObsConnected ? "Connect" : "Disconnect"))
            {
                this.obsConnectRequest.OnNext(Unit.Default);
            }
            if (this.ShowObsAuthError)
            {
                ImGui.SameLine();
                ImGui.TextColored(Vector4Colors.Red, "Authentication failed.");
            }

            ImGui.Indent(-indent);
            ImGui.Spacing();

            ImGui.Text("OBS Settings"); // ---------------
            ImGui.Indent(indent);

            if (ImGui.BeginTable("ObsSettings", 2))
            {
                ImGui.TableSetupColumn("ObsSettingsCol1", ImGuiTableColumnFlags.WidthFixed, 150);
                ImGui.TableNextRow(); ImGui.TableNextColumn();
                var rightPad = 30;
                var aliveStateSourceName = this.AliveStateSourceName.Value;
                var aliveStateLabel = "Alive image source name";
                ImGui.Text(aliveStateLabel); ImGui.TableNextColumn();
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth() - rightPad);
                if (ImGui.InputText($"##{aliveStateLabel}", ref aliveStateSourceName, 100))
                {
                    this.AliveStateSourceName.Value = aliveStateSourceName;
                }
                ImGui.TableNextRow(); ImGui.TableNextColumn();
                var deadStateSourceName = this.DeadStateSourceName.Value;
                var deadStateLabel = "Dead image source name";
                ImGui.Text(deadStateLabel); ImGui.TableNextColumn();
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth() - rightPad);
                if (ImGui.InputText($"##{deadStateLabel}", ref deadStateSourceName, 100))
                {
                    this.DeadStateSourceName.Value = deadStateSourceName;
                }
                ImGui.EndTable();
            }

            ImGui.Indent(-indent);
            ImGui.Spacing();

            ImGui.Text("Debug"); // ---------------
            ImGui.Indent(indent);

            ImGui.Text("Character state:");
            ImGui.SameLine();
            ImGui.Text(this.IsCharacterAlive ? "Alive" : "Dead");
            ImGuiExtensions.SetDisabled(!this.ObsConnected);
            if (ImGui.Button("Test Alive"))
            {
                this.testAlive.OnNext(Unit.Default);
            }
            ImGui.SameLine();
            if (ImGui.Button("Test Dead"))
            {
                this.testDead.OnNext(Unit.Default);
            }
            ImGuiExtensions.SetDisabled(false);
            ImGui.SameLine();
            ImGui.Text("(OBS only)");

            ImGui.Indent(-indent);
            ImGui.Spacing();

            if (ImGui.BeginTable("SeparatorLine1", 1, ImGuiTableFlags.BordersInnerH))
            {
                ImGui.TableNextRow(); ImGui.TableNextColumn(); ImGui.Spacing();

                var printLogsToChat = this.PrintLogsToChat.Value;
                if (ImGui.Checkbox("Print logs to chat", ref printLogsToChat))
                {
                    this.PrintLogsToChat.Value = printLogsToChat;
                }

                if (printLogsToChat)
                {
                    ImGui.SameLine();
                    var minLogLevel = this.MinimumVisibleLogLevel.Value;
                    ImGui.SetNextItemWidth(70);
                    if (ImGui.Combo("Min log level",
                        ref minLogLevel,
                        LogLevel.AllLoggingLevels.Select(l => l.Name).ToArray(),
                        LogLevel.AllLoggingLevels.Count()))
                    {
                        this.MinimumVisibleLogLevel.Value = minLogLevel;
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}
