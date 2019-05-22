using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.GuardItems
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPatrol : GuardItem
    {
        [SerializeField, Header("Patrol properties")] private float moveRadius = 10;
        [SerializeField] private float maxMoveSpeed = 10;
        [SerializeField] private float minMoveDelay = 4;
        [SerializeField] private float maxMoveDelay = 10;

        private float origMoveSpeed;


        private Vector3 startPosition;
        private Vector3 currentDestination;
        private float changePosTime;

        private NavMeshAgent _navMeshAgent;

        #region Properties

        public NavMeshAgent MeshAgent
        {
            get { return _navMeshAgent; }
            set => _navMeshAgent = value;
        }

        public float OrigMoveSpeed
        {
            get { return origMoveSpeed; }
        }

        public float MaxMoveSpeed
        {
            get { return maxMoveSpeed; }
        }

        #endregion


        #region On'Unity's methods

        public override void OnStart()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            startPosition = transform.position;
            changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
            origMoveSpeed = _navMeshAgent.speed;
        }

        public override void OnUpdate()
        {
            Wandering();
        }

        #endregion

        #region Patrol methods

        
        private void Wandering()
        {
            changePosTime -= Time.deltaTime;
            if (changePosTime <= 0)
            {
                RandomMove();
                changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
            }
        }

        private void RandomMove()
        {
            currentDestination =
                Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * new Vector3(moveRadius, 0, 0) + startPosition;
            MeshAgent.SetDestination(currentDestination);
        }

        #endregion
    }
}