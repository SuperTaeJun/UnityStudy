using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public string AttackName;
    public float AttackRange;
    public float MoveSpeed;
    public float AttackIndex;
    [Range(1,3)]
    public float AnimSpeed;
    public AttackType_Melee AttackType;
}
public enum AttackType_Melee
{
    Close,
    Charge
}
public class EnemyMelee : Enemy
{

    public IdleStateMelee IdleStateMelee {  get; private set; }
    public MoveStateMelee MoveStateMelee { get; private set;}
    public RecoveryStateMelee RecoveryStateMelee { get; private set; }
    public ChaseStateMelee ChaseStateMelee { get; private set; }
    public AttackStateMelee AttackStateMelee { get; private set; }
    public DeadStateMelee DeadStateMelee { get; private set; }

    [Header("AttackData")]
    public AttackData AttackData;
    public List<AttackData> AttackDatas;

    protected override void Awake()
    {
        base.Awake();
        IdleStateMelee = new IdleStateMelee(this, StateMachine, "Idle");
        MoveStateMelee = new MoveStateMelee(this, StateMachine, "Move");
        RecoveryStateMelee = new RecoveryStateMelee(this, StateMachine, "Recovery");
        ChaseStateMelee = new ChaseStateMelee(this, StateMachine, "Chase");
        AttackStateMelee = new AttackStateMelee(this, StateMachine, "Attack");
        DeadStateMelee = new DeadStateMelee(this, StateMachine, "Idle"); //랙돌 사용
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Init(IdleStateMelee);
    }
    protected override void Update()
    {
        base.Update();

        StateMachine.CurState.Update();


    }
    public override void GetHit()
    {
        base.GetHit();

        if(Health <= 0)
            StateMachine.ChangeState(DeadStateMelee);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
    }

    public bool PlayerInAttckRange() => Vector3.Distance(transform.position, Player.transform.position) < AttackData.AttackRange;
}
