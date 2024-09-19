using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : PlayerBaseState
{
    public PlayerHurt(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Hurt State");
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