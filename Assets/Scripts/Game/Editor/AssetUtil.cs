using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Runner.Game
{
    static class AssetUtil
    {
        [MenuItem("Caching/Cache Blocks Data")]
        static void CacheLocationBlocksData() {
            var data = GetCachedData();
            data.locationElements = GetPrefabPaths(Configuration.BLOCK_ELEMENTS_PREFABS_PATH)
                                   .Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();
            data.data = GetPrefabPaths(Configuration.LOCATION_BLOCK_PREFABS_PATH).GroupBy(GetLocationTag)
                                                                                 .Select(GetLocationBlocksCached).ToArray();
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Cached prefab data was updated.");
        }

        static CachedPooledObjectsData GetCachedData() {
            string[] assets = AssetDatabase.FindAssets(Configuration.CACHED_BLOCKS_DATA_NAME);
            if (assets != null && assets.Length > 0)
                return AssetDatabase.LoadAssetAtPath<CachedPooledObjectsData>(assets[0]);

            var result = ScriptableObject.CreateInstance<CachedPooledObjectsData>();
            AssetDatabase.CreateAsset(result, Configuration.CACHED_BLOCKS_DATA_NAME);
            return result;
        }

        static string GetLocationTag(string path) => path.Split('/').Last().Split('.', ' ')[0];

        static IEnumerable<string> GetPrefabPaths(string folder) => AssetDatabase.FindAssets("t:Prefab", new[] {folder})
                                                                                 .Select(AssetDatabase.GUIDToAssetPath);

        static LocationBlocksData GetLocationBlocksCached(IGrouping<string, string> group) =>
            new LocationBlocksData {
                                       tag = group.Key,
                                       blocks   = group.Select(AssetDatabase.LoadAssetAtPath<GameObject>).Select(CacheChildrenData).ToArray()
                                   };

        static CachedBlockData CacheChildrenData(GameObject parent) {
            var result = new CachedPrefabData[parent.transform.childCount];
            for (int i = 0; i < result.Length; i++)
                result[i] = new CachedPrefabData(parent.transform.GetChild(i).gameObject);
            return new CachedBlockData {name = parent.name, data = result};
        }
    }
}
