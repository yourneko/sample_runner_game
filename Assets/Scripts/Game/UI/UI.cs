using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Game
{
    class UI : MonoBehaviour
    {
        const string SCORE_TEXT = "Score: ";

        internal event Action OnGameRestart, OnExitToMenu;
        
        [SerializeField] GameObject overlayHud, gameOverMenu;
        [SerializeField] Text hudCurrentScoreText;
        
        [SerializeField] Text currentScoreText, highScoreText, totalScoreText;
        [SerializeField] GameObject newHighScoreBlock;

        public void SetMode(Mode mode) {
            overlayHud.SetActive(mode == Mode.Playing);
            gameOverMenu.SetActive(mode != Mode.Playing);
            newHighScoreBlock.SetActive(mode == Mode.NewHighScore);
        }
        
        public void SetCurrentScore(int score) => hudCurrentScoreText.text = SCORE_TEXT + score;

        public void UpdateHighScores(int current, int high, int total) {
            bool isHighScore = current == high;
            newHighScoreBlock.SetActive(isHighScore);
            highScoreText.transform.parent.gameObject.SetActive(!isHighScore);

            totalScoreText.text   = total.ToString();
            currentScoreText.text = current.ToString();
            highScoreText.text    = high.ToString();
        }

        public void RestartClickedHandler() => OnGameRestart?.Invoke();
        public void ExitClickedHandler() => OnExitToMenu?.Invoke();

        public enum Mode
        {
            Playing,
            GameOver,
            NewHighScore
        }
    }
}
