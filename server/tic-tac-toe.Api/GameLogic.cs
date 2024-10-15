using Manufacturing.Api.State;

namespace Manufacturing.Api;

public class GameLogic
{
    public static bool CheckGameOver(GameState game)
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