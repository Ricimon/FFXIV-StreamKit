using System;

namespace StreamKit
{
    [Serializable]
    public class ObsConfiguration
    {
        public string ObsWebsocketUrl { get; set; } = "ws://localhost:4444";
        public string ObsWebsocketPassword { get; set; }
        public bool ConnectOnStartup { get; set; }
        public string AliveStateSourceName { get; set; }
        public string DeadStateSourceName { get; set; }
    }
}
