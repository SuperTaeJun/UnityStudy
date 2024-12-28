using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateMelee : EnemyState
{
    private EnemyMelee Enemy;

    private Vector3 MovementDir;
    private const float MaxMovementDistance = 20f;
    private float MoveSpeed;

    public AbilityStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        Enemy.PulledWeaponActive();

        MoveSpeed = Enemy.MoveSpeed;
        MovementDir = Enemy.transform.position + (Enemy.transform.forward * MaxMovementDistance);
    }

    public override void Exit()
    {
        base.Exit();
        Enemy.MoveSpeed = MoveSpeed;
        Enemy.Animator.SetFloat("RecoveryIndex", 0f);
    }

    public override void Update()
    {
        base.Update();


        if (Enemy.ManualRotationActive())
        {
            Enemy.transform.rotation = Enemy.ForwardTarget(Enemy.Player.position);
            MovementDir = Enemy.transform.position + (Enemy.transform.forward * MaxMovementDistance);
        }

        if (Enemy.ManualMovementActive())
        {
            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, MovementDir, Enemy.MoveSpeed * Time.deltaTime);
        }
        if(TriggerCalled)
        {
            StateMachine.ChangeState(Enemy.RecoveryStateMelee);
        }
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();


        GameObject NewAxe = ObjectPool.instance.GetObject(Enemy.AxePrefab);

        NewAxe.transform.position = Enemy.AxeStartPoint.position;
        NewAxe.GetComponent<EnemyAxe>().AxeSetup(Enemy.AxeFlySpeed, Enemy.Player, Enemy.AxeTimer);
    }
}
