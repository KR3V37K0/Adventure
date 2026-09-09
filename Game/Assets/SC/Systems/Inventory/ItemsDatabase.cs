using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    private Dictionary<string, ItemData> itemsById = new Dictionary<string, ItemData>();

    private void Awake()
    {
        LoadAllItems();
    }

    private void LoadAllItems()
    {
        ItemData[] loadedItems = Resources.LoadAll<ItemData>("Items");
        foreach (var item in loadedItems)
        {
            if (string.IsNullOrEmpty(item.name))
            {
                Debug.LogWarning($"Item {item.name} has no ID, skipping.");
                continue;
            }
            if (itemsById.ContainsKey(item.name))
                Debug.LogWarning($"Duplicate item ID: {item.name}");
            else
                itemsById[item.name] = item;
        }
    }

    public ItemData GetItemById(string id)
    {
        itemsById.TryGetValue(id, out ItemData item);
        return item;
    }

    public bool HasItem(string id) => itemsById.ContainsKey(id);
}
