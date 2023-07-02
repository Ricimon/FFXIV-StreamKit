import { useInjection } from "inversify-react";
import { useEffect } from "react";

import { IWebsocketClient } from './websocketClientInterface';
import { queryString } from "../constants";

function WebsocketClientComponent() {
  const ws = useInjection(IWebsocketClient.$);
  const urlSearchParams = useInjection(URLSearchParams);

  const wsUrl = urlSearchParams.get(queryString.wsUrl) ?? '';

  useEffect(() => {
    ws.connect(wsUrl);
  }, []);
  return (
    <></>
  )
}

export default WebsocketClientComponent;