using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Game
{
    // Represents a part of location of default length as a collection of pooled objects, which are placed inside of that space
    class SpawnedBlock : MonoBehaviour
    {
        readonly IList<PooledObject> pooled = new List<PooledObject>();
        ObjectPool pool;
        int index;
        Action returnToPoolAction;

        public float Position => transform.position.z;

        public void SetPosition(int blockIndex, Action disposeAction) {
            index              = blockIndex;
            returnToPoolAction = disposeAction;
            transform.Translate(new Vector3(0, 0, Configuration.BLOCK_LENGTH * index - Position));
            if (pool == null)
                pool = ServiceProvider.Get<ObjectPool>();
        }

        public void PlaceExteriorPrefab(CachedPrefabData data) {
            if (pool.TryGetByHash(data.poolID.GetHashCode(), out var result)) {
                pooled.Add(result);
                var t = result.Target.transform;
                t.SetParent(transform);
                t.localScale = data.scale;
                t.rotation   = data.rotation;
                SetChildLocalPosition(t, data.position);
            }
        }

        public void PlaceObjectOnTracks(int objHash, int track, int line) {
            if (pool.TryGetByHash(objHash, out var result)) {
                pooled.Add(result);
                result.Target.transform.SetParent(transform);
                var localPos = new Vector3(Configuration.GetTrackPosX(track), 0, (line - Configuration.OBJECT_LINES_IN_BLOCK) * Configuration.DISTANCE_BETWEEN_OBJECT_ROWS);
                SetChildLocalPosition(result.Target.transform, localPos);
            }
        }

        public void ReturnToPool() {
            foreach (var o in pooled)
                o.Dispose();
            pooled.Clear();
            returnToPoolAction.Invoke();
        }

        void SetChildLocalPosition(Transform target, Vector3 pos) => target.localPosition = pos; //.Translate(pos + transform.position - target.position);
    }
}
