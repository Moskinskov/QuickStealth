using Assets.Scripts.GuardItems;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPatrol), typeof(EnemyBrain))]
public class Guard : MonoBehaviour
{
    [SerializeField] private GameObject seeEffectsGO;
    [SerializeField] private GameObject hearEffectsGO;

    private EnemyBrain _brain;
    private EnemyPatrol _patrol;
    private List<ParticleSystem> seeEffects = new List<ParticleSystem>();
    private List<ParticleSystem> hearEffects = new List<ParticleSystem>();
    private Light _light;
    private bool disabled;

    private void Start()
    {
        _brain = GetComponent<EnemyBrain>();
        _patrol = GetComponent<EnemyPatrol>();

        _brain?.OnStart();
        _patrol?.OnStart();

        _brain.PlayerRef.OnWinEvent += GameOver;
        _brain.PlayerRef.OnGameOverEvent += GameOver;
        _brain.OnEventHearingThePlayer += HearThePlayer;
        _brain.OnEventSeingThePlayer += SeeThePlayer;
        _brain.OnEventLoseThePlayer += LoseThePlayer;

        _light = GetComponentInChildren<Light>();
        _light.spotAngle = _brain.AngleOfView;

        foreach (var effect in seeEffectsGO.GetComponentsInChildren<ParticleSystem>())
        {
            seeEffects.Add(effect);
            effect.gameObject.SetActive(false);
        }
        foreach (var effect in hearEffectsGO.GetComponentsInChildren<ParticleSystem>())
        {
            hearEffects.Add(effect);
            effect.gameObject.SetActive(false);
        }

        disabled = false;
    }

    private void OnValidate()
    {
        _brain = GetComponent<EnemyBrain>();
        _patrol = GetComponent<EnemyPatrol>();
    }
    protected void Update()
    {
        if (!disabled)
        {
            _brain?.OnUpdate();
            _patrol?.OnUpdate();
        }
    }

    #region Reactions

    private void HearThePlayer()
    {
        _light.color = Color.yellow;
        _patrol.MeshAgent.SetDestination(transform.position);
        transform.Rotate(new Vector3(0, transform.position.y + 100 * Time.deltaTime));
        foreach (var effect in hearEffects)
        {
            if (!effect.IsAlive())
            {
                effect.gameObject.SetActive(true);
                if (!effect.isPlaying)
                    effect.Play();
            }
        }
        foreach (var effect in seeEffects)
        {
            effect.gameObject.SetActive(false);
        }
    }
    private void SeeThePlayer()
    {
        _light.color = Color.red;
        _patrol.MeshAgent.speed = _patrol.MaxMoveSpeed;
        _patrol.MeshAgent.SetDestination(_brain.PlayerRef.transform.position);

        foreach (var effect in seeEffects)
        {
            if (!effect.IsAlive())
            {
                effect.gameObject.SetActive(true);
                if (!effect.isPlaying)
                    effect.Play();
            }
        }
        foreach (var effect in hearEffects)
        {
            effect.gameObject.SetActive(false);
        }
    }
    private void LoseThePlayer()
    {
        _patrol.MeshAgent.speed = _patrol.OrigMoveSpeed;
        _light.color = Color.white;
        foreach (var effect in seeEffects)
        {
            effect.gameObject.SetActive(false);
        }
        foreach (var effect in hearEffects)
        {
            effect.gameObject.SetActive(false);
        }
    }

    #endregion

    private void GameOver()
    {
        disabled = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _brain.ViewDistance);
    }
}
