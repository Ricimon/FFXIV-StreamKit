import { Container } from "inversify";

import { IWebsocketClient } from './websocket/websocketClientInterface';
import { LocalWebsocketClient } from './websocket/localWebsocketClient';

const container = new Container();
container.bind(IWebsocketClient.$).to(LocalWebsocketClient);
