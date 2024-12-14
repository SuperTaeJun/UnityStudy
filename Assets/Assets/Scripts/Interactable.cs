using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected WeaponController WeaponController;
    protected MeshRenderer Mesh;

    [SerializeField] private Material HighlightMaterial;
    protected Material DefaultMaterial;


    private void Start()
    {
        if(Mesh == null)
            Mesh = GetComponentInChildren<MeshRenderer>();

        DefaultMaterial = Mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer mesh)
    {
        Mesh = mesh;
        DefaultMaterial = Mesh.sharedMaterial;
    }

    public virtual void Interact()
    {

    }


    public void HighlightActive(bool active)
    {
        if(active)
            Mesh.material = HighlightMaterial;
        else
            Mesh.material = DefaultMaterial;

    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (WeaponController == null)
        {
            WeaponController = other.GetComponent<WeaponController>();
        }
        PlayerInteract interact = other.GetComponent<PlayerInteract>();

        if (!interact) return;

        interact.GetInteractables().Add(this);
        interact.UpdateClosest();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteract interact = other.GetComponent<PlayerInteract>();

        if (!interact) return;
        interact.GetInteractables().Remove(this);
        interact.UpdateClosest();
    }
}
