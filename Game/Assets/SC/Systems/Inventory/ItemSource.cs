using UnityEngine;
using DG.Tweening;
using Zenject;

public class ItemSource : MonoBehaviour, IInteractable
{
    [Inject] private ItemPool itemPool;
    [SerializeField] private ItemData itemData;
    [SerializeField] private int minCount = 1;
    [SerializeField] private int maxCount = 3;
    [SerializeField] private float scatterRadius = 3f;
    [SerializeField] private float flyDuration = 0.3f;

    public void Interact()
    {
        GiveItems();
    }

    private void GiveItems()
    {
        int count = Random.Range(minCount, maxCount + 1);
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * scatterRadius;
            Vector3 targetPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            GameObject newItem = itemPool.Get(transform.position, Quaternion.identity);
            newItem.GetComponent<Item>().SetData(itemData);

            newItem.transform.localScale = Vector3.zero;

            newItem.transform.DOMove(targetPos, flyDuration).SetEase(Ease.InOutSine);
            newItem.transform.DOScale(Vector3.one, flyDuration).SetEase(Ease.OutBack, 1.5f);
        }
    }
}