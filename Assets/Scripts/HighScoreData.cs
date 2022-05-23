using System;

namespace BrickBreaker
{
    /// <summary>
    /// Class representing a high score
    /// </summary>
    [Serializable]
    public class HighScoreData
    {
        public string playerName;
        public int highScore;
    }
}