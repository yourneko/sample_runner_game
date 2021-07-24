using System;
using System.Collections.Generic;
using System.Linq;
using Runner.Core;
using Runner.Misc;
using UnityEngine;

namespace Runner.Game
{
    // Loads and unloads elements of the location along the player's way   
    partial class Generator : MonoBehaviour
    {
        [SerializeField] GameObject coinPrefab;
        [SerializeField] GameObject[] obstaclePrefabs;
        [SerializeField] CachedLocationBlocksData blocksData;

        readonly Queue<SpawnedBlock> blocks = new Queue<SpawnedBlock>();
        BlockSelector blockSelector;
        ObjectSelector objectSelector;
        ObjectPool pool;
        int currentBlockIndex;
        int currentItemRowIndex;
        IGameProgress progressProvider;
        bool initialized;

        public void Init() {
            objectSelector   = new ObjectSelector(Configuration.ObjectSpawnSequences);
            blockSelector    = new BlockSelector(blocksData.Data);
            progressProvider = Services.Get<IGameProgress>();
            pool             = Services.Get<ObjectPool>();
            foreach (var element in blocksData.LocationElements) 
                pool.PreloadObjects(element, 1);
            PreloadLocation();
            initialized = true;
        }

        public void Restart() {
            foreach (var block in blocks)
                block.ReleaseBlock();
            blocks.Clear();

            currentBlockIndex = 0;
            objectSelector.Reset();
            blockSelector.Reset();
            PreloadLocation();
        }

        void Update() {
            if (!initialized)return;
            
            if (blocks.Peek().Position < progressProvider.Distance + Configuration.UNLOAD_DISTANCE)
                blocks.Dequeue().ReleaseBlock();
            if (currentBlockIndex * Configuration.BLOCK_LENGTH < progressProvider.Distance + Configuration.PRELOAD_DISTANCE)
                GenerateBlock();
        }

        void GenerateBlock() {
            var pooledBlock = pool.AssembleObject(blockSelector.GetBlock());
            pooledBlock.Target.transform.SetParent(transform);
            var newBlock = new SpawnedBlock(Configuration.BLOCK_LENGTH * currentBlockIndex, pooledBlock);
            blocks.Enqueue(newBlock);

            if (currentBlockIndex++ <= 0) return;
            for (int i = 0; i < Configuration.OBJECT_ROWS_IN_BLOCK; i++) {
                var row = objectSelector.GetNextRow();
                foreach (var obj in row.Where(t => t != ObjectType.None).Select(GetPooledObject))
                    newBlock.Pooled.Add(obj);
                currentItemRowIndex += 1;
            }
        }

        void PreloadLocation() {
            for (int i = 0; i < Configuration.PRELOAD_ON_START_COUNT; i++)
                GenerateBlock();
        }

        PooledObject GetPooledObject(ObjectType type, int track) {
            var go = type switch {
                         ObjectType.Coin     => coinPrefab,
                         ObjectType.Obstacle => obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length - 1)],
                         ObjectType.None     => null,
                         _                   => throw new Exception("Invalid object type")
                     };
            var pooled = pool.GetCopy(go);
            pooled.Target.transform.Translate(new Vector3(GetPosX(track), 0, currentItemRowIndex * Configuration.DISTANCE_BETWEEN_OBJECT_ROWS));
            return pooled;
        }

        static float GetPosX(int trackNumber) => (trackNumber - Configuration.DEFAULT_TRACK_INDEX) * Configuration.SINGLE_TRACK_WIDTH;
    }
}
