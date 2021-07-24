using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Runner.Misc;

namespace Runner.Game
{
    class Game : MonoBehaviour, Core.IGameState
    {
        [SerializeField] ObjectPool pool; // The easiest way to do it  
        [SerializeField] PlayerAvatar player;
        [SerializeField] Generator locationGenerator;
        [SerializeField] Button restartButton;
        [SerializeField] UI gameUI;

        // Top-down hierarchy does good when behaviors are simple. It also allows me to ignore an execution order of scripts.
        public IEnumerator Init() {
            // Subscriptions. No unsub needed, I gonna be using same objects till unloading the scene. 
            restartButton.onClick.AddListener(Restart);
            player.OnDeath   += OnPlayerDied;
            player.OnCoinHit += OnCoinCollected;
            
            // Initializing stuff for the game state
            pool.Init();
            locationGenerator.Init();
            //todo: connect player input before player.Init
            player.Init();
            
            // Starting the game
            //todo: UI
            yield break;
        }

        public IEnumerator Exit() {
            yield break;
        }

        //todo: Reset it all to defaults. Hide the Game Over screen. Resume the game
        void Restart() { // I think I might end up using a coroutine there, so - no events
            
        }
        
        void Update() {
            if (!player.IsAlive) return;
            
            locationGenerator.OnDistanceReached(player.transform.position.z);
        }
        
        // Stop the game. Show the Game Over screen (built in Game scene)
        void OnPlayerDied() {
            //todo: show Game Over screen and update scores
        }

        void OnCoinCollected() {
            //todo: increase counter, notify UI
        }
    }
}
