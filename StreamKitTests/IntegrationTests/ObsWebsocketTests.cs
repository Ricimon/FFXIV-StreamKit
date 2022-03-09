using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OBSWebsocketDotNet;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace StreamKitTests.IntegrationTests
{
    // OBS with the obs-websocket plugin must be running for these tests to pass.
    public class ObsWebsocketTests
    {
        private IConfiguration Configuration { get; set; }

        private string ObsWebsocketUrl => TestContext.Parameters["ObsWebsocketUrl"];
        private string ObsWebsocket4Password => Configuration["ObsWebsocket4Password"];

        public ObsWebsocketTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ObsWebsocketTests>();
            Configuration = builder.Build();
        }

        [Test]
        public async Task ConnectToObsWebsocket_ShouldWork()
        {
            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new(ObsWebsocketUrl), CancellationToken.None);
            Assert.AreEqual(WebSocketState.Open, ws.State);
        }

        [Test]
        public void ObsWebsocketDotnetConnection_ShouldWork()
        {
            var obs = new OBSWebsocket();
            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);
        }

        [Test]
        public async Task Testing()
        {
        }
    }
}
