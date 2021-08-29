using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner.Game
{
    // Location is made of blocks which are assembled in front of a player. Elements of a location are reused (with object pool)
    class Generator : MonoBehaviour
    {
        [SerializeField] GameObject coinPrefab, blockPrefab;
        [SerializeField] GameObject[] obstaclePrefabs;
        [SerializeField] CachedPooledObjectsData blocksData;

        // contains references to loaded location blocks.
        readonly Queue<SpawnedBlock> blocks = new Queue<SpawnedBlock>();
        bool initialized;
        BlockSelector blockSelector;
        ObjectSelector objectSelector;
        ObjectPool pool;
        IGameProgress progressProvider;
        int currentBlockIndex;
        // hashes used by pool
        int coinPrefabHash;
        int[] obstaclePrefabHashes;

        public void Init() {
            objectSelector = new ObjectSelector(Configuration.ObjectSpawnSequences);
            blockSelector = new BlockSelector(blocksData.data);
            coinPrefabHash = coinPrefab.name.GetHashCode();
            obstaclePrefabHashes = obstaclePrefabs.Select(o => o.name.GetHashCode()).ToArray();
            progressProvider = ServiceProvider.Get<IGameProgress>();

            // pre-loading prefabs to the pool, so prefabs can be requested from pool without a GameObject reference, by the hash alone
            pool = ServiceProvider.Get<ObjectPool>();
            foreach (var t in blocksData.locationElements)
                pool.PreloadObjects(t, 1);
            foreach (var o in obstaclePrefabs)
                pool.PreloadObjects(o, 1);
            pool.PreloadObjects(coinPrefab, 1);
            PreloadLocation();
            initialized = true;
        }

        public void Restart() {
            foreach (var block in blocks)
                block.ReturnToPool();
            blocks.Clear();

            currentBlockIndex = 0;
            objectSelector.Reset();
            blockSelector.Reset();
            PreloadLocation();
        }

        void Update() {
            if (!initialized) return;
            
            if (blocks.Peek().Position < progressProvider.Distance + Configuration.UNLOAD_DISTANCE)
                blocks.Dequeue().ReturnToPool();
            if (currentBlockIndex * Configuration.BLOCK_LENGTH < progressProvider.Distance + Configuration.PRELOAD_DISTANCE)
                GenerateNextBlock();
        }

        void GenerateNextBlock() {
            var block = pool.GetCopy(blockPrefab);
            var spawnedBlock = block.Target.GetComponent<SpawnedBlock>();
            spawnedBlock.transform.SetParent(transform);
            spawnedBlock.SetPosition(currentBlockIndex, block.Dispose);
            blocks.Enqueue(spawnedBlock);

            foreach (var prefab in blockSelector.GetBlock())
                spawnedBlock.PlaceExteriorPrefab(prefab);

            if (currentBlockIndex++ <= 0) return;
            for (int i = 0; i < Configuration.OBJECT_LINES_IN_BLOCK; i++) {
                var row = objectSelector.GetNextRow();
                for (int track = Configuration.LEFT_TRACK_INDEX; track < Configuration.RIGHT_TRACK_INDEX; track++)
                    SpawnObjectOnTrack(spawnedBlock, row[track - Configuration.LEFT_TRACK_INDEX], i, track);
            }
        }

        // Loads first few blocks
        void PreloadLocation() {
            for (int i = 0; i < Configuration.PRELOAD_ON_START_COUNT; i++)
                GenerateNextBlock();
        }

        void SpawnObjectOnTrack(SpawnedBlock block, ObjectType obj, int line, int track) {
            if (obj == ObjectType.None)
                return;
            int hash = obj == ObjectType.Coin
                           ? coinPrefabHash
                           : obstaclePrefabHashes[Random.Range(0, obstaclePrefabHashes.Length - 1)];
            block.PlaceObjectOnTracks(hash, track, line);
        }
    }
}
