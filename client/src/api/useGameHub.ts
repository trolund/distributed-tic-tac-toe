import { useState, useCallback } from "react";
import useSignalR from "./useSignalR";
import { GameState } from "../models/GameState";
import { LogLevel } from "@microsoft/signalr";
import { connectionHandler } from "../services/EventService";

const useGameHub = (url: string) => {
  const [gameState, setGameState] = useState<GameState | null>(null);
  const [message, setMessage] = useState<string>("");

  const eventHandlers = {
    PlayerJoined: (message: string, gameState: GameState) => {
      setGameState(gameState);
      setMessage(message);
      console.log(message);
    },
    PlayerLeft: (message: string) => {
      setMessage(message);
      console.log(message);
    },
    MoveEvent: (message: string, state: GameState) => {
      console.log(message);
      setGameState(state);
    },
    GameOver: (message: string, state: GameState) => {
      setMessage(message);
      setGameState(state);
      console.log(message);
    },
    GameEnded: (message: string) => {
      setMessage(message);
      console.log("Game Ended", message);
    },
    InvalidMove: (message: string) => {
      setMessage(message);
      console.log("Invalid Move:", message);
    },
    GameNotFound: (message: string) => {
      console.log(message);
      setMessage(message);
      setGameState(null);
    },
    GameFull: (message: string) => {
      console.log(message);
    },
    GameCreated: (gameState: GameState) => {
      setGameState(gameState);
      var players = gameState.players;
      console.log("Game created", players[0].name);
      console.log("Game id:", gameState.gameId);
      console.table(gameState.board);
    },
    PlayerNotInGame: (message: string) => {
      setMessage(message);
      console.log(message);
      setGameState(null);
    },
    NotEnoughPlayers: (message: string) => {
      setMessage(message);
      console.log(message);
    },
    NotYourTurn: (message: string) => {
      setMessage(message);
      console.log(message);
    },
  };

  // Use the useSignalR hook
  const [hubConnection, isConnected] = useSignalR(
    url,
    eventHandlers,
    LogLevel.Critical,
    connectionHandler,
  );

  // Join a game
  const joinGame = useCallback(
    async (gameId: string, playerName: string) => {
      if (hubConnection) {
        try {
          await hubConnection.invoke("JoinGame", gameId, playerName);
          //   setGameId(gameId);
          //   setPlayerName(playerName);
        } catch (error) {
          console.error("Error joining game:", error);
        }
      }
    },
    [hubConnection],
  );

  // Create a new game
  const createGame = useCallback(
    async (playerName: string) => {
      if (hubConnection) {
        try {
          await hubConnection.invoke("CreateGame", playerName);
          //   setPlayerName(playerName);
        } catch (error) {
          console.error("Error creating game:", error);
        }
      }
    },
    [hubConnection],
  );

  // Make a move
  const makeMove = useCallback(
    async (x: number, y: number) => {
      if (hubConnection && gameState) {
        try {
          await hubConnection.invoke("MakeMove", gameState.gameId, x, y);
        } catch (error) {
          console.error("Error making move:", error);
        }
      }
    },
    [hubConnection, gameState],
  );

  // Leave the game
  const leaveGame = useCallback(async () => {
    if (hubConnection && gameState) {
      try {
        await hubConnection.invoke("LeaveGame", gameState.gameId);
        setGameState(null);
      } catch (error) {
        console.error("Error leaving game:", error);
      }
    }
  }, [hubConnection, gameState]);

  return {
    isConnected,
    message,
    gameState,
    joinGame,
    createGame,
    makeMove,
    leaveGame,
  };
};

export default useGameHub;
