import { injectable } from "inversify";
import { BehaviorSubject, filter, map, Observable, startWith, Subject } from "rxjs";
import { IWebsocketClient, NullableString, WebsocketState } from "./websocketClientInterface";

@injectable()
export default class LocalWebsocketClient implements IWebsocketClient {
  public connectionStateObservable: Observable<WebsocketState>;

  private connectionStateSubject = new BehaviorSubject<WebsocketState>(WebsocketState.CLOSED);
  private data: { [key: string]: string } = {};
  private dataSubject = new Subject<{ key: string, data: string }>();

  constructor() {
    this.connectionStateObservable = this.connectionStateSubject.asObservable();
  }

  public connect(url: string): void {
    if (!url) {
      this.connectionStateSubject.next(WebsocketState.CLOSED);
      return;
    }
    this.connectionStateSubject.next(WebsocketState.CONNECTING);
    this.connectionStateSubject.next(WebsocketState.OPEN);
  }

  public pushData(key: string, data: string): void {
    this.data[key] = data;
    this.dataSubject.next({ key: key, data: data });
  }

  public listenForDataUpdate(key: string): Observable<NullableString> {
    let obs = this.dataSubject.asObservable()
      .pipe(
        filter(entry => entry.key === key),
        map<{ key: string, data: string }, NullableString>(({ data }) => data)
      );

    if (this.data.hasOwnProperty(key)) {
      obs = obs.pipe(startWith(this.data[key]));
    } else {
      obs = obs.pipe(startWith<NullableString>(null));
    }

    return obs;
  }
}