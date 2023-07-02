import { injectable, multiInject, optional } from 'inversify';

import { ILogger } from './logInterface';

@injectable()
export default class Logger {
  @multiInject(ILogger.$) @optional() private readonly loggers?: ILogger[];

  public trace(msg: string) { this.loggers?.forEach(l => l.trace(msg)); }
  public debug(msg: string) { this.loggers?.forEach(l => l.debug(msg)); }
  public info(msg: string) { this.loggers?.forEach(l => l.info(msg)); }
  public warn(msg: string) { this.loggers?.forEach(l => l.warn(msg)); }
  public error(msg: string) { this.loggers?.forEach(l => l.error(msg)); }
}