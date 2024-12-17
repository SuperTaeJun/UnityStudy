using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{

    public IdleStateMelee IdleStateMelee {  get; private set; }
    public MoveStateMelee MoveStateMelee { get; private set;}

    protected override void Awake()
    {
        base.Awake();
        IdleStateMelee = new IdleStateMelee(this, StateMachine, "Idle");
        MoveStateMelee = new MoveStateMelee(this, StateMachine, "Move");
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
}
