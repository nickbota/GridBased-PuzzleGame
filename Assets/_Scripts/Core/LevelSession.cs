namespace Core
{
    public class LevelSession
    {
        public int Lives { get; private set; }
        public int GemsFound { get; private set; }
        public int TotalGems { get; }

        public LevelSession(int startingLives, int totalGems)
        {
            Lives = startingLives;
            TotalGems = totalGems;
            GemsFound = 0;
        }

        public void DecrementLife() => Lives--;
        public void IncrementGems() => GemsFound++;
    }
}
