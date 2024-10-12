import { Player } from "./player";

export interface GameState {
  gameId: string;
  players: Player[];
  currentPlayer: number;
  board: number[][];
  winner: number;
  isDraw: boolean;
  isGameOver: boolean;
}
