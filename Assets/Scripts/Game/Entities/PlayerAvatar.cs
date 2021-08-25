using System;
using System.Collections;
using UnityEngine;

namespace Runner.Game
{
    [RequireComponent(typeof(Rigidbody))]
    class PlayerAvatar : MonoBehaviour, IGameProgress
    {
        public event Action OnDeath, OnCoinHit;

        [SerializeField] Animator modelAnimator;

        static readonly int runStateID = Animator.StringToHash("Running");
        static readonly int aliveStateID = Animator.StringToHash("Alive");
        
        float currentSpeed;
        new Rigidbody rigidbody;
        int currentTrackNumber; // todo: coroutine based line swap
        IPlayerInput input;
        bool isAlive;

        public float Distance => transform.position.z;

        public IEnumerator Init() {
            rigidbody = GetComponent<Rigidbody>();
            input     = ServiceProvider.Get<IPlayerInput>();
            yield return Restart();
        }
        
        public IEnumerator Restart() {
            SetSpeed(Configuration.INITIAL_SPEED);
            rigidbody.MovePosition(Vector3.zero);
            currentTrackNumber = Configuration.DEFAULT_TRACK_INDEX;
            isAlive            = true;
            modelAnimator.SetBool(aliveStateID, true); // default state
            yield return new WaitForSeconds(1);        // this represents a delay required for initial animation
            modelAnimator.SetBool(runStateID, true); // running state
        }
        
        void OnCollisionEnter(Collision other) {
            var spawned = other.gameObject.GetComponent<IObjectOnTrack>();
            if (spawned == null) return;
            
            switch (spawned.GetObjectType()) {
                case ObjectType.Coin: 
                    OnCoinHit?.Invoke();
                    break;
                case ObjectType.Obstacle:
                    Die();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        void FixedUpdate() {
            if (!isAlive) return;
            
            SetSpeed(currentSpeed + (Configuration.MAX_SPEED - currentSpeed) * Configuration.ASYMPTOTIC_SPEED_GAIN_PER_FRAME);

            int inputValue = input.GetInputValue();
            int moveToTrack = inputValue + currentTrackNumber;
            if (moveToTrack < Configuration.LEFT_TRACK_INDEX || moveToTrack > Configuration.RIGHT_TRACK_INDEX || moveToTrack == currentTrackNumber)
                return;

            currentTrackNumber = moveToTrack;
            Vector3 offset = new Vector3(inputValue * Configuration.SINGLE_TRACK_WIDTH, 0);
            rigidbody.MovePosition(rigidbody.position + offset); //todo: travel time?
        }

        void SetSpeed(float speed) {
            currentSpeed       = speed;
            rigidbody.velocity = new Vector3(0, 0, currentSpeed);
        }

        void Die() {
            isAlive = false;
            SetSpeed(0);
            modelAnimator.SetBool(aliveStateID, false); // very dead state
            modelAnimator.SetBool(runStateID, false);
            OnDeath?.Invoke();
        }
    }
}
