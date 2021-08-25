using System.Collections;
using Runner.Core;
using UnityEngine;
using Runner.Userdata;

namespace Runner.Game
{
    class Game : MonoBehaviour, IGameState
    {
        [SerializeField] ObjectPool pool;
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
            player.OnDeath       += OnPlayerDied;
            player.OnCoinHit     += OnCoinCollected;
            
            // Initializing stuff and starting the game
            scores = ServiceProvider.Get<HighScores>();
            pool.Init();
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
        
        void OnPlayerDied() {
            var mode = scores.SetScore(score) ? UI.Mode.NewHighScore : UI.Mode.GameOver;
            gameUI.SetMode(mode);
            gameUI.UpdateHighScores(score, scores.HighScore, scores.TotalScore);
        }
        void OnCoinCollected() => SetScore(score + 1);
    }
}
