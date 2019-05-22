using System;
using UnityEngine;

namespace Assets.Scripts.GuardItems
{
    public class EnemyBrain : GuardItem
    {
        [SerializeField] private float _viewDistance = 5;
        [SerializeField] private float _angleOfView = 30;

        private Player _player;
        private Guard _guard;

        private bool _isSeeThePlayer;
        private bool _isHearThePlayer;

        public event Action OnEventSeingThePlayer;
        public event Action OnEventHearingThePlayer;
        public event Action OnEventLoseThePlayer;


        #region Properties

        public Player PlayerRef
        {
            get { return _player; }
            set { _player = value; }
        }

        public float ViewDistance
        {
            get { return _viewDistance; }
        }

        public float AngleOfView
        {
            get { return _angleOfView; }
        }

        public Guard GuardRef
        {
            get { return _guard; }
            set { _guard = value; }
        }

        public bool IsSeeThePlayer
        {
            get { return _isSeeThePlayer; }
        }

        public bool IsHearThePlayer
        {
            get { return _isHearThePlayer; }
        }

        #endregion


        public override void OnStart()
        {
            PlayerRef = FindObjectOfType<Player>();
            GuardRef = GetComponent<Guard>();
        }

        public override void OnUpdate()
        {
            Vector3 tempVector = PlayerRef.transform.position - GuardRef.transform.position;
            float tempAngle = Vector3.Angle(GuardRef.transform.forward, tempVector);

            _isHearThePlayer = tempVector.sqrMagnitude <= Mathf.Pow(_viewDistance, 2) + 1 && !_player._playerDontMove;

            if (IsHearThePlayer)
            {
                OnEventHearingThePlayer?.Invoke();

                if (tempAngle <= _angleOfView / 2)
                {
                    _isSeeThePlayer = !Physics.Linecast(GuardRef.transform.position, PlayerRef.transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
                    if (IsSeeThePlayer)
                    {
                        OnEventSeingThePlayer?.Invoke();
                    }
                }
            }
            else
            {
                OnEventLoseThePlayer?.Invoke();
            }
        }


    }
}