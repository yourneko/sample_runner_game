using UnityEngine;

namespace Runner.Game
{
    // This script is used as a helper when assembling Block prefabs.
    class BlockBorder : MonoBehaviour
    {
        const float MIDDLE_Z = -Configuration.DISTANCE_BETWEEN_OBJECT_ROWS / 2;
        static readonly Color color = new Color(1, 0, 0, 0.3f);
        static readonly Color altColor = new Color(1, 0, 0, 0.2f);

        static readonly Vector3 bCenter = new Vector3(((float)Configuration.LEFT_TRACK_INDEX + Configuration.RIGHT_TRACK_INDEX) / 2, -10, MIDDLE_Z);
        static readonly Vector3 bSize = new Vector3((Configuration.TRACK_COUNT + 2) * Configuration.TRACK_WIDTH, 20, Configuration.DISTANCE_BETWEEN_OBJECT_ROWS);

        static readonly Vector3 lCenter = new Vector3(Configuration.TRACK_WIDTH * (Configuration.LEFT_TRACK_INDEX - Configuration.DEFAULT_TRACK_INDEX - 1), 50, MIDDLE_Z);
        static readonly Vector3 rCenter = new Vector3(Configuration.TRACK_WIDTH * (Configuration.RIGHT_TRACK_INDEX - Configuration.DEFAULT_TRACK_INDEX + 1), 50, MIDDLE_Z);
        static readonly Vector3 sideSize = new Vector3(Configuration.TRACK_WIDTH, 100, Configuration.DISTANCE_BETWEEN_OBJECT_ROWS);
        
        static BlockBorder visualizedBorder;
        
        void OnDrawGizmos() {
            // Gizmos are drawn in the same way, if 1 or more instances of the script exist in the scene.
            if (visualizedBorder == null)
                visualizedBorder = this;
            if (visualizedBorder != this)
                return;

            for (int i = 0; i < Configuration.OBJECT_LINES_IN_BLOCK; i++) {
                Gizmos.color = (i & 1) == 0 ? color : altColor;
                var offset = i * Configuration.DISTANCE_BETWEEN_OBJECT_ROWS * Vector3.back;
                // bottom part
                Gizmos.DrawCube(bCenter  + offset, bSize);
                // sides
                Gizmos.DrawCube(lCenter + offset, sideSize);
                Gizmos.DrawCube(rCenter + offset, sideSize);
            }
        }

        // Always stay in zero position, to match Gizmos
        void OnValidate() {
            transform.Translate(-transform.position);
        }
    }
}
