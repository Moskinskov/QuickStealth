using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
    [SerializeField, Header("Patrol properties")] private float moveRadius = 10;
    [SerializeField] private float minMoveDelay = 4;
    [SerializeField] private float maxMoveDelay = 10;
    [SerializeField] private float viewDistance = 5;
    [SerializeField] private float _angleOfView = 30;
    private Vector3 startPosition;
    private Vector3 currentDestination;
    private float changePosTime;

    private Player _player;
    private NavMeshAgent _navMeshAgent;
    private Light _light;
    private bool isHearPlayer;
    private bool isSeePlayer;
    private bool disabled;

    public event Action OnEventHearingThePlayer;
    public event Action OnEventSeingThePlayer;

    private void Start()
    {
        startPosition = transform.position;
        changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        _player = FindObjectOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _light = GetComponentInChildren<Light>();
        _light.spotAngle = _angleOfView;

        OnEventHearingThePlayer += HearThePlayer;
        OnEventSeingThePlayer += SeeThePlayer;
        _player.OnWinEvent += GameOver;
        _player.OnGameOverEvent += GameOver;
        disabled = false;
    }
    protected void Update()
    {
        if (!disabled)
        {
            TryToFindPlayer();
            if (!isSeePlayer && !isHearPlayer)
            {
                Wandering(Time.deltaTime);
                LoseThePlayer();
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    #region Patrol methods

    /// <summary>
    /// Блуждание
    /// </summary>
    /// <param name="deltaTime">спустя "время"</param>
    private void Wandering(float deltaTime)
    {
        changePosTime -= deltaTime;
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
        _navMeshAgent.SetDestination(currentDestination);
    }

    private void TryToFindPlayer()
    {
        Vector3 tempVector = _player.transform.position - transform.position;
        float tempAngle = Vector3.Angle(transform.forward, tempVector);

        if (tempVector.sqrMagnitude <= Mathf.Pow(viewDistance, 2))
        {
            OnEventHearingThePlayer?.Invoke();
            if (tempAngle <= _angleOfView / 2)
            {
                OnEventSeingThePlayer?.Invoke();
            }
        }
        else
        {
            isHearPlayer = false;
            isSeePlayer = false;
        }
    }

    #endregion

    #region Reactions

    private void HearThePlayer()
    {
        isHearPlayer = true;
        _light.color = Color.yellow;
        _navMeshAgent.SetDestination(transform.position);

        transform.Rotate(new Vector3(transform.position.x, transform.position.y + 100 * Time.deltaTime));
    }
    private void SeeThePlayer()
    {
        isSeePlayer = true;
        _light.color = Color.red;
        _navMeshAgent.speed = 10;
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    private void LoseThePlayer()
    {
        _navMeshAgent.speed = 4;
        _light.color = Color.white;
    }

    #endregion

    private void GameOver()
    {
        disabled = true;
    }
}
