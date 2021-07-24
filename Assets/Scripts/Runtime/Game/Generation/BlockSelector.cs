using System.Collections.Generic;
using Runner.Misc;
using UnityEngine;

namespace Runner.Game
{
    // Implements the selection rules of location exterior elements.
    struct BlockSelector
    {
        readonly IList<List<CachedPrefabData[]>> blocks;
        int locationIndex;
        int remainingBlocks;
        
        public BlockSelector(IList<List<CachedPrefabData[]>> blocksForLocations) {
            blocks          = blocksForLocations;
            locationIndex   = 0;
            remainingBlocks = 0;
            Reset();
        }

        public CachedPrefabData[] GetBlock() {
            if (remainingBlocks < 0) {
                int newLocationIndex = Random.Range(0, blocks.Count - 2);
                locationIndex   = (newLocationIndex >= locationIndex) ? newLocationIndex + 1 : newLocationIndex;
                remainingBlocks = RandomLocationLength;
            }
            remainingBlocks -= 1;
            return RandomBlock;
        }

        public void Reset() {
            locationIndex   = Random.Range(0, blocks.Count - 1);
            remainingBlocks = RandomLocationLength;
        }
        
        CachedPrefabData[] RandomBlock => blocks[locationIndex][Random.Range(0, blocks[locationIndex].Count - 1)];
        static int RandomLocationLength => Random.Range(Configuration.MIN_BLOCKS_IN_LOCATION, Configuration.MAX_BLOCKS_IN_LOCATION);
    }
}
