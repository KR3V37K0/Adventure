using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    IInteractable objInteract;
    [SerializeField] bool canInteract;
    void OnTriggerEnter2D(Collider2D coll)
    {
        canInteract=coll.gameObject.TryGetComponent(out objInteract);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(canInteract) 
        {
            objInteract=null;
            canInteract=false;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(objInteract!=null)
        {
            objInteract.Interact();
        }
    }
    /*
        void OnCollisionExit2D(Collision2D coll)
        {
            if(canInteract) objInteract=null;
        }*/
}
