using System.Linq;
using Runner.Misc;
using UnityEditor;
using UnityEngine;

namespace Runner.Game
{
    partial class Generator
    {
        [MenuItem("Cache Blocks Data")]
        static void CacheLocationBlocksData() {
            var data = GetCachedData();
            var elementsPaths = AssetDatabase.FindAssets(string.Empty, new[] {Configuration.BLOCK_ELEMENTS_PREFABS_PATH});
            data.LocationElements = new GameObject[elementsPaths.Length];
            for (int i = 0; i < data.LocationElements.Length; i++)
                data.LocationElements[i] = AssetDatabase.LoadAssetAtPath<GameObject>(elementsPaths[i]);

            var prefabsToCache = AssetDatabase.FindAssets(string.Empty, new[] {Configuration.LOCATION_BLOCK_PREFABS_PATH});
            data.Data = prefabsToCache.Select(LoadBlockPrefabFromPath).GroupBy(x => x.Item1)
                                      .Select(group => @group.Select(x => CachedPrefabData.CacheChildrenData(x.Item2)).ToList()).ToList();
            AssetDatabase.SaveAssets();
        }

        static (string, GameObject) LoadBlockPrefabFromPath(string path) {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var locID = path.Split('/').Last().Split('.', ' ')[0];
            return (locID, go);
        }

        static CachedLocationBlocksData GetCachedData() {
            var assets = AssetDatabase.FindAssets(Configuration.CACHED_BLOCKS_DATA_NAME);
            if (assets.Length > 0)
                return AssetDatabase.LoadAssetAtPath<CachedLocationBlocksData>(assets[0]);

            var result = ScriptableObject.CreateInstance<CachedLocationBlocksData>();
            AssetDatabase.CreateAsset(result, Configuration.CACHED_BLOCKS_DATA_NAME);
            return result;
        }
    }
}