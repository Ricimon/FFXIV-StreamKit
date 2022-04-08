import React from 'react';
import { Subject } from 'rxjs';

import { LogLevel, LogMessage, Logger } from './logInterface';

const onLogSubject = new Subject<LogMessage>();
const onLog = (msg: string, logLevel: LogLevel) => {
  onLogSubject.next({ msg: msg, logLevel: logLevel });
}
const logger: Logger = {
  trace: (msg) => onLog(msg, LogLevel.Trace),
  debug: (msg) => onLog(msg, LogLevel.Debug),
  info: (msg) => onLog(msg, LogLevel.Info),
  warn: (msg) => onLog(msg, LogLevel.Warn),
  error: (msg) => onLog(msg, LogLevel.Error),
  onLog: onLogSubject,
}

export function getLogLevelName(logLevel: LogLevel): string {
  return LogLevel[logLevel];
}

const LogContext = React.createContext<Logger>(logger);
export const useLogContext = () => React.useContext(LogContext);

function LogProvider(props: { children: any; }) {
  const { children } = props;
  return (
    <LogContext.Provider value={logger}>
      {children}
    </LogContext.Provider>
  );
}

export default LogProvider;