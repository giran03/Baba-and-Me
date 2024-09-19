using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : PlayerBaseState
{
    public PlayerDashing(PlayerStateMachine currentContext, PlayerStateFactory factory) : base(currentContext, factory)
    {
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Dash State");
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
        
    }
}
