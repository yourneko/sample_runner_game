using System;
using System.Collections;
using UnityEngine;

namespace Runner.Game
{
    [RequireComponent(typeof(Rigidbody))]
    class PlayerAvatar : MonoBehaviour, IGameProgress
    {
        static readonly int runStateID = Animator.StringToHash("Running");
        static readonly int aliveStateID = Animator.StringToHash("Alive");

        public event Action OnDeath, OnCoinHit;

        [SerializeField] Animator modelAnimator;
        [SerializeField] Transform camTransform;
        [SerializeField] Rigidbody rig;
        [SerializeField] float cameraMovementMultiplier = 1f;

        IPlayerInput input;
        int currentTrack, desiredTrack;
        bool isAlive;

        public float Distance => transform.position.z;
        float DesiredPositionX => (desiredTrack - Configuration.DEFAULT_TRACK_INDEX) * Configuration.TRACK_WIDTH;

        public IEnumerator Init() {
            input = ServiceProvider.Get<IPlayerInput>();
            yield return Restart();
        }

        public IEnumerator Restart() {
            currentTrack = desiredTrack = Configuration.DEFAULT_TRACK_INDEX;
            rig.MovePosition(Vector3.zero);
            rig.velocity                  = new Vector3(0, 0, Configuration.INITIAL_SPEED);
            isAlive                       = true;
            camTransform.localPosition = Vector3.zero;
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
                desiredTrack = currentTrack;
                rig.velocity = Vector3.zero;
                modelAnimator.SetBool(aliveStateID, false);
                modelAnimator.SetBool(runStateID, false);
                OnDeath?.Invoke();
            }
        }

        void FixedUpdate() {
            if (!isAlive) return;

            // slow down when close to desired X position, to prevent an overshooting
            rig.velocity = new Vector3(Mathf.Lerp(0, Configuration.TRACK_SWITCH_SPEED * desiredTrack.CompareTo(currentTrack),
                                                  Mathf.Abs(DesiredPositionX - rig.position.x) / Configuration.TRACK_SWITCH_SPEED * Time.fixedTime),
                                       0,
                                       (Configuration.MAX_SPEED - rig.velocity.z) * Configuration.ASYMPTOTIC_SPEED_GAIN_PER_FRAME);

            if (currentTrack != desiredTrack) // check if switching tracks
                camTransform.localPosition = new Vector3(transform.localPosition.x * (cameraMovementMultiplier - 1), 0);
            else
                CheckInputs(); // inputs are ignored until switching tracks is finished
        }

        void CheckInputs() {
            int moveToTrack = input.GetInputValue() + currentTrack;
            if (!(moveToTrack < Configuration.LEFT_TRACK_INDEX || moveToTrack > Configuration.RIGHT_TRACK_INDEX || moveToTrack == currentTrack))
                StartCoroutine(SwitchTrack(moveToTrack));
        }

        IEnumerator SwitchTrack(int trackIndex) {
            desiredTrack = trackIndex;
            while ((desiredTrack < currentTrack) ^ (DesiredPositionX > rig.position.x)) // crossing the desired X is the condition being checked
                yield return null;
            currentTrack = desiredTrack;
        }
    }
}
