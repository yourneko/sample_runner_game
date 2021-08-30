using System;
using System.Collections;
using UnityEngine;

namespace Runner.Game
{
    class PlayerAvatar : MonoBehaviour, IGameProgress
    {
        static readonly int runStateID = Animator.StringToHash("Running");
        static readonly int aliveStateID = Animator.StringToHash("Alive");
        static readonly Vector3 startPosition = new Vector3(0, 0, Configuration.START_POSITION_Z);

        public event Action OnDeath, OnCoinHit;

        [SerializeField] Animator modelAnimator;
        [SerializeField] Rigidbody rig;
        [SerializeField] Transform camTransform;
        [SerializeField] float cameraMovementMultiplier = 1f;
        [SerializeField] AudioSource deathAudioSource;
        [SerializeField] SoundSeriesController coinAudioSource;

        IPlayerInput input;
        int currentTrack, desiredTrack;
        float speedZ;
        bool isAlive;

        public float Distance => rig.position.z;
        float DesiredPositionX => (desiredTrack - Configuration.DEFAULT_TRACK_INDEX) * Configuration.TRACK_WIDTH;

        public IEnumerator Init() {
            input           =  ServiceProvider.Get<IPlayerInput>();
            rig.isKinematic =  true;
            OnCoinHit       += coinAudioSource.PlaySound;
            OnDeath         += deathAudioSource.Play;
            yield return Restart();
        }

        public IEnumerator Restart() {
            currentTrack = desiredTrack = Configuration.DEFAULT_TRACK_INDEX;
            isAlive      = true;
            speedZ       = Configuration.INITIAL_SPEED;
            rig.position = startPosition;
            camTransform.Translate(-camTransform.localPosition);
            modelAnimator.SetBool(aliveStateID, true); // default state

            yield return new WaitForSeconds(1);        // this represents a delay required for initial animation
            modelAnimator.SetBool(runStateID, true);   // running state
        }

        void OnCollisionEnter(Collision other) {
            var hit = other.gameObject.GetComponent<IObjectOnTrack>()?.GetObjectType() ?? ObjectType.None;
            if (hit == ObjectType.Coin)
                OnCoinHit?.Invoke();
            else if (hit == ObjectType.Obstacle) {
                isAlive      = false;
                modelAnimator.SetBool(aliveStateID, false);
                modelAnimator.SetBool(runStateID, false);
                OnDeath?.Invoke();
            }
        }

        void FixedUpdate() {
            if (!isAlive) return;

            float baseSpeedX = (DesiredPositionX - rig.position.x) / Time.fixedDeltaTime;
            float speedX     = Mathf.Min(Configuration.TRACK_SWITCH_SPEED, Mathf.Abs(baseSpeedX)) * baseSpeedX.CompareTo(0);
            speedZ += (Configuration.MAX_SPEED - speedZ) * Configuration.ASYMPTOTIC_SPEED_GAIN_PER_FRAME;
            rig.position += new Vector3(speedX * Time.fixedDeltaTime, 0, speedZ * Time.fixedDeltaTime);

            // When switching tracks, inputs are ignored, but camera position is updated
            if (currentTrack != desiredTrack)
                camTransform.localPosition = new Vector3(rig.position.x * (cameraMovementMultiplier - 1), 0);
            // Checking inputs. Camera position can't change, if tracks are not changed.
            else {
                int moveToTrack = input.GetInputValue() + currentTrack;
                if (!(moveToTrack < Configuration.LEFT_TRACK_INDEX || moveToTrack > Configuration.RIGHT_TRACK_INDEX || moveToTrack == currentTrack))
                    StartCoroutine(SwitchTrack(moveToTrack));
            }
        }

        IEnumerator SwitchTrack(int trackIndex) {
            desiredTrack = trackIndex;
            while (Mathf.Abs(DesiredPositionX - rig.position.x) > 1f)
                yield return null;
            currentTrack = desiredTrack;
        }
    }
}
