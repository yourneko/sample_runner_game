using System;
using UnityEngine;

namespace Runner
{
    [Serializable]
    public readonly struct PooledObject  : IDisposable
    {
        public readonly GameObject Target;
        readonly Action returnAction;

        internal PooledObject(GameObject go, Action returnToPoolAction) {
            Target       = go;
            returnAction = returnToPoolAction;
        }

        public void Dispose() {
            returnAction.Invoke();
        }
    }
}
