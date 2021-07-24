using System.Collections.Generic;
using Runner.Misc;
using UnityEngine;

namespace Runner.Game
{
    class CachedLocationBlocksData : ScriptableObject
    {
        public GameObject[] LocationElements;
        public IList<List<CachedPrefabData[]>> Data;
    }
}
