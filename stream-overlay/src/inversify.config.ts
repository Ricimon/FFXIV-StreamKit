import { Container } from 'inversify';
import 'reflect-metadata'

import { IWebsocketClient } from './websocket/websocketClientInterface';
import LocalWebsocketClient from './websocket/localWebsocketClient';
import Logger from './log/logDispatcher';
import { ILogger } from './log/logInterface';
import ConsoleLogger from './log/consoleLogger';
import Character from './effects/common/character';

function createContainer(urlSearchParams: URLSearchParams) {
  const container = new Container();
  container.bind(Logger).toSelf();
  container.bind(ILogger.$).to(ConsoleLogger);
  container.bind(IWebsocketClient.$).to(LocalWebsocketClient).inSingletonScope();
  container.bind(URLSearchParams).toConstantValue(urlSearchParams);
  container.bind(Character).toSelf();
  return container;
}


export default createContainer;