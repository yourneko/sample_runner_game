using System;
using System.Collections;
using Runner.Core;
using UnityEngine;

namespace Runner.Game
{
    [RequireComponent(typeof(Rigidbody))]
    class PlayerAvatar : MonoBehaviour, IGameProgress
    {
        public event Action OnDeath, OnCoinHit;

        //todo: SerializeField with character model. change animations of it
        [SerializeField] string runAnimName, deathAnimName, waitAnimName;
        
        float currentSpeed;
        new Rigidbody rigidbody;
        int currentTrackNumber;
        IPlayerInput input;
        
        public bool IsAlive { get; private set; }

        public float Distance => transform.position.z;
        Vector3 Velocity => new Vector3(0, 0, currentSpeed);

        public IEnumerator Init() {
            rigidbody = GetComponent<Rigidbody>(); // the mode is set to Kinematic
            input     = Services.Get<IPlayerInput>();
            IsAlive   = true;
            yield return Restart(); 
        }
        
        public IEnumerator Restart() {
            currentSpeed = Configuration.INITIAL_SPEED;
            rigidbody.MovePosition(Vector3.zero);
            currentTrackNumber = Configuration.DEFAULT_TRACK_INDEX;
            IsAlive            = true;
            yield break; // todo: delay on start
        }

        void OnCollisionEnter(Collision other) {
            var spawned = other.gameObject.GetComponent<IObjectOnTrack>();
            if (spawned == null) return;
            
            switch (spawned.GetObjectType()) {
                case ObjectType.Coin: 
                    OnCoinHit?.Invoke();
                    break;
                case ObjectType.Obstacle:
                    IsAlive = false;
                    rigidbody.velocity = Vector3.zero;
                    OnDeath?.Invoke();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        void FixedUpdate() {
            if (!IsAlive) return;
            
            currentSpeed       += (Configuration.MAX_SPEED - currentSpeed) * Configuration.ASYMPTOTIC_SPEED_GAIN_PER_FRAME;
            rigidbody.velocity =  Velocity;

            int inputValue = input.GetInputValue();
            int moveToTrack = inputValue + currentTrackNumber;
            if (moveToTrack < Configuration.LEFT_TRACK_INDEX || moveToTrack > Configuration.RIGHT_TRACK_INDEX || moveToTrack == currentTrackNumber)
                return;

            currentTrackNumber = moveToTrack;
            Vector3 offset = new Vector3(inputValue * Configuration.SINGLE_TRACK_WIDTH, 0);
            rigidbody.MovePosition(rigidbody.position + offset); //todo: travel time?
        }
    }
}
