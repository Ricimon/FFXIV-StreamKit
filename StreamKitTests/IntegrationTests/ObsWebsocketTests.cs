using NUnit.Framework;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace StreamKitTests.IntegrationTests
{
    // OBS with the obs-websocket plugin must be running for these tests to pass.
    public class ObsWebsocketTests
    {
        private string ObsWebsocketUrl => TestContext.Parameters["ObsWebsocketUrl"];

        [Test]
        public async Task ConnectToObsWebsocket_ShouldWork()
        {
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(new Uri(ObsWebsocketUrl), CancellationToken.None);
                Assert.AreEqual(WebSocketState.Open, ws.State);
            }
        }
    }
}
