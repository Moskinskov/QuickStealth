using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private GameObject stepEffectGO;

        private float smoothMoveTime = 0.1f;
        private float turnSpeed = 8;
        private float angle;
        private float smoothInputMagnitude;
        private float smoothmoveVelocity;
        private Vector3 velocity;
        private List<ParticleSystem> stepEffects = new List<ParticleSystem>();

        private Rigidbody _rigidbody;
        private bool isCatched;
        public event Action OnGameOverEvent;
        public event Action OnWinEvent;
        public bool _playerDontMove = true;


        private void Awake()
        {
            isCatched = false;
            _rigidbody = GetComponent<Rigidbody>();
            foreach (var effect in stepEffectGO.GetComponentsInChildren<ParticleSystem>())
            {
                effect.Stop();
                stepEffects.Add(effect);
            }
        }

        private void Update()
        {
            if (!isCatched)
            {
                MovingChecks();
            }
        }


        private void FixedUpdate()
        {
            if (!isCatched)
            {
                Move();
            }
        }


        private void MovingChecks()
        {
            Vector3 tempVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            tempVector = tempVector.normalized;

            var inputMagnitude = tempVector.magnitude;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothmoveVelocity,
                smoothMoveTime);
            var targetAngle = Mathf.Atan2(tempVector.x, tempVector.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
            velocity = transform.forward * movementSpeed * smoothInputMagnitude;

            _playerDontMove = Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;
            if (!_playerDontMove)
            {
                foreach (var effect in stepEffects)
                {
                    if (!effect.isPlaying)
                    {
                        effect.Play();
                    }
                }
            }
        }
        private void Move()
        {
            _rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            _rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                isCatched = true;
                OnGameOverEvent?.Invoke();
            }
            if (collision.gameObject.tag == "Finish")
            {
                isCatched = true;
                OnWinEvent?.Invoke();
            }
        }

    }
}