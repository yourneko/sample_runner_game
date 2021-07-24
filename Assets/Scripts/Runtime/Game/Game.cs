using System;
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

        // Top-down hierarchy is good for apps with simple behavior. It also allows me to ignore an execution order of scripts.
        public IEnumerator Init() {
            // Subscriptions. No unsub needed, I gonna be using same objects till unloading the scene. 
            restartButton.onClick.AddListener(Restart);
            player.OnDeath += PlayerDeathHandler;
            
            // Initializing services for the game state
            pool.Init();
            
            // Starting the game
            //todo: UI
            //todo: connect player input
            yield break;
        }

        public IEnumerator Exit() {
            yield break;
        }

        // Reset game mechanics to defaults. Hide the Game Over screen. Resume the game
        void Restart() {
            
        }

        // Stop the game. Show the Game Over screen (built in Game scene)
        void PlayerDeathHandler() {
            
        }

        void FixedUpdate() {
            locationGenerator.OnDistanceReached(player.transform.position.z);
        }
    }
}
