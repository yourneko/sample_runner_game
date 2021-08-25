using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Core
{
    /// <summary>Loads and initializes scenes.</summary>
    public class AppManager : MonoBehaviour
    {
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";
        public const string GAME_SCENE_NAME = "Game";

        IGameState currentState;
        /// <summary>TRUE while loading the scene. Do not touch the manager while it's TRUE.</summary>
        public bool IsBusy { get; private set; }

        void Start() {
            DontDestroyOnLoad(this);
            // Starting app scope services
            ServiceProvider.Register(this);
            ServiceProvider.Register(new Userdata.HighScores()); // i tell you, it's IUserDataProvider
            
            StartCoroutine(LoadSceneCoroutine(MAIN_MENU_SCENE_NAME, false));
        }

        /// <summary>Unloads a current scene. Loads a new scene. Initializes the IGameState of the loaded scene.</summary>
        /// <param name="scene">A name of a scene.</param>
        public void LoadScene(string scene) {
            if (IsBusy) throw new Exception("Service is busy");
            StartCoroutine(LoadSceneCoroutine(scene, true));
        }

        IEnumerator LoadSceneCoroutine(string scene, bool unloadCurrent) {
            // I put coroutines there with a clear understanding, that it's completely useless for this project.
            // But I don't want to add the loading screen there, it will go down too fast.  
            IsBusy = true;
            if (unloadCurrent) 
                yield return currentState.ExitRoutine();
            
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            int index = 0;
            // If the scene doesn't contain an instance of IGameState, let's end it all now
            while ((currentState = rootGameObjects[index].GetComponentInChildren<IGameState>()) == null)
                index += 1; 

            yield return currentState.InitRoutine();
            IsBusy = false;
        }
    }
}
