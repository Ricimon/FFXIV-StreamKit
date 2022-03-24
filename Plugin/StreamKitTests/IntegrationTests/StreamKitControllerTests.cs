using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OBSWebsocketDotNet;
using StreamKit;
using StreamKit.Log;

namespace StreamKitTests.IntegrationTests
{
    public class StreamKitControllerTests
    {
        private IConfiguration Configuration { get; set; }

        private string ObsWebsocketUrl => TestContext.Parameters["ObsWebsocketUrl"];
        private string ObsWebsocket4Password => Configuration["ObsWebsocket4Password"];

        public StreamKitControllerTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ObsWebsocketTests>();
            Configuration = builder.Build();
        }

        [Test]
        public void SettingCharacterAliveState_ShouldCreateObsSourceVisibilityChanges()
        {
            var obsConfiguration = new ObsConfiguration
            {
                AliveStateSourceName = "Alive State Testing",
                DeadStateSourceName = "Dead State Testing"
            };
            var character = new Character();
            var obs = new OBSWebsocket();

            var streamKitController =
                new StreamKitController(character, obs, obsConfiguration, new NLogLogger());
            streamKitController.SetupBindings();

            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);

            var sceneName = obs.GetCurrentScene().Name;
            Assert.IsTrue(obs.TryGetSceneItemProperties(
                obsConfiguration.AliveStateSourceName, sceneName, out _),
                $"Source {obsConfiguration.AliveStateSourceName} does not exist.");
            Assert.IsTrue(obs.TryGetSceneItemProperties(
                obsConfiguration.DeadStateSourceName, sceneName, out _),
                $"Source {obsConfiguration.DeadStateSourceName} does not exist.");

            character.IsAlive = false;

            obs.TryGetSceneItemProperties(
                obsConfiguration.AliveStateSourceName, sceneName, out var aliveStateSource);
            Assert.IsFalse(aliveStateSource.Visible);

            obs.TryGetSceneItemProperties(
                obsConfiguration.DeadStateSourceName, sceneName, out var deadStateSource);
            Assert.IsTrue(deadStateSource.Visible);

            character.IsAlive = true;

            obs.TryGetSceneItemProperties(
                obsConfiguration.AliveStateSourceName, sceneName, out aliveStateSource);
            Assert.True(aliveStateSource.Visible);

            obs.TryGetSceneItemProperties(
                obsConfiguration.DeadStateSourceName, sceneName, out deadStateSource);
            Assert.IsFalse(deadStateSource.Visible);
        }
    }
}
