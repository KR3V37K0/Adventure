using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Zenject;

public class Essence : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.2f;
    [Inject]Inventory inventory;
    private bool isCollected = false;

    public void Initialize(StationLab lab)
    {
        // Можно добавить анимацию появления
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCollected) return;
        transform.DOScale(hoverScale, 0.2f);
        Collect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isCollected) return;
        transform.DOScale(1f, 0.2f);
    }

    private void Collect()
    {
        isCollected = true;
        // Добавить эссенцию в инвентарь
        // Inventory.Instance.AddItem("essence", 1)
        inventory.AddItem("essence", 1);

        transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}