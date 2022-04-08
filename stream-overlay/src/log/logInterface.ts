import { Observable } from 'rxjs';

export enum LogLevel {
  Trace = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Silent = 5,
}

export interface LogMessage {
  msg: string;
  logLevel: LogLevel;
}

export interface Logger {
  trace(msg:string): void;
  debug(msg:string): void;
  info(msg:string): void;
  warn(msg:string): void;
  error(msg:string): void;

  onLog: Observable<LogMessage>;
}