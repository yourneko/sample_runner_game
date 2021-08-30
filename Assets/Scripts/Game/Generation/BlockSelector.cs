using System.Collections.Generic;
using UnityEngine;

namespace Runner.Game
{
    // Implements the selection rules of location exterior elements.
    struct BlockSelector
    {
        readonly LocationBlocksData[] locations;
        int locationIndex, blockIndex,  remainingBlocks;

        CachedBlockData[] LocationBlocksArray => locations[locationIndex].blocks;
        public BlockSelector(LocationBlocksData[] data) {
            locations     = data;
            locationIndex = blockIndex = remainingBlocks = -1;
            Reset();
        }

        public CachedBlockData GetBlock() {
            if (remainingBlocks-- < 0)
                SelectRandomLocation();
            return LocationBlocksArray[blockIndex = GetNextRandomIndex(LocationBlocksArray, blockIndex)];
        }

        public void Reset() {
            locationIndex   = -1;
            SelectRandomLocation();
        }

        void SelectRandomLocation() {
            locationIndex   = GetNextRandomIndex(locations, locationIndex);
            remainingBlocks = Random.Range(Configuration.MIN_BLOCKS_IN_LOCATION, Configuration.MAX_BLOCKS_IN_LOCATION);
            blockIndex      = -1;
        }

        static int GetNextRandomIndex<T>(IList<T> elements, int previousIndex) {
            int index = Random.Range(Mathf.Min(0, previousIndex), elements.Count - 1);
            return index >= previousIndex ? index + 1 : index;
        }
    }
}
