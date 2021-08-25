using UnityEngine;

namespace Runner.Game
{
    // Implements the selection rules of location exterior elements.
    struct BlockSelector
    {
        readonly LocationBlocksData[] blocks;
        int locationIndex;
        int remainingBlocks;
        
        public BlockSelector(LocationBlocksData[] blocksForLocations) {
            blocks          = blocksForLocations;
            locationIndex   = 0;
            remainingBlocks = 0;
            Reset();
        }

        public CachedPrefabData[] GetBlock() {
            if (remainingBlocks < 0) {
                int newLocationIndex = Random.Range(0, blocks.Length - 2);
                locationIndex   = (newLocationIndex >= locationIndex) ? newLocationIndex + 1 : newLocationIndex;
                remainingBlocks = RandomLocationLength;
            }
            remainingBlocks -= 1;
            return RandomBlock;
        }

        public void Reset() {
            locationIndex   = Random.Range(0, blocks.Length - 1);
            remainingBlocks = RandomLocationLength;
        }
        
        CachedPrefabData[] RandomBlock => blocks[locationIndex].blocks[Random.Range(0, blocks[locationIndex].blocks.Length - 1)].data;
        static int RandomLocationLength => Random.Range(Configuration.MIN_BLOCKS_IN_LOCATION, Configuration.MAX_BLOCKS_IN_LOCATION);
    }
}
