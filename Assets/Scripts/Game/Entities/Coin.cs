using UnityEngine;

namespace Runner.Game
{
    class Coin : MonoBehaviour, IObjectOnTrack
    {
        [SerializeField] Transform visuals;
        bool isSpinning;

        void OnEnable() {
            isSpinning = true;
            visuals.rotation = Quaternion.identity;
        }

        void Update() {
            if (!isSpinning)
                return;
            visuals.Rotate(Vector3.up, Time.deltaTime * Configuration.COIN_ROTATION_DEGREES_PER_SECOND);
        }

        public ObjectType GetObjectType() {
            transform.Translate(Configuration.CoinDisappearanceTranslation);
            isSpinning = false;
            return ObjectType.Coin;
        }
    }
}
