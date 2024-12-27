using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField] private Transform RagdollParent;

    private Collider[] RagdollColliders;
    private Rigidbody[] RagdollRigidbodies;

    private void Awake()
    {
        RagdollColliders = GetComponentsInChildren<Collider>();
        RagdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        RagdollActive(false);
    }
    public void RagdollActive(bool active)
    {
        foreach(Rigidbody rb in RagdollRigidbodies)
        {
            rb.isKinematic = !active;
        }
    }

    public void CollidersActive(bool active)
    {
        foreach(Collider cd in RagdollColliders)
        {
            cd.enabled = active;
        }
    }
}
