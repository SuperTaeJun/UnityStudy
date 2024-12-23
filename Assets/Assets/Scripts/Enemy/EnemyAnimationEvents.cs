using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy Enemy;

    private void Awake()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger() => Enemy.AnimationTrigger();
}
