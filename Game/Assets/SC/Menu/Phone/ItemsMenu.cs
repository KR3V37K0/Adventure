using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using Zenject;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField]Transform content;
    [SerializeField]GameObject item_panel;
    [Inject]Inventory inventory;
    [Inject]ItemsDatabase itemDB;
    private ItemPanelPool pool;

    void OnEnable()
    {
        Visualize();
    }
    void OnDisable()
    {
        Clear();
    }
    

    private void Awake()
    {
        pool = new ItemPanelPool(item_panel, content, 3);
    }

    private void Visualize()
    {
        foreach (var item in inventory.items)
        {
            GameObject obj = pool.Get();
            obj.GetComponent<Item_InMenu>().SetData(itemDB.GetItemById(item.Key), item.Value);
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

    public ItemPanelPool(GameObject prefab, Transform parent, int prewarmCount = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < prewarmCount; i++)
            pool.Enqueue(CreateNew());
    }

    private GameObject CreateNew()
    {
        GameObject obj = Object.Instantiate(prefab, parent);
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
