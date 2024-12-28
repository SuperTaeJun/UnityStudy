using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    public RecoveryStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        Enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Enemy.transform.rotation =  Enemy.ForwardTarget(Enemy.Player.position);

        if(TriggerCalled)
        {
            if(Enemy.CanThrowAxe())
                StateMachine.ChangeState(Enemy.AbilityStateMelee);
            else if (Enemy.PlayerInAttckRange())
                StateMachine.ChangeState(Enemy.AttackStateMelee);
            else
                StateMachine.ChangeState(Enemy.ChaseStateMelee);

        }
    }
}
