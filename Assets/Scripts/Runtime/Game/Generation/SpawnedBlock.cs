using System.Collections.Generic;
using Runner.Misc;
using UnityEngine;

namespace Runner.Game
{
    readonly struct SpawnedBlock
    {
        public readonly float Position;
        public readonly IList<PooledObject> Pooled;

        public SpawnedBlock(float positionZ, PooledObject block) {
            Position = positionZ;
            Pooled   = new List<PooledObject> {block};
            block.Target.transform.Translate(new Vector3(0, 0, positionZ));
        }

        public void ReleaseBlock() {
            foreach (var o in Pooled) 
                o.Dispose();
        }
    }
}
