using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Runner.Core;

namespace Runner.Misc
{
    // This level of abstraction is enough for current needs
    public class ObjectPool : MonoBehaviour 
    {
        readonly Dictionary<int, PoolStack> stacks = new Dictionary<int, PoolStack>();
        Transform storedObjects;

        public void Init() { 
            storedObjects = new GameObject().transform;
            storedObjects.SetParent(transform);
            storedObjects.position = Vector3.zero;
            Services.Register(this);
        }

        public void Clear() {
            foreach (var stack in stacks)
                stack.Value.Clear();
            stacks.Clear();
        }

        public PooledObject GetCopy(GameObject go) {
            var obj = GetStack(go).Take();
            return new PooledObject(obj, () => Push(GetPoolID(go), obj));
        }

        public PooledObject AssembleObject(CachedPrefabData[] data) {
            var go = new GameObject();
            var disposeActions = new List<Action>();
            foreach (var child in data) {
                var stack = stacks[child.PoolID];
                var ch = stack.Take();
                ch.transform.SetParent(go.transform);
                ch.transform.localPosition = child.Position;
                ch.transform.localRotation = child.Rotation;
                ch.transform.localScale    = child.Scale;
                disposeActions.Add(() => stack.Put(ch));
            }

            return new PooledObject(go, () => {
                disposeActions.ForEach(x => x.Invoke());
                Destroy(go);
            });
        }

        public void PreloadObjects(GameObject go, int count) {
            var stack = GetStack(go);
            foreach (var newGO in Enumerable.Repeat(Instantiate(go), count)) {
                stack.Put(newGO);
                newGO.transform.SetParent(storedObjects);
            }
        }

        void OnDestroy() => Services.Unregister<ObjectPool>();

        void Push(int id, GameObject item) {
            if (stacks.TryGetValue(id, out var pool)) {
                pool.Put(item);
                item.transform.SetParent(storedObjects);
            }
            else {
                Destroy(item);
            }
        }

        PoolStack GetStack(GameObject go) {
            int poolID = GetPoolID(go);
            if (!stacks.TryGetValue(poolID, out var stack)) 
                stacks.Add(poolID, stack = new PoolStack(go));
            return stack;
        }

        internal static int GetPoolID(GameObject go) => go.GetInstanceID();
    }
}
