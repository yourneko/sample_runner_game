using UnityEngine;

namespace Runner.Game
{
    class Coin : MonoBehaviour, IObjectOnTrack
    {
        public ObjectType GetObjectType() {
            transform.Translate(Configuration.CoinDisappearanceTranslation);
            return ObjectType.Coin;
        }
    }
}
