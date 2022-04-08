import { useEffect, useRef, useState } from 'react';
import { Subscription } from 'rxjs';

import { useLogContext, getLogLevelName } from './LogContext';

// The console logger should only ever be subscribed once
let subscription: Subscription | null = null;

function ConsoleLogger() {
  const log = useLogContext();

  if (!subscription) {
    subscription = log.onLog.subscribe({
      next(logMsg) {
        console.log(`${getLogLevelName(logMsg.logLevel)} | ${logMsg.msg}`)
      }
    });
  }

  useEffect(() => {
    return () => {
      subscription?.unsubscribe();
      subscription = null;
    }
  }, []);

  return (<></>);
}

export default ConsoleLogger;