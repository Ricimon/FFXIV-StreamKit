import { interfaces } from 'inversify';

export enum LogLevel {
  Trace = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Silent = 5,
}

export function getLogLevelName(logLevel: LogLevel): string {
  return LogLevel[logLevel];
}

export interface ILogger {
  trace(msg:string): void;
  debug(msg:string): void;
  info(msg:string): void;
  warn(msg:string): void;
  error(msg:string): void;
}

export namespace ILogger {
  export const $: interfaces.ServiceIdentifier<ILogger> = Symbol('ILogger');
}