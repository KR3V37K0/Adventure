using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject item_panel;
    [SerializeField] private GameObject txt_noItems;

    [Inject] private Inventory inventory;
    [Inject] private ItemsDatabase itemDB;
    [Inject] private DiContainer container;
    [Inject] private SignalBus bus;

    private ItemPanelPool pool;

    private void Awake()
    {
        pool = new ItemPanelPool(container, item_panel, content, 3);
    }

    private void OnEnable()
    {
        bus.Subscribe<ItemCollectedSignal>(OnItemCollected);
        Visualize();
    }

    private void OnDisable()
    {
        bus.Unsubscribe<ItemCollectedSignal>(OnItemCollected);
        Clear();
    }

    private void OnItemCollected(ItemCollectedSignal signal)
    {
        if(itemDB.GetItemById(signal.ItemID).inInventory==false)return;

        foreach (var p in content.GetComponentsInChildren<Item_InMenu>())
        {
            if (p.data != null && p.data.name == signal.ItemID)
            {
                txt_noItems.SetActive(false);
                return;
            }
        }

        GameObject obj = pool.Get();
        var panel = obj.GetComponent<Item_InMenu>();
        panel.SetPool(pool);
        panel.SetData(itemDB.GetItemById(signal.ItemID), inventory.GetItemCount(signal.ItemID));

        txt_noItems.SetActive(false);
    }

    private void Visualize()
    { 

        if (inventory.items == null || inventory.items.Count == 0)
        {
            txt_noItems.SetActive(true);
            return;
        }

        txt_noItems.SetActive(false);

        foreach (var item in inventory.items)
        {
            if (itemDB.GetItemById(item.Key).inInventory == true)
            {
                GameObject obj = pool.Get();
                var panel = obj.GetComponent<Item_InMenu>();
                panel.SetPool(pool);
                panel.SetData(itemDB.GetItemById(item.Key), item.Value);
            }
        }
    }

    private void Clear()
    {
        foreach (Transform child in content)
        {
            pool.Return(child.gameObject);
        }
    }
}
public class ItemPanelPool
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;
    private Transform parent;
    DiContainer container;

    public ItemPanelPool(DiContainer container,GameObject prefab, Transform parent, int prewarmCount = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.container=container;
        for (int i = 0; i < prewarmCount; i++)
            pool.Enqueue(CreateNew());
    }

    private GameObject CreateNew()
    {
        GameObject obj = container.InstantiatePrefab(prefab, parent);
        obj.SetActive(false);
        return obj;
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return CreateNew();
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
