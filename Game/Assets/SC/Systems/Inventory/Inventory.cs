using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemID, int count = 1)
    {
        if (items.ContainsKey(itemID))
            items[itemID] += count;
        else
            items[itemID] = count;

        signalBus.Fire(new ItemCollectedSignal(itemID));
    }

    public int GetItemCount(string itemID)
    {
        return items.TryGetValue(itemID, out int count) ? count : 0;
    }

    public bool HasItem(string itemID, int count = 1)
    {
        return GetItemCount(itemID) >= count;
    }
}