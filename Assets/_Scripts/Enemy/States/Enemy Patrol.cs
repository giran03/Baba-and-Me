using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : EnemyBaseState
{
    Collider[] _hitColliders;

    public EnemyPatrol(EnemyStateMachine currentContext, EnemyStateFactory factory) : base(currentContext, factory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log($"ENEMY PATROLING~");
        PlayerPrefs.SetString($"{CurrentContext.name}_isEnemyReadyToAttack", "true");
    }

    public override void UpdateState()
    { 
        CheckSwitchStates();
        Patrol();

        CheckAnimation();
    }

    public override void ExitState()
    {

    }

    public override void OnDisable()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (IsPlayerDetected())
            SwitchState(Factory.EnemyChase());
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    void Patrol()
    {
        if (!CurrentContext._navMeshAgent.isActiveAndEnabled) return;

        if (CurrentContext._navMeshAgent.remainingDistance <= 0.1f)
            if (RandomPoint(CurrentContext.transform.position, CurrentContext.patrolRadius, out Vector3 point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                CurrentContext._navMeshAgent.SetDestination(point);
            }
    }


    bool IsPlayerDetected()
    {
        Collider[] hitColliders = Physics.OverlapSphere(CurrentContext.transform.position, CurrentContext.detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
                return true;
        }
        return false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
