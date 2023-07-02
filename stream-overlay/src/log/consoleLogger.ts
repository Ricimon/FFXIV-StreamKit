import { injectable } from 'inversify';
import { getLogLevelName, ILogger, LogLevel } from './logInterface';

@injectable()
export default class ConsoleLogger implements ILogger {
  public trace(msg: string) { this.log(msg, LogLevel.Trace); }
  public debug(msg: string) { this.log(msg, LogLevel.Debug); }
  public info(msg: string) { this.log(msg, LogLevel.Info); }
  public warn(msg: string) { this.log(msg, LogLevel.Warn); }
  public error(msg: string) { this.log(msg, LogLevel.Error); }

  public log(msg: string, logLevel: LogLevel) {
    console.log(`${getLogLevelName(logLevel)} | ${msg}`)
  }
}