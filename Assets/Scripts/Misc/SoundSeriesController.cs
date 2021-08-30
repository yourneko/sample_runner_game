using UnityEngine;

namespace Runner
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSeriesController : MonoBehaviour
    {
        const float PITCH_FOR_CHAIN_STACK = 0.015f;
        const float CHAIN_BREAK_DELAY = 0.3f;

        [SerializeField] AudioClip clip;
        AudioSource source;
        int chain = -1;
        float timeRemains;

        public void PlaySound() {
            source.pitch = ++chain * PITCH_FOR_CHAIN_STACK + 1;
            timeRemains  = CHAIN_BREAK_DELAY;
            source.Play();
        }

        void Start() {
            source             = GetComponent<AudioSource>();
            source.clip        = clip;
            source.playOnAwake = false;
            source.loop        = false;
        }

        void Update() {
            if (timeRemains > 0 && (timeRemains -= Time.deltaTime) < 0)
                chain = -1;
        }
    }
}
