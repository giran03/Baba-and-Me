using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : PlayerBaseState
{
    public PlayerAttacking(PlayerStateMachine currentContext, PlayerStateFactory factory) : base(currentContext, factory)
    {
    }

    public override void CheckSwitchStates()
    {

    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Attack State");
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void OnTriggerEnter()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log($"BASIC ATTACK!");
    }
}
