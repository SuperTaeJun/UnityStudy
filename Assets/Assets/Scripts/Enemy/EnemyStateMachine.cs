using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState CurState {  get; private set; }

    public void Init(EnemyState NewState)
    {
        CurState = NewState;
        CurState.Enter();
    }
    public void ChangeState(EnemyState NewState)
    {
        CurState.Exit();
        CurState = NewState;
        CurState.Enter();
    }
}
