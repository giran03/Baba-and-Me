using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Configs")]
    public float patrolRadius = 5f;
    public float detectionRadius = 10f;
    public float attackRate = 2.5f;

    [Header("Attack")]
    public GameObject attackPrefab;

    [Header("Orc Animation")]
    public List<AnimatorController> AnimationList;
    public List<int> AnimationHashCodes;
    public Dictionary<string, int> AnimationDictionary = new();

    [HideInInspector] public NavMeshAgent _navMeshAgent;
    [HideInInspector] public bool isAttacking;

    // state variables
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    // references
    DamageableStats damageableStats;

    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        _states = new(this);
        _currentState = _states.EnemyPatrol();
        _currentState.EnterState();
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        foreach (var animation in AnimationList)
            AnimationDictionary.Add(animation.name, animation.GetHashCode());
        // AnimationHashCodes.Add(animation.GetHashCode());
    }

    void Update()
    {
        // states
        _currentState.UpdateStates();
    }

    void OnDisable()
    {
        _currentState.OnDisable();
    }
}
