using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerInteract : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();

    private Interactable ClosestInteractable;
    

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.Controls.Character.Interact.performed += context => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        ClosestInteractable?.Interact();
        interactables.Remove(ClosestInteractable);
        UpdateClosest();
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


    public List<Interactable> GetInteractables() => interactables;

}
