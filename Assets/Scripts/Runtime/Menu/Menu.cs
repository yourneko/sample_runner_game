using System.Collections;
using Runner.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Menu
{
    class Menu : MonoBehaviour, IGameState
    {
        [SerializeField] Button playBtn;
        
        public IEnumerator Init() {
            throw new System.NotImplementedException();
        }

        public IEnumerator Exit() {
            throw new System.NotImplementedException();
        }
    }
}
