using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class GrabableLabImage : MonoBehaviour, IPointerDownHandler
{
    [Inject]DiContainer container;
    [Inject]Inventory inventory;
    public Item_InLab item_InLab;
    [SerializeField]GameObject dragGhostPrefab;


 
    public void OnPointerDown(PointerEventData eventData)
    {
        int count=item_InLab.current_count;
        for(int i=0; i < count; i++)
        {
            GameObject ghost = container.InstantiatePrefab(dragGhostPrefab);

            ghost.GetComponent<ObjectInFlask>().BuildFromConfig(item_InLab.data);
            inventory.RemoveItem(item_InLab.data.name, 1);
        }
    }
}
