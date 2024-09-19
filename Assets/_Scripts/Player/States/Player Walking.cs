using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerWalking : PlayerBaseState
{
    public PlayerWalking(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            SwitchState(Factory.Idle());
    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Walk State");
        AnimationUpdate("walk_x");
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Attacking());
    }

    public override void OnTriggerEnter()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
