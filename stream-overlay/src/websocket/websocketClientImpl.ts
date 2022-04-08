import { Observable } from "rxjs";
import { IWebsocketClient, WebsocketState } from "./websocketClientInterface";

class WebsocketClient implements IWebsocketClient {
  public connectionState: Observable<WebsocketState>;

  constructor() {
    this.connectionState = new Observable();
  }

  public connect(url: string): void {
    throw new Error("Method not implemented.");
  }

  public pushData(key: string, data: Uint8Array): void {
    throw new Error("Method not implemented.");
  }

  public listenForDataUpdate(key: string): Observable<Uint8Array> {
    throw new Error("Method not implemented.");
  }

}
