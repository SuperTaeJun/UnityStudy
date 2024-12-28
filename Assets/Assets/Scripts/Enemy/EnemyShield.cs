using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private EnemyMelee Enemy;
    [SerializeField] private int Durability;

    private void Awake()
    {
        Enemy = GetComponentInParent<EnemyMelee>();
    }

    public void ReduceDurability()
    {
        Durability--;
        if (Durability <= 0)
        {
            Enemy.Animator.SetFloat("ChaseIndex", 0f);
            Destroy(gameObject); 
        }
    }
}
