using System;

namespace Core
{
    public interface IGameStatsProvider
    {
        event Action<int, int> OnGemsProgressChanged;
        event Action<int> OnLivesChanged;
        event Action<GameState> OnGameStateChanged;
        
        int CurrentGems { get; }
        int TotalGems { get; }
        int CurrentLives { get; }
    }
}
