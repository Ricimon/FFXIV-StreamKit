import { inject, injectable } from "inversify";
import { BehaviorSubject, Observable } from "rxjs";
import { WebsocketBuilder, Websocket } from 'websocket-ts';

import { IWebsocketClient, NullableString, WebsocketState } from "./websocketClientInterface";
import Logger from "../log/logDispatcher";

@injectable()
class WebsocketClient implements IWebsocketClient {
  public connectionStateObservable: Observable<WebsocketState>;
  private connectionStateSubject = new BehaviorSubject<WebsocketState>(WebsocketState.CLOSED);

  private ws: Websocket | null = null;

  @inject(Logger) private readonly log?: Logger;

  constructor() {
    this.connectionStateObservable = this.connectionStateSubject.asObservable();
  }

  public connect(url: string): void {
    this.ws = new WebsocketBuilder(url)
    .onOpen((i, ev) => {})
    .build();
  }

  public pushData(key: string, data: string): void {
    throw new Error("Method not implemented.");
  }

  public listenForDataUpdate(key: string): Observable<NullableString> {
    throw new Error("Method not implemented.");
  }

}
