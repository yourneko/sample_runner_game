using UnityEngine;

namespace Runner.Game
{
    class Obstacle : MonoBehaviour, IObjectOnTrack
    {
        public ObjectType GetObjectType() => ObjectType.Obstacle;
    }
}
