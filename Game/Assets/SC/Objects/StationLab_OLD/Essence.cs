using UnityEngine;
using DG.Tweening;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class Essence : MonoBehaviour
{
    [SerializeField] private float hoverScale = 1.2f;
    [Inject] private Inventory inventory;

    private bool isCollected = false;

    public void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    private void OnMouseEnter()
    {
        if (isCollected) return;
        transform.DOScale(hoverScale, 0.2f);
        Collect();
    }


    private void Collect()
    {
        isCollected = true;
        inventory.AddItem("Essence", 1);

        transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}