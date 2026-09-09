using UnityEngine;
using Zenject;
using System.Collections;
using UnityEngine.UI;

public class Item : MonoBehaviour, IGrabable
{
    [Inject] private Inventory inventory;
    [Inject] private ItemPool itemPool;
    //public string itemID;
    ItemData itemData;
    [SerializeField] SpriteRenderer sprite;
    float moveSpeed = 10f;          
    float maxDistance = 5f;       
    float arrivalDistance = 0.2f;  

    private Coroutine grabCoroutine;
    public void SetData(ItemData _itemdata)
    {
        itemData=_itemdata;
        sprite.sprite=itemData.icon;
    }

    public void GrabTo(Transform target)
    {
        if (grabCoroutine != null)
            return;
        grabCoroutine = StartCoroutine(MoveToTarget(target));
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        while (true)
        {
            if (target == null)
                break;

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= arrivalDistance)
            {
                AddToInventory();
                grabCoroutine = null;
                break;
            }
            if (distance > maxDistance)
            {
                StopCoroutine(grabCoroutine);
                grabCoroutine=null;
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        grabCoroutine = null;
    }

    private void AddToInventory()
    {
        inventory.AddItem(itemData.name);
        itemPool.Return(gameObject);
    }
}