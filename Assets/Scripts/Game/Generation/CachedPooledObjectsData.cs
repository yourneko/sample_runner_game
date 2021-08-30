using System;
using UnityEngine;

namespace Runner.Game
{
    class CachedPooledObjectsData : ScriptableObject
    {
        public GameObject[] locationElements;
        public LocationBlocksData[] data;
    }

    [Serializable]
    class LocationBlocksData
    {
        public string tag;
        public CachedBlockData[] blocks;
    }

    [Serializable]
    class CachedBlockData
    {
        public string name;
        public CachedPrefabData[] data;
    }

    [Serializable]
    public class CachedPrefabData
    {
        public string poolID;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public CachedPrefabData(GameObject go) {
            poolID   = go.transform.name;
            position = go.transform.localPosition;
            rotation = go.transform.localRotation;
            scale    = go.transform.localScale;
        }
    }
}
