using System.Collections;
using Runner.Core;
using UnityEngine;
using Runner.Misc;
using Runner.Userdata;

namespace Runner.Game
{
    class Game : MonoBehaviour, IGameState
    {
        [SerializeField] ObjectPool pool; // The easiest way to define all dependencies in project this small.   
        [SerializeField] PlayerAvatar player;
        [SerializeField] Generator locationGenerator;
        [SerializeField] UI gameUI;

        HighScores scores;
        int score;
        
        // Top-down hierarchy does good when behaviors are simple. It also allows me to ignore an execution order of scripts.
        public IEnumerator InitRoutine() {
            // Subscriptions. No unsub needed, I gonna be using same objects till unloading the scene. 
            scores               =  Services.Get<HighScores>();
            gameUI.OnGameRestart += Restart;
            gameUI.OnExitToMenu  += OpenMainMenu;
            player.OnDeath       += OnPlayerDied;
            player.OnCoinHit     += OnCoinCollected;
            
            // Initializing stuff and starting the game
            pool.Init();
            Services.Register(gameUI.GetComponentInChildren<IPlayerInput>());
            yield return player.Init();
            Services.Register<IGameProgress>(player);
            locationGenerator.Init();
            
            // Updating Game UI
            gameUI.UpdateHighScores(scores.HighScore, scores.TotalScore);
            gameUI.SetMode(UI.Mode.Playing);
        }

        public IEnumerator ExitRoutine() {
            Services.Unregister<IPlayerInput>();
            Services.Unregister<IGameProgress>();
            yield break;
        }

        void SetScore(int value) 
        {
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
            gameUI.UpdateHighScores(scores.HighScore, scores.TotalScore);
        }

        void Restart() => StartCoroutine(RestartRoutine());
        void OnCoinCollected() => SetScore(score + 1);
        static void OpenMainMenu() => Services.Get<AppManager>().LoadScene(AppManager.MAIN_MENU_SCENE_NAME);
    }
}
