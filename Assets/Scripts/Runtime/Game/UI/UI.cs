using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Game
{
    class UI : MonoBehaviour
    {
        internal event Action OnGameRestart, OnExitToMenu;
        
        [SerializeField] GameObject overlayHud, gameOverMenu;
        [SerializeField] Text hudCurrentScoreText;
        
        [SerializeField] Text[] highScoreTexts;
        [SerializeField] Text totalScoreText;
        [SerializeField] GameObject newHighScoreBlock;

        public void SetMode(Mode mode) {
            overlayHud.SetActive(mode == Mode.Playing);
            gameOverMenu.SetActive(mode != Mode.Playing);
            newHighScoreBlock.SetActive(mode == Mode.NewHighScore);
        }
        
        public void SetCurrentScore(int score) => hudCurrentScoreText.text = score.ToString();

        public void UpdateHighScores(int high, int total) {
            foreach (var txt in highScoreTexts)
                txt.text = high.ToString();
            totalScoreText.text = total.ToString();
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
