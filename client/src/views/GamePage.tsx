import { BASE_URL_SIGNALR } from "../contants/constants";
import useGameHub from "../api/useGameHub";
import { useCallback, useState } from "react";
import CreateGameMenu from "../components/CreateGameMenu";
import JoinGameMenu from "../components/JoinGameMenu";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faX, faO } from "@fortawesome/free-solid-svg-icons";

export default function GamePage() {
  const { message, gameState, joinGame, createGame, makeMove, leaveGame } =
    useGameHub(BASE_URL_SIGNALR);

  const [isGameCreator, setIsGameCreator] = useState<boolean | null>(null);

  const isGameStated = useCallback(() => {
    return gameState?.gameId != null;
  }, [gameState]);

  const isWaitingForPlayerToJoin = useCallback(() => {
    return gameState?.players[1] === null;
  }, [gameState]);

  return (
    <>
      {message.length > 0 && <span>{message}</span>}
      {!isGameStated() && isGameCreator == null && (
        <div className="ml-auto mr-auto flex max-w-96 flex-col justify-center gap-4">
          <button onClick={() => setIsGameCreator(true)}>Create</button>
          <button onClick={() => setIsGameCreator(false)}>Join</button>
        </div>
      )}
      {!isGameStated() && isGameCreator == true && (
        <div className="ml-auto mr-auto max-w-screen-lg">
          <div className="flex justify-start p-5">
            <button onClick={() => setIsGameCreator(null)}>Back</button>
          </div>
          <CreateGameMenu onSubmit={(d) => createGame(d.playerName)} />
        </div>
      )}
      {!isGameStated() && isGameCreator == false && (
        <div className="ml-auto mr-auto max-w-screen-lg">
          <div className="flex justify-start p-5">
            <button onClick={() => setIsGameCreator(null)}>Back</button>
          </div>
          <JoinGameMenu onSubmit={(d) => joinGame(d.gameId, d.playerName)} />
        </div>
      )}
      {isGameStated() && isWaitingForPlayerToJoin() && (
        <>
          <h2>Waiting for player to join</h2>
          <h1>Join code: {gameState?.gameId}</h1>
        </>
      )}
      {isGameStated() && !isWaitingForPlayerToJoin() && (
        <div className="flex flex-col gap-4">
          <h1>Game</h1>
          {gameState?.winner === 0 && isGameCreator && (
            <div>
              <h2>You won!</h2>
            </div>
          )}
          {gameState?.winner === 1 && !isGameCreator && (
            <div>
              <h2>You won!</h2>
            </div>
          )}
          {gameState?.winner === 2 && (
            <div>
              <h2>It's a draw!</h2>
            </div>
          )}
          {gameState?.winner === 0 && !isGameCreator && (
            <div>
              <h2>You lost!</h2>
            </div>
          )}
          <div className="ml-auto mr-auto grid h-96 w-96 border-collapse grid-cols-3 gap-0 border-0">
            {gameState?.board.map((row, x) =>
              row.map((cell, y) => (
                <div
                  key={`${x}-${y}`}
                  onClick={() => {
                    makeMove(x, y);
                  }}
                  className={`flex h-full w-full items-center justify-center border-2 border-cyan-600 text-center ${
                    x === 0 ? "border-t-0" : ""
                  } ${y === 0 ? "border-l-0" : ""} ${
                    x === gameState.board.length - 1 ? "border-b-0" : ""
                  } ${y === row.length - 1 ? "border-r-0" : ""}`}
                >
                  {cell === 0 ? (
                    <FontAwesomeIcon
                      color="teal"
                      className="h-24 w-24 text-xl"
                      icon={faO}
                    />
                  ) : cell === 1 ? (
                    <FontAwesomeIcon
                      color="teal"
                      className="h-24 w-24 text-xl"
                      icon={faX}
                    />
                  ) : (
                    <div className="h-24 w-24" />
                  )}
                </div>
              )),
            )}
          </div>
          <button onClick={leaveGame}>Leave game</button>
        </div>
      )}
    </>
  );
}
