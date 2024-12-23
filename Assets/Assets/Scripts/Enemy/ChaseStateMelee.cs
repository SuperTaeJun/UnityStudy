using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    private float LastTimeDestination;
    public ChaseStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy= BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        Enemy.Agent.isStopped = false;
        Enemy.Agent.speed = Enemy.ChaseSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Enemy.transform.rotation = Enemy.ForwardTarget(Enemy.Agent.steeringTarget);

        if(CanUpdateDestination())
        {
            Enemy.Agent.destination = Enemy.Player.transform.position;
        }

        //어택 스테이트
    }

    private bool CanUpdateDestination()
    {
        if(Time.time > LastTimeDestination + 0.25f)
        {
            LastTimeDestination= Time.time;
            return true;
        }
        return false;
    }
}
