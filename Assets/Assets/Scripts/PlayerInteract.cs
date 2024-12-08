using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerInteract : MonoBehaviour
{
    public List<Interactable> interactables;

    private Interactable ClosestInteractable;


    public void Start()
    {
        Player player = GetComponent<Player>();
        player.Controls.Character.Interact.performed += context => InteractWithClosest();
    }

    public void InteractWithClosest()
    {
        ClosestInteractable?.Interact();
    }

    public void UpdateClosest()
    {
        ClosestInteractable?.HighlightActive(false);


        ClosestInteractable = null;

        float ClosestDistance =float.MaxValue;

        foreach (var interactable in interactables)
        {
            float Distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (Distance < ClosestDistance)
            {
                ClosestDistance = Distance;
                ClosestInteractable= interactable;
            }
        }

        ClosestInteractable?.HighlightActive(true);
    }

}
