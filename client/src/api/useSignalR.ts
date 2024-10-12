import { useEffect, useState } from "react";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
} from "@microsoft/signalr";

const useSignalR = (
  url: string,
  eventHandlers: Object = {},
  logLevel: LogLevel = LogLevel.Critical,
  connectionChangeCallback?: (isConnected: boolean) => void,
): [HubConnection | null, boolean] => {
  const [hubConnection, setHubConnection] = useState<HubConnection | null>(
    null,
  );
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    if (connectionChangeCallback) {
      connectionChangeCallback(isConnected);
    }
  }, [isConnected, connectionChangeCallback]);

  useEffect(() => {
    // Create the HubConnection
    const connection = new HubConnectionBuilder()
      .withAutomaticReconnect()
      .configureLogging(logLevel)
      .withUrl(url)
      .build();

    // Start the connection
    const startConnection = async () => {
      try {
        await connection.start();
        console.debug("SignalR connected");
        setIsConnected(true);
        setHubConnection(connection);
      } catch (error) {
        console.error("Error while starting connection:", error);
      }
    };

    const stopConnection = async () => {
      try {
        await connection.stop();
      } catch (error) {
        console.error("Error while starting connection:", error);
      }
    };

    // Set up event handlers
    for (const [eventName, handler] of Object.entries(eventHandlers)) {
      connection.on(eventName, handler as (...args: any[]) => any);
    }

    connection.onreconnecting(() => {
      console.debug("Reconnecting");
      setIsConnected(false);
    });

    connection.onreconnected(() => {
      console.debug("Reconnected");
      setIsConnected(true);
    });

    connection.onclose(() => {
      console.debug("Connection closed");
      setIsConnected(false);
    });

    // Start the connection when the hook is first used
    stopConnection().then(() => {
      startConnection();
    });

    // Clean up the connection when the component unmounts
    return () => {
      connection.stop().then(() => {
        console.debug("SignalR connection stopped");
        setIsConnected(false);
      });
    };
  }, [url, logLevel]);

  return [hubConnection, isConnected];
};

export default useSignalR;
