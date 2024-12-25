using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Idle Info")]
    public float IdleTime;
    public float AggresionRange;

    [Header("Move Info")]
    public float TrunSpeed;
    public float MoveSpeed;
    public float ChaseSpeed;
    private bool ManualRotation;
    private bool ManualMovement;


    [SerializeField] private Transform[] PatrolPoint;
    private int CurPatrolIndex;


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
    }

    private void InitPatrolPoint()
    {
        foreach (Transform t in PatrolPoint)
        {
            t.parent = null;
        }
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AggresionRange);
    }

    public void AcitveManualMovement(bool ManualRotation) => this.ManualMovement = ManualRotation;
    public bool ManualMovementActive() => ManualMovement;

    public void AcitveManualRotation(bool ManualRotation) => this.ManualRotation = ManualRotation;
    public bool ManualRotationActive() => ManualRotation;



    public void AnimationTrigger() => StateMachine.CurState.AnimationTrigger();

    public bool PlayerInAggresionRange() => Vector3.Distance(transform.position, Player.transform.position) < AggresionRange;



    public Vector3 GetPatrolPoint()
    {
        Vector3 Destination = PatrolPoint[CurPatrolIndex].position;
        CurPatrolIndex++;

        if(CurPatrolIndex >= PatrolPoint.Length)
        {
            CurPatrolIndex = 0;
        }

        return Destination;
    }
    public Quaternion ForwardTarget(Vector3 Target)
    {
        Quaternion TargetRot = Quaternion.LookRotation(Target - transform.position);

        Vector3 CurEulerAngles = transform.rotation.eulerAngles;

        float yRot = Mathf.LerpAngle(CurEulerAngles.y, TargetRot.eulerAngles.y, TrunSpeed * Time.deltaTime);

        return Quaternion.Euler(CurEulerAngles.x, yRot, CurEulerAngles.z);
    }

}
