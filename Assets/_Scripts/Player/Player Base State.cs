using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Animations;
using UnityEngine;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    string _currentAnimation = "";

    public bool IsRootState
    {
        get => _isRootState;
        set => _isRootState = value;
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
    public abstract void OnTriggerEnter(Collider other);

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

        if (_isRootState)
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

    public void ChangeAnimation(string animationName, bool flipSprite = false)
    {
        CurrentContext.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = flipSprite;
        var animationList = PlayerConfigs.Instance.playerAnimationList;

        if (PlayerConfigs.Instance.isMisha)
            animationList = PlayerConfigs.Instance.MishaAnimationList;

        if (!_currentAnimation.Contains(animationName))
        {
            foreach (var newAnimation in animationList)
                if (newAnimation.name.Contains(animationName))
                {
                    _currentAnimation = newAnimation.name;
                    CurrentContext.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = newAnimation;
                }
        }
    }

    public void CheckAnimation()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        if (_currentAnimation.Contains("attack") || _currentAnimation.Contains("Idle") || _currentAnimation.Contains("Death"))
            return;

        if (x != 0 || y != 0)
        {
            if (x > 0)
                ChangeAnimation("Run_right");
            else if (x < 0)
                ChangeAnimation("Run_left");
            else if (y < 0)
                ChangeAnimation("Run_down");
            else if (y > 0)
                ChangeAnimation("Run_up");
        }
    }
}