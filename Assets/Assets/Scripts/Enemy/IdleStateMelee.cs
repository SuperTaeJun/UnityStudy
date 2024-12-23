using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateMelee : EnemyState
{
    private EnemyMelee Enemy;

    public IdleStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        StateTimer = Enemy.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (Enemy.PlayerInRange())
        {
            StateMachine.ChangeState(Enemy.RecoveryStateMelee);
            return;
        }

        if (StateTimer < 0)
        {
            StateMachine.ChangeState(Enemy.MoveStateMelee);
        }
    }

}
