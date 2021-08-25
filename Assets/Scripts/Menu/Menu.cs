using System.Collections;
using Runner.Core;
using Runner.Userdata;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Menu
{
    class Menu : MonoBehaviour, IGameState
    {
        [SerializeField] Text highScoreText, totalScoreText;
        
        public IEnumerator InitRoutine() {
            var scores = ServiceProvider.Get<HighScores>();
            highScoreText.text = scores.HighScore.ToString();
            totalScoreText.text = scores.TotalScore.ToString();
            yield break;
        }

        public IEnumerator ExitRoutine() {
            yield break;
        }

        public void StartGame() => ServiceProvider.Get<AppManager>().LoadScene(AppManager.GAME_SCENE_NAME);
    }
}
