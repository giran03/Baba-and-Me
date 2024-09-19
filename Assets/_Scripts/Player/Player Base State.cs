using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class PlayerBaseState
{
    private bool isRootState { get; set; }
    private PlayerStateMachine _ctx { get; set; }
    private PlayerStateFactory _factory { get; set; }
    private PlayerBaseState _currentSubState { get; set; }
    private PlayerBaseState _currentSuperState { get; set; }

    public bool IsRootState
    {
        get => isRootState;
        set => isRootState = value;
    }

    public PlayerStateMachine CurrentContext
    {
        get => _ctx;
        set => _ctx = value;
    }

    public PlayerStateFactory Factory
    {
        get => _factory;
        set => _factory = value;
    }

    public PlayerBaseState CurrentSubState
    {
        get => _currentSubState;
        set => _currentSubState = value;
    }

    public PlayerBaseState CurrentSuperState
    {
        get => _currentSuperState;
        set => _currentSuperState = value;
    }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();
    public abstract void OnTriggerEnter();

    public void UpdateStates()
    {
        UpdateState();
        _currentSubState?.UpdateStates();
    }

    public void ExitStates()
    {
        ExitState();
        _currentSubState?.ExitStates();
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (isRootState)
            _ctx.CurrentState = newState;
        else
            _currentSuperState?.SetSubState(newState);
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    public virtual void AnimationUpdate(string searchString)
    {
        Animator _animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();

        foreach (var animatorController in PlayerConfigs.Instance.playerAnimationList)
        {
            if (animatorController.name.Contains(searchString))
            {
                _animator.runtimeAnimatorController = animatorController;
                return;
            }
        }

        Debug.LogError($"Could not find animator controller with name containing '{searchString}'");
    }
}