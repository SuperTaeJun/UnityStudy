using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    public AbilityStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
