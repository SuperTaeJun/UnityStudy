using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float Health = 20.0f;


    [Header("Idle Info")]
    public float IdleTime;
    public float AggresionRange;

    [Header("Move Info")]
    public float TrunSpeed;
    public float MoveSpeed;
    public float ChaseSpeed;
    private bool ManualRotation;
    private bool ManualMovement;


    [SerializeField] private Transform[] PatrolPoints;
    private Vector3[] PatrolPointsPos;

    private int CurPatrolIndex;

    public bool InBattleMode {  get; private set; }

    public Transform Player {  get; private set; }
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent {  get; private set; }
    public EnemyStateMachine StateMachine {get; private set;}

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        InitPatrolPoint();
        InitPatrolPos();
    }

    private void InitPatrolPoint()
    {
        foreach (Transform t in PatrolPoints)
        {
            t.parent = null;
        }
    }

    protected virtual void Update()
    {

    }

    protected bool ShouldEnterBattleMode()
    {
        bool InAggresionRange = Vector3.Distance(transform.position, Player.transform.position) < AggresionRange;

        if (InAggresionRange && !InBattleMode)
        {
            EnterBattleMode();
            return true;
        }
        return false;
    }

    public virtual void EnterBattleMode()
    {
        InBattleMode = true;
    }

    public virtual void GetHit()
    {
        EnterBattleMode();
        Health--;
    }
    public virtual void DeadImpact(Vector3 Power, Vector3 HitPoint, Rigidbody rb)
    {
        StartCoroutine(DeadImpactCourutine(Power, HitPoint, rb));
    }

    private IEnumerator DeadImpactCourutine(Vector3 Power,Vector3 HitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);

        rb.AddForceAtPosition(Power, HitPoint,ForceMode.Impulse);
    }

    public Quaternion ForwardTarget(Vector3 Target)
    {
        Quaternion TargetRot = Quaternion.LookRotation(Target - transform.position);

        Vector3 CurEulerAngles = transform.rotation.eulerAngles;

        float yRot = Mathf.LerpAngle(CurEulerAngles.y, TargetRot.eulerAngles.y, TrunSpeed * Time.deltaTime);

        return Quaternion.Euler(CurEulerAngles.x, yRot, CurEulerAngles.z);
    }

    #region AnimationEvent
    public void AcitveManualMovement(bool ManualRotation) => this.ManualMovement = ManualRotation;
    public bool ManualMovementActive() => ManualMovement;

    public void AcitveManualRotation(bool ManualRotation) => this.ManualRotation = ManualRotation;
    public bool ManualRotationActive() => ManualRotation;



    public void AnimationTrigger() => StateMachine.CurState.AnimationTrigger();
    public virtual void AbilityTrigger() => StateMachine.CurState.AbilityTrigger();

    #endregion

    #region Patrol
    public Vector3 GetPatrolPoint()
    {
        Vector3 Destination = PatrolPointsPos[CurPatrolIndex];
        CurPatrolIndex++;

        if(CurPatrolIndex >= PatrolPoints.Length)
        {
            CurPatrolIndex = 0;
        }

        return Destination;
    }
    private void InitPatrolPos()
    {
        PatrolPointsPos = new Vector3[PatrolPoints.Length];

        for(int i = 0; i < PatrolPoints.Length; i++)
        {
            PatrolPointsPos[i] = PatrolPoints[i].position;
            PatrolPoints[i].gameObject.SetActive(false);
        }
    }
    #endregion
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AggresionRange);
    }
}
