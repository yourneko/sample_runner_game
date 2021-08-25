using UnityEngine;

namespace Runner.Userdata
{
    public class HighScores
    {
        const string HIGH_SCORE_ID = "HighScore";
        const string TOTAL_SCORE_ID = "TotalScore";
        
        public int HighScore { get; private set; }
        public int TotalScore { get; private set; }

        public HighScores() {
            if (PlayerPrefs.HasKey(HIGH_SCORE_ID))
                ReadHighScore();
            if (PlayerPrefs.HasKey(TOTAL_SCORE_ID))
                ReadTotalScore();
        }
        
        public bool SetScore(int score) {
            SetTotalScore(TotalScore + score);
            bool result = score > HighScore;
            if (result)
                SetHighScore(score);
            PlayerPrefs.Save();
            return result;
        }

        void SetHighScore(int value) => PlayerPrefs.SetInt(HIGH_SCORE_ID, HighScore = value);
        void SetTotalScore(int value) => PlayerPrefs.SetInt(TOTAL_SCORE_ID, TotalScore = value);
        void ReadHighScore() => HighScore = PlayerPrefs.GetInt(HIGH_SCORE_ID);
        void ReadTotalScore() => TotalScore = PlayerPrefs.GetInt(TOTAL_SCORE_ID);
    }
}
