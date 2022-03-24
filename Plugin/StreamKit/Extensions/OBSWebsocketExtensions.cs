using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Reactive.Linq;

namespace StreamKit
{
    public static class OBSWebsocketExtensions
    {
        public static IObservable<object> ConnectedAsObservable(this OBSWebsocket obs)
        {
            return Observable.FromEventPattern(
                h => obs.Connected += h,
                h => obs.Connected -= h);
        }

        public static IObservable<object> DisconnectedAsObservable(this OBSWebsocket obs)
        {
            return Observable.FromEventPattern(
                h => obs.Disconnected += h,
                h => obs.Disconnected -= h);
        }

        public static bool TryGetSceneItemProperties(
            this OBSWebsocket obs,
            string itemName,
            out SceneItemProperties sceneItemProperties)
        {
            return TryGetSceneItemProperties(obs, itemName, null, out sceneItemProperties);
        }

        public static bool TryGetSceneItemProperties(
            this OBSWebsocket obs,
            string itemName,
            string sceneName,
            out SceneItemProperties sceneItemProperties)
        {
            try
            {
                sceneItemProperties = obs.GetSceneItemProperties(itemName, sceneName);
                return true;
            }
            catch (ErrorResponseException)
            {
                sceneItemProperties = null;
                return false;
            }
        }
    }
}
