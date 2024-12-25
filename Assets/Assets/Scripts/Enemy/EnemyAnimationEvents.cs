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


    public void StartManualMovement() => Enemy.AcitveManualMovement(true);
    public void StopManualMovement() => Enemy.AcitveManualMovement(false);

    public void StartManualRotation() => Enemy.AcitveManualRotation(true);
    public void StopManualRotation() => Enemy.AcitveManualRotation(false);
}
