using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    private EnemyRagdoll Ragdoll;
    private bool InteractDisable;
    public DeadStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
        Ragdoll = Enemy.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();

        InteractDisable = false;

        Enemy.Animator.enabled = false;
        Enemy.Agent.isStopped = true;

        Ragdoll.RagdollActive(true);

        StateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(StateTimer < 0 && !InteractDisable)
        {
            InteractDisable = true;
            Ragdoll.RagdollActive(false);
            Ragdoll.CollidersActive(false);
        }
    }
}
