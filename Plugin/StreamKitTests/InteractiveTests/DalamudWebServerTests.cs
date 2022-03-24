#if NET5_0
using NUnit.Framework;
using StreamKitDalamud.Web;

namespace StreamKitTests.InteractiveTests
{
    public class DalamudWebServerTests
    {
        [Test]
        public void StartingLocalWebserver_ShouldWorkAndBlock()
        {
            var webServer = new WebServer();
            webServer.Start();
        }
    }
}
#endif
