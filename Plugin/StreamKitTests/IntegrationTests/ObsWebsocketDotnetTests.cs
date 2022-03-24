using Microsoft.Extensions.Configuration;
using NLog;
using NUnit.Framework;
using OBSWebsocketDotNet;

namespace StreamKitTests.IntegrationTests
{
    public class ObsWebsocketDotnetTests
    {
        private IConfiguration Configuration { get; set; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private string ObsWebsocketUrl => TestContext.Parameters["ObsWebsocketUrl"];
        private string ObsWebsocket4Password => Configuration["ObsWebsocket4Password"];

        public ObsWebsocketDotnetTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ObsWebsocketTests>();
            Configuration = builder.Build();
        }

        [Test]
        public void ObsWebsocketDotnetConnection_ShouldWork()
        {
            var obs = new OBSWebsocket();
            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);
        }

        [Test]
        public void GettingCurrentScene_ShouldWork()
        {
            var obs = new OBSWebsocket();
            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);

            var scene = obs.GetCurrentScene();
            Logger.Info("The current scene name is " + scene.Name);
        }

        [Test]
        public void GettingNonexistentSceneItem_ShouldThrowException()
        {
            var obs = new OBSWebsocket();
            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);

            Assert.Throws<ErrorResponseException>(() =>
            {
                // No one should have a scene item named this.
                obs.GetSceneItemProperties("oi34nvo4nv3ivnsd");
            });
        }

        [Test]
        public void Testing()
        {
            var obs = new OBSWebsocket();
            obs.Connect(ObsWebsocketUrl, ObsWebsocket4Password);
            //var sources = obs.GetSourcesList();
            //foreach(var source in sources)
            //{
            //    logger.LogInformation(source.Name);
            //}

            var scene = obs.GetCurrentScene();
            //logger.LogInformation(scene.Name);
            //var items = obs.GetSceneItemList(scene.Name);
            //foreach(var item in items)
            //{
            //    logger.LogInformation(item.SourceName);
            //}

            var prop = obs.GetSceneItemProperties("catJAM");
            Logger.Info(prop.Position.X.ToString() + ", " + prop.Position.Y);

            prop.Position.X = 50;
            //obs.SetSceneItemProperties(prop, scene.Name);
        }

    }
}
