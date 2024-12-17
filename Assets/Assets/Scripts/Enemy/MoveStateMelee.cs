using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    private Vector3 Destination;
    public MoveStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        Destination = Enemy.GetPatrolPoint();
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("Exit MoveState");
    }

    public override void Update()
    {
        base.Update();
        Enemy.Agent.SetDestination(Destination);

        if(Enemy.Agent.remainingDistance <= 1 )
        {
            StateMachine.ChangeState(Enemy.IdleStateMelee);
        }




    }
}
