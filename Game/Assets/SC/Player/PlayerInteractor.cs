using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Collections.Generic;

public class PlayerInteractor : MonoBehaviour
{
    [Inject] private PlayerInput input;
    private List<IInteractable> interactablesInRange = new List<IInteractable>();

    void Start()
    {
        input.actions["Interact"].canceled += ctx => OnInteract(ctx);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            interactablesInRange.Remove(interactable);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (interactablesInRange.Count > 0)
        {
            GetClosestInteractable().Interact();
        }
    }
    private IInteractable GetClosestInteractable()
    {
        if (interactablesInRange.Count == 0) return null;
        IInteractable closest = interactablesInRange[0];
        float minDist = Vector3.Distance((closest as MonoBehaviour).transform.position, transform.position);
        foreach (var item in interactablesInRange)
        {
            float dist = Vector3.Distance((item as MonoBehaviour).transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = item;
            }
        }
        return closest;
    }
}
