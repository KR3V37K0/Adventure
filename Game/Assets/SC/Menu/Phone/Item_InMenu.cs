using TMPro;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Item_InMenu : MonoBehaviour
{
    public ItemData data;
    public int count;

    [Inject] private SignalBus bus;
    [SerializeField] private Image img_icon;
    [SerializeField] private TMP_Text txt_name, txt_count;

    private ItemPanelPool pool;

    public void SetPool(ItemPanelPool _pool) => pool = _pool;

    private void OnEnable()
    {
        bus.Subscribe<ItemCollectedSignal>(AddCount);
        bus.Subscribe<ItemRemovedSignal>(RemoveCount);
    }

    private void OnDisable()
    {
        bus.Unsubscribe<ItemCollectedSignal>(AddCount);
        bus.Unsubscribe<ItemRemovedSignal>(RemoveCount);
    }

    public virtual void SetData(ItemData item, int _count)
    {
        data = item;
        count = _count;
        Visualize();
    }

    public virtual void Visualize()
    {
        if (data == null) return;
        img_icon.sprite = data.icon;
        txt_name.text = data.itemName;
        txt_count.text = "x" + count;
    }

    public virtual void AddCount(ItemCollectedSignal signal)
    {
        if (data == null || signal.ItemID != data.name) return;
        count += signal.Count;
        Visualize();
    }

    public virtual void RemoveCount(ItemRemovedSignal signal)
    {
        if (data == null || signal.ItemID != data.name) return;
        count -= signal.Count;

        if (count <= 0)
        {
            ReturnToPool();
            return;
        }

        Visualize();
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }
}
