import { useInjection } from "inversify-react";
import { useEffect, useState } from "react";

import { IWebsocketClient, WebsocketState } from "../../websocket/websocketClientInterface";

import './websocket-status.css';

const statusTexts = new Map<WebsocketState, string>([
  [WebsocketState.CLOSED, 'Disconnected'],
  [WebsocketState.CLOSING, 'Disconnecting'],
  [WebsocketState.CONNECTING, 'Connecting'],
  [WebsocketState.OPEN, 'Connected'],
]);

function WebsocketStatus() {
  const ws = useInjection(IWebsocketClient.$);

  const [connectionState, setConnectionState] = useState<WebsocketState>(WebsocketState.CLOSED);

  useEffect(() => {
    var connectionStateSub = ws.connectionStateObservable.subscribe(setConnectionState);
    return () => {
      connectionStateSub.unsubscribe();
    }
  }, []);

  return (
    <div className='websocket-status'>
      Websocket connection status: {statusTexts.get(connectionState) ?? 'UNKNOWN'}
    </div>
  )
}

export default WebsocketStatus;