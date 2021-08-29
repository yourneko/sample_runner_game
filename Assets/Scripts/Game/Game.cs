using System.Collections;
using Runner.Core;
using UnityEngine;
using Runner.Userdata;

namespace Runner.Game
{
    class Game : MonoBehaviour, IGameState
    {
        [SerializeField] PlayerAvatar player;
        [SerializeField] Generator locationGenerator;
        [SerializeField] UI gameUI;

        HighScores scores;
        int score;
        
        // Top-down hierarchy works good when behaviors are simple. It also allows me to ignore an execution order of scripts.
        public IEnumerator InitRoutine() {
            // Subscriptions. No unsub needed, I gonna use same objects till unloading the scene.
            gameUI.OnGameRestart += () => StartCoroutine(RestartRoutine());
            gameUI.OnExitToMenu  += () => ServiceProvider.Get<AppManager>().LoadScene(AppManager.MAIN_MENU_SCENE_NAME);
            player.OnCoinHit     += () => SetScore(score + 1);
            player.OnDeath       += OnPlayerDeath;
            
            // Initializing stuff and starting the game
            scores = ServiceProvider.Get<HighScores>();
            ServiceProvider.Register(gameUI.GetComponentInChildren<IPlayerInput>());
            yield return player.Init();
            ServiceProvider.Register<IGameProgress>(player);
            locationGenerator.Init();
            
            // Updating Game UI
            gameUI.UpdateHighScores(score, scores.HighScore, scores.TotalScore);
            gameUI.SetMode(UI.Mode.Playing);
        }

        public IEnumerator ExitRoutine() {
            ServiceProvider.Unregister<IPlayerInput>();
            ServiceProvider.Unregister<IGameProgress>();
            ServiceProvider.Get<ObjectPool>().Clear();
            yield break;
        }

        void SetScore(int value) {
            score = value;
            gameUI.SetCurrentScore(score);
        }

        IEnumerator RestartRoutine() { // I think I might end up using a coroutine there
            SetScore(0);
            gameUI.SetMode(UI.Mode.Playing);
            locationGenerator.Restart();
            yield return player.Restart();
        }
        
        void OnPlayerDeath() {
            var mode = scores.SetScore(score)
                ? UI.Mode.NewHighScore
                : UI.Mode.GameOver;
            gameUI.SetMode(mode);
            gameUI.UpdateHighScores(score, scores.HighScore, scores.TotalScore);
        }
    }
}
