using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        Enemy.Agent.speed = Enemy.MoveSpeed;

        Destination = Enemy.GetPatrolPoint();
        Enemy.Agent.SetDestination(Destination);

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (Enemy.PlayerInAggresionRange())
        {
            StateMachine.ChangeState(Enemy.RecoveryStateMelee);
            return;
        }
        Enemy.transform.rotation = Enemy.ForwardTarget(Enemy.Agent.steeringTarget);

        if(Enemy.Agent.remainingDistance <= Enemy.Agent.stoppingDistance +0.05f)
        {
            StateMachine.ChangeState(Enemy.IdleStateMelee);
        }


    }
    //public Vector3 GetNextPathPoint()
    //{
    //    NavMeshAgent Agent = Enemy.Agent;
    //    NavMeshPath Path = Agent.path;

    //    if(Path.corners.Length < 2)
    //    {
    //        return Agent.destination;
    //    }

    //    for(int i = 0; i < Path.corners.Length; i++)
    //    {
    //        if (Vector3.Distance(Agent.transform.position, Path.corners[i]) < 1)
    //        {
    //            return Path.corners[i+1];
    //        }

    //    }
    //    return Agent.destination;

    //}

}
