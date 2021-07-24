using System;
using UnityEngine;

namespace Runner.Game
{
    class PlayerAvatar : MonoBehaviour
    {
        public event Action OnDeath;
        public event Action<GameObject> OnCoinHit;
    }
}