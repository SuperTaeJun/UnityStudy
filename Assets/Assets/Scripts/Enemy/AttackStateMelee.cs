using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMelee : EnemyState
{
    private EnemyMelee Enemy;
    private Vector3 AttackDir;
    private float AttackMoveSpeed;

    private const float MaxAttackDistance = 50f;

    public AttackStateMelee(Enemy BaseEnemy, EnemyStateMachine StateMachine, string animBoolName) : base(BaseEnemy, StateMachine, animBoolName)
    {
        Enemy = BaseEnemy as EnemyMelee;
    }

    public override void Enter()
    {   
        base.Enter();
        Enemy.PulledWeaponActive();

        AttackMoveSpeed = Enemy.AttackData.MoveSpeed;

        Enemy.Animator.SetFloat("AttackAnimSpeed", Enemy.AttackData.AnimSpeed);
        Enemy.Animator.SetFloat("AttackIndex", Enemy.AttackData.AttackIndex);
        Enemy.Animator.SetFloat("SlashAttackIndex", Random.Range(0, 4));

        Enemy.Agent.isStopped = true;
        Enemy.Agent.velocity = Vector3.zero;

        AttackDir = Enemy.transform.position + (Enemy.transform.forward * MaxAttackDistance);
    }

    public override void Exit()
    {
        base.Exit();
        SetupNextAttack();
    }

    private void SetupNextAttack()
    {
        float RecoveryIndex = PlayerClose() ? 1 : 0;
        Enemy.Animator.SetFloat("RecoveryIndex", RecoveryIndex);

        Enemy.AttackData = UpdatedAttackData();
    }

    public override void Update()
    {
        base.Update();

        if(Enemy.ManualRotationActive())
        {
            Enemy.transform.rotation = Enemy.ForwardTarget(Enemy.Player.position);
        }

        if (Enemy.ManualMovementActive())
        {
            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, AttackDir, AttackMoveSpeed * Time.deltaTime);
        }
        if (TriggerCalled)
        {
            if (Enemy.PlayerInAttckRange())
                StateMachine.ChangeState(Enemy.RecoveryStateMelee);
            else
                StateMachine.ChangeState(Enemy.ChaseStateMelee);
        }

    }

    public bool PlayerClose() => Vector3.Distance(Enemy.transform.position, Enemy.Player.position) <= 1;

    private AttackData UpdatedAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(Enemy.AttackDatas);

        if (PlayerClose())
        {
            validAttacks.RemoveAll(parameter => parameter.AttackType == AttackType_Melee.Charge);
        }
        int random = Random.Range(0,validAttacks.Count);

        return validAttacks[random];
    }
}
