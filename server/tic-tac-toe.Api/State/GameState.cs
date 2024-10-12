namespace Manufacturing.Api.State;

public class GameState
{
    public string GameId { get; set; }
    public Player[] Players { get; set; }
    public int CurrentPlayer { get; set; } = -1;
    public int[][] Board { get; set; }
    public int Winner { get; set; } = -1;
    public bool IsDraw { get; set; }
    public bool IsGameOver { get; set; }
}