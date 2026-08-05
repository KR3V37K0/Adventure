using UnityEngine;
using Zenject;

public class Item : MonoBehaviour, IInteractable
{
    [Inject]Inventory inventory;
    public string itemID;
    public void Interact()
    {
        inventory.AddItem(itemID);
        Destroy(gameObject);
    }
}