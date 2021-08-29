using UnityEngine;

namespace Runner.Game
{
    class Coin : MonoBehaviour, IObjectOnTrack
    {
        // keeping the collider on a same place while moving and rotating the mesh
        [SerializeField] Transform visuals;
        [SerializeField] Quaternion defaultRotation;
        bool active;

        void OnEnable() {
            active           = true;
            visuals.rotation = defaultRotation;
        }

        void Update() {
            if (active)
                visuals.Rotate(Vector3.forward, Time.deltaTime * Configuration.COIN_ROTATION_DEGREES_PER_SECOND);
        }

        public ObjectType GetObjectType() {
            transform.Translate(Configuration.CoinDisappearanceTranslation);
            active = false;
            return ObjectType.Coin;
        }
    }
}
