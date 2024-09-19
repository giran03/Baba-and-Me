using UnityEngine;

public class PlayerIdle : PlayerBaseState
{
    public PlayerIdle(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Idle State");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        AnimationUpdate("idle");
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            SwitchState(Factory.Walking());
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Attacking());
    }

    public override void OnTriggerEnter()
    {

    }
}
