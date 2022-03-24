using Ninject.Modules;
using OBSWebsocketDotNet;

namespace StreamKit.Ninject
{
    public class StreamKitModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Character>().ToSelf().InSingletonScope();
            Bind<StreamKitController>().ToSelf().InSingletonScope();
            Bind<OBSWebsocket>().ToSelf().InSingletonScope();
        }
    }
}
