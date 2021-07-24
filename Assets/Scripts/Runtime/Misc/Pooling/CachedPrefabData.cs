using UnityEngine;

namespace Runner.Misc
{
    public readonly struct CachedPrefabData
    {
        public readonly int PoolID;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Scale;

        public CachedPrefabData(GameObject go) {
            PoolID   = ObjectPool.GetPoolID(go);
            Position = go.transform.localPosition;
            Rotation = go.transform.localRotation;
            Scale    = go.transform.localScale;
        }

        public static CachedPrefabData[] CacheChildrenData(GameObject parent) {
            var result = new CachedPrefabData[parent.transform.childCount];
            for (int i = 0; i < result.Length; i++)
                result[i] = new CachedPrefabData(parent.transform.GetChild(i).gameObject);
            return result;
        }
    }
}
