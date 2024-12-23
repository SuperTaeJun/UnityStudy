using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState
{
    protected Enemy BaseEnemy;
    protected EnemyStateMachine StateMachine;
    protected string AnimBoolName;
    protected float StateTimer;

    protected bool TriggerCalled;
    public EnemyState(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName)
    {
        this.BaseEnemy = BaseEnemy;
        this.StateMachine = StateMachine;
        this.AnimBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        BaseEnemy.Animator.SetBool(AnimBoolName, true);

        TriggerCalled = false;
    }
    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;
    }
    public virtual void Exit() 
    {
        BaseEnemy.Animator.SetBool(AnimBoolName, false);
    }  

    public void AnimationTrigger() => TriggerCalled = true;
}
