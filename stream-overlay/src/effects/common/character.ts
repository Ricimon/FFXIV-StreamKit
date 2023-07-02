import { injectable } from 'inversify';
import { BehaviorSubject, Observable } from 'rxjs';

@injectable()
export default class Character {
  private _isAliveObservable: Observable<boolean>;
  private _isAliveSubject = new BehaviorSubject<boolean>(true);

  constructor() {
    this._isAliveObservable = this._isAliveSubject.asObservable();
  }

  public get isAlive() { return this._isAliveSubject.value; }

  public set isAlive(state: boolean) {
    this._isAliveSubject.next(state);
  }

  public get isAliveObservable() { return this._isAliveObservable; }
}