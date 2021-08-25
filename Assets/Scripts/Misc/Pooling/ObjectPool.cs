using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner
{
    // Pooling is used only for assembling location blocks from reusable parts.
    public class ObjectPool : MonoBehaviour 
    {
        readonly Dictionary<int, PoolStack> stacks = new Dictionary<int, PoolStack>();
        Transform storedObjects;

        public void Init() { 
            storedObjects = new GameObject().transform;
            storedObjects.SetParent(transform);
            storedObjects.position = Vector3.zero;
            ServiceProvider.Register(this);
        }

        public void Clear() {
            foreach (var stack in stacks)
                stack.Value.Clear();
            stacks.Clear();
        }

        public PooledObject GetCopy(GameObject go) {
            var obj = GetStack(go).Take();
            return new PooledObject(obj, () => Push(go.name.GetHashCode(), obj));
        }

        public bool TryGetByHash(int hash, out PooledObject obj) {
            if (stacks.TryGetValue(hash, out var stack)) {
                var go = stack.Take();
                obj = new PooledObject(go, () => Push(hash, go));
                return true;
            }
            obj = new PooledObject(null, null);
            return false;
        }

        public void PreloadObjects(GameObject go, int count) {
            var stack = GetStack(go);
            foreach (var newGO in Enumerable.Repeat(Instantiate(go), count)) {
                stack.Put(newGO);
                newGO.transform.SetParent(storedObjects);
            }
        }

        void OnDestroy() => ServiceProvider.Unregister<ObjectPool>();

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
            int poolID = go.name.GetHashCode();
            if (!stacks.TryGetValue(poolID, out var stack)) 
                stacks.Add(poolID, stack = new PoolStack(go));
            return stack;
        }
    }
}
