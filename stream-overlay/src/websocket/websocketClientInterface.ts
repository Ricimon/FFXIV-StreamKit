import { Observable } from 'rxjs';
import { interfaces } from 'inversify';

export enum WebsocketState {
  CLOSED = 0,
  CLOSING = 1,
  CONNECTING = 2,
  OPEN = 3,
}

export interface IWebsocketClient {
  connectionState: Observable<WebsocketState>;

  connect(url: string): void;
  pushData(key: string, data: Uint8Array): void;
  listenForDataUpdate(key: string): Observable<Uint8Array>;
}

export namespace IWebsocketClient {
  export const $: interfaces.ServiceIdentifier<IWebsocketClient> = Symbol('IWebsocketClient');
}