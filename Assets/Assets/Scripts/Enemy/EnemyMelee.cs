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

public enum EnemyMelee_Type
{
    Regular,
    Shield,
    Dodge,
    Axe
}


public class EnemyMelee : Enemy
{
    #region State
    public IdleStateMelee IdleStateMelee {  get; private set; }
    public MoveStateMelee MoveStateMelee { get; private set;}
    public RecoveryStateMelee RecoveryStateMelee { get; private set; }
    public ChaseStateMelee ChaseStateMelee { get; private set; }
    public AttackStateMelee AttackStateMelee { get; private set; }
    public DeadStateMelee DeadStateMelee { get; private set; }
    public AbilityStateMelee AbilityStateMelee { get; private set; }
    #endregion

    [Header("EnemyMelee Setting")]
    public EnemyMelee_Type MeleeType;
    [SerializeField] private Transform ShieldTransform;
    public float RollCooldown;
    private float LastRollTime = -10;

    [Header("AxeThrow Ability")]
    public GameObject AxePrefab;
    public float AxeFlySpeed;
    public float AxeTimer;
    public float AxeCooldown;
    public Transform AxeStartPoint;
    private float LastTimeAxeThrown;

    [Header("AttackData")]
    public AttackData AttackData;
    public List<AttackData> AttackDatas;


    [SerializeField] Transform PulledWeapon;
    protected override void Awake()
    {
        base.Awake();
        IdleStateMelee = new IdleStateMelee(this, StateMachine, "Idle");
        MoveStateMelee = new MoveStateMelee(this, StateMachine, "Move");
        RecoveryStateMelee = new RecoveryStateMelee(this, StateMachine, "Recovery");
        ChaseStateMelee = new ChaseStateMelee(this, StateMachine, "Chase");
        AttackStateMelee = new AttackStateMelee(this, StateMachine, "Attack");
        DeadStateMelee = new DeadStateMelee(this, StateMachine, "Idle"); //랙돌 사용
        AbilityStateMelee = new AbilityStateMelee(this, StateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Init(IdleStateMelee);
        InitSpciality();
    }
    protected override void Update()
    {
        base.Update();

        StateMachine.CurState.Update();

        if(ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }

    }

    public override void EnterBattleMode()
    {
        if (InBattleMode) return;

        base.EnterBattleMode();
            
        StateMachine.ChangeState(RecoveryStateMelee);
    }


    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        MoveSpeed = MoveSpeed * 0.6f;
        PulledWeapon.gameObject.SetActive(false);
    }

    private void InitSpciality()
    {
        if(MeleeType == EnemyMelee_Type.Shield)
        {
            Animator.SetFloat("ChaseIndex", 1f);
            ShieldTransform.gameObject.SetActive(true);
        }
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

    public void ActiveRoll()
    {
        if (MeleeType != EnemyMelee_Type.Dodge)
            return;

        if (StateMachine.CurState != ChaseStateMelee)
            return;

        if (Vector3.Distance(transform.position, Player.position) < 3f)
            return;

        float DodgeAnimDuration = GetAnimationClipDuration("Roll");

        if (Time.time > RollCooldown + LastRollTime)
        {
            LastRollTime = Time.time;
            Animator.SetTrigger("Roll");

        }

    }

    public bool CanThrowAxe()
    {
        if (MeleeType != EnemyMelee_Type.Axe)
            return false;

        if (Time.time > LastTimeAxeThrown + AxeCooldown)
        {
            LastTimeAxeThrown = Time.time;
            return true;
        }
        return false;
    }

    private float GetAnimationClipDuration(string ClipName)
    {
        AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if(clip.name == ClipName)
                return clip.length;
        }


        return 0;
    }

    public void PulledWeaponActive() => PulledWeapon.gameObject.SetActive(true);
    

}
