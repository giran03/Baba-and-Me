using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private EnemyStateMachine _ctx;
    private EnemyStateFactory _factory;
    private EnemyBaseState _currentSubState;
    private EnemyBaseState _currentSuperState;

    string _currentAnimation = "";

    public bool IsRootState
    {
        get => _isRootState;
        set => _isRootState = value;
    }

    public EnemyStateMachine CurrentContext
    {
        get => _ctx;
        set => _ctx = value;
    }

    public EnemyStateFactory Factory
    {
        get => _factory;
        set => _factory = value;
    }

    public EnemyBaseState CurrentSubState
    {
        get => _currentSubState;
        set => _currentSubState = value;
    }

    public EnemyBaseState CurrentSuperState
    {
        get => _currentSuperState;
        set => _currentSuperState = value;
    }

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnDisable();
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

    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else
            _currentSuperState?.SetSubState(newState);
    }
    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(EnemyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    public void ChangeAnimation(string animationName, bool flipSprite = false)
    {
        CurrentContext.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = flipSprite;

        if (!_currentAnimation.Contains(animationName))
        {
            foreach (var newAnimation in CurrentContext.AnimationList)
                if (newAnimation.name.Contains(animationName))
                {
                    _currentAnimation = newAnimation.name;
                    CurrentContext.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = newAnimation;
                }


        }
    }

    public void CheckAnimation()
    {
        if (_currentAnimation.Contains("attack"))
            return;

        if (CurrentContext._navMeshAgent.velocity.x > 1.2f)
            ChangeAnimation("run_side");
        else if (CurrentContext._navMeshAgent.velocity.x < -1.2f)
            ChangeAnimation("run_side", true);
        else if (CurrentContext._navMeshAgent.velocity.y != 0)
            ChangeAnimation("run_up");
    }
}