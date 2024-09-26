using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : EnemyBaseState
{
    public EnemyChase(EnemyStateMachine currentContext, EnemyStateFactory factory) : base(currentContext, factory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        // PlayerPrefs.SetString($"{CurrentContext.name}_isEnemyReadyToAttack", "true");
        Debug.Log($"CHASING ~");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
        ChasePlayer(_player);

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
        float distance = Vector3.Distance(CurrentContext.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distance <= 7f)
            SwitchState(Factory.EnemyAttack());
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    void ChasePlayer(Transform _player)
    {
        var lookPos = _player.position - CurrentContext.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        CurrentContext.transform.rotation = Quaternion.Slerp(CurrentContext.transform.rotation, rotation, Time.deltaTime * .7f);

        CurrentContext._navMeshAgent.SetDestination(_player.position);
    }
}
