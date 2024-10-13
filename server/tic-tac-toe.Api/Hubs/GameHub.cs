using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufacturing.Api.State;
using Microsoft.AspNetCore.SignalR;
using Sqids;

namespace Manufacturing.Api.Hubs;

public class GameHub: Hub
{
    private static readonly Dictionary<string, GameState> Games = new();
    private static readonly SqidsEncoder<int> Sqids = new();
    
    // leave game
    public async Task LeaveGame(string gameId)
    {
        // Check if the game exists
        if (!Games.TryGetValue(gameId, out var game))
        {
            await Clients.Caller.SendAsync("GameNotFound", "Game not found.");
            return;
        }
        
        // Remove the player from the game
        var player = game.Players.FirstOrDefault(player => player.ConnectionId == Context.ConnectionId);
        if(player == null)
        {
            if (game.Players.All(x => x == null))
            {
                // remove the game if there are no players
                Games.Remove(gameId);
            }
            
            await Clients.Caller.SendAsync("PlayerNotInGame", "Player not in game.");
            return;
        }
        
        game.Players[Array.IndexOf(game.Players, player)] = null;
        
        // Check if the game is over
        if (game.IsGameOver)
        {
            Games.Remove(gameId);
            await Clients.Group(gameId).SendAsync("GameEnded", "Game ended.");
            return;
        }
        
        // Remove the player from the group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        
        // Send the player left event to all players
        await Clients.Group(gameId).SendAsync("PlayerLeft", $"{player.Name} has left the group {gameId}.");
    }
    
    public async Task JoinGame(string gameId, string playerName)
    {
        // Check if the game exists
        if (!Games.TryGetValue(gameId, out var game))
        {
            await Clients.Caller.SendAsync("GameNotFound", "Game not found.");
            return;
        }
        
        // Check if the game is full
        if (game.Players[1] != null)
        {
            await Clients.Caller.SendAsync("GameFull", "Game is full.");
            return;
        }
        
        // Add the player to the game
        var player = new Player(Context.ConnectionId, playerName);
        Games[gameId].Players[1] = player;
        
        // Add the player to the group
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        
        // randomize the first player
        game.CurrentPlayer = new Random().Next(0, 1);
        
        // Send the player joined event to all players
        await Clients.Group(gameId).SendAsync("PlayerJoined", $"{playerName} has joined the group {gameId}. The first player to go is {game.Players[game.CurrentPlayer].Name}.", game);
    }
    
    public Task CreateGame(string playerName)
    {
        var gameId = Sqids.Encode(Games.Count);
        var player1 = new Player(Context.ConnectionId, playerName);
        var board = new int[3][];   
        
        // Initialize the board
        for (var i = 0; i < board.Length; i++)
        {
            board[i] = new int[3];
        }
        
        // Initialize each element to -1
        foreach (var t in board)
        {
            for (var j = 0; j < t.Length; j++)
            {
                t[j] = -1;
            }
        }

        var newGame = new GameState
        {
            GameId = gameId,
            Players = [player1, null],
            Board = board
        };
        
        Games.Add(gameId, newGame);
        
        // Add the player to the group
        Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        // Send the game created event to the player
        return Clients.Caller.SendAsync("GameCreated", newGame);
    }
    
    public async Task MakeMove(string gameId, int x, int y)
    {
        // Check if the game exists
        if (!Games.TryGetValue(gameId, out var game))
        {
            await Clients.Caller.SendAsync("GameNotFound", "Game not found.");
            return;
        }
        
        // Check if the player is in the game
        if (game.Players.All(player => player.ConnectionId != Context.ConnectionId))
        {
            await Clients.Caller.SendAsync("PlayerNotInGame", "Player not in game.");
            return;
        }
        
        // check if there is two players
        if (game.Players[1] == null)
        {
            await Clients.Caller.SendAsync("NotEnoughPlayers", "Not enough players.");
            return;
        }
        
        // Check if it's the player's turn
        if (game.Players[game.CurrentPlayer].ConnectionId != Context.ConnectionId)
        {
            await Clients.Caller.SendAsync("NotYourTurn", "Not your turn.");
            return;
        }
        
        // Check if the move is valid
        if (game.Board[x][y] != -1 || CheckGameOver(game))
        {
            await Clients.Caller.SendAsync("InvalidMove", "Invalid move.");
            return;
        }
        
        // Update the board
        game.Board[x][y] = game.CurrentPlayer;
        
        // Switch players
        game.CurrentPlayer = game.CurrentPlayer == 0 ? 1 : 0;
        
        // Check if the game is over
        if (CheckGameOver(game))
        {
            game.IsGameOver = true;
            
            if (game.IsDraw)
            {
                await Clients.Group(gameId).SendAsync("GameOver", "Game over. It's a draw.", game);
                return;
            }
            
            switch (game.Winner)
            {
                case 0:
                    await Clients.Group(gameId).SendAsync("GameOver", $"Game over. {game.Players[0].Name} wins.", game);
                    return;
                case 1:
                    await Clients.Group(gameId).SendAsync("GameOver", $"Game over. {game.Players[1].Name} wins.", game);
                    return;
            }
        }
        
        // Send the move event to all players
        await Clients.Group(gameId).SendAsync("MoveEvent", $"{Context.ConnectionId}: {gameId}, move: ({x}, {y})", game);
    }
    
    private bool CheckGameOver(GameState game)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (game.Board[i][0] != -1 && game.Board[i][0] == game.Board[i][1] && game.Board[i][1] == game.Board[i][2])
            {
                game.Winner = game.Board[i][0];
                return true;
            }
        }
        
        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (game.Board[0][i] != -1 && game.Board[0][i] == game.Board[1][i] && game.Board[1][i] == game.Board[2][i])
            {
                game.Winner = game.Board[0][i];
                return true;
            }
        }
        
        // Check diagonals
        if (game.Board[0][0] != -1 && game.Board[0][0] == game.Board[1][1] && game.Board[1][1] == game.Board[2][2])
        {
            game.Winner = game.Board[0][0];
            return true;
        }
        
        if (game.Board[0][2] != -1 && game.Board[0][2] == game.Board[1][1] && game.Board[1][1] == game.Board[2][0])
        {
            game.Winner = game.Board[0][2];
            return true;
        }
        
        // Check for draw
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (game.Board[i][j] == -1)
                {
                    return false;
                }
            }
        }
        
        game.IsDraw = true;
        return true;
    }
}