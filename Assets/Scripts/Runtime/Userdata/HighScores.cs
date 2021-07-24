using UnityEngine;

namespace Runner.Userdata
{
    public class HighScores
    {
        const string HIGH_SCORE_ID = "HighScore";
        const string TOTAL_SCORE_ID = "TotalScore";
        
        int highScore, totalScore;

        public HighScores() {
            if (PlayerPrefs.HasKey(HIGH_SCORE_ID))
                ReadHighScore();
            if (PlayerPrefs.HasKey(TOTAL_SCORE_ID))
                ReadTotalScore();
        }
        
        public bool SetScore(int score) {
            SetTotalScore(totalScore + score);
            bool result = score > highScore;
            if (result)
                SetHighScore(score);
            PlayerPrefs.Save();
            return result;
        }

        void SetHighScore(int value) => PlayerPrefs.SetInt(HIGH_SCORE_ID, highScore = value);
        void SetTotalScore(int value) => PlayerPrefs.SetInt(TOTAL_SCORE_ID, totalScore = value);
        void ReadHighScore() => highScore = PlayerPrefs.GetInt(HIGH_SCORE_ID);
        void ReadTotalScore() => totalScore = PlayerPrefs.GetInt(TOTAL_SCORE_ID);
    }
}
