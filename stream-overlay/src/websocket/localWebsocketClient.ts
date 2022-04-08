import { injectable } from "inversify";
import { BehaviorSubject, filter, map, Observable, startWith, Subject } from "rxjs";
import { IWebsocketClient, WebsocketState } from "./websocketClientInterface";

@injectable()
export class LocalWebsocketClient implements IWebsocketClient {
  public connectionState: Observable<WebsocketState>;

  private connectionStateSubject = new BehaviorSubject<WebsocketState>(WebsocketState.CLOSED);
  private data: {[key:string]: Uint8Array} = {};
  private dataSubject = new Subject<{ key: string, data: Uint8Array }>();

  constructor() {
    this.connectionState = this.connectionStateSubject.asObservable();
  }

  public connect(url: string): void {
    this.connectionStateSubject.next(WebsocketState.CONNECTING);
    this.connectionStateSubject.next(WebsocketState.OPEN);
  }

  public pushData(key: string, data: Uint8Array): void {
    this.data[key] = data;
    this.dataSubject.next({ key: key, data: data });
  }

  public listenForDataUpdate(key: string): Observable<Uint8Array> {
    let obs = this.dataSubject.asObservable()
      .pipe(
        filter(entry => entry.key === key),
        map(({ data }) => data)
      );

    if (this.data.hasOwnProperty(key)) {
      obs.pipe(startWith(this.data[key]));
    }

    return obs;
  }
}