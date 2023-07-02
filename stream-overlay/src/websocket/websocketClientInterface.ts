import { Observable } from 'rxjs';
import { interfaces } from 'inversify';

export enum WebsocketState {
  CLOSED = 0,
  CLOSING = 1,
  CONNECTING = 2,
  OPEN = 3,
}

export type NullableString = string | null;

export interface IWebsocketClient {
  connectionStateObservable: Observable<WebsocketState>;

  connect(url: string): void;
  pushData(key: string, data: string): void;
  listenForDataUpdate(key: string): Observable<NullableString>;
}

export namespace IWebsocketClient {
  export const $: interfaces.ServiceIdentifier<IWebsocketClient> = Symbol('IWebsocketClient');
}