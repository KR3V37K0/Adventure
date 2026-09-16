using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour, ISaveable
{
    [Inject] SignalBus signalBus;
    public Dictionary<string, int> items{get;private set;} = new Dictionary<string, int>();


    public void AddItem(string itemID, int count = 1)
    {
        if (items.ContainsKey(itemID))
            items[itemID] += count;
        else
            items[itemID] = count;

        signalBus.Fire(new ItemCollectedSignal(itemID,count));
    }
    public bool RemoveItem(string itemID, int count = 1)
    {
        if (!items.TryGetValue(itemID, out int current) || current < count)
        {
            Debug.LogWarning($"Недостаточно {itemID} для удаления ({count} из {current})");
            return false;
        }

        current -= count;

        if (current <= 0)
            items.Remove(itemID);
        else
            items[itemID] = current;

        signalBus.Fire(new ItemRemovedSignal(itemID, count));
        return true;
    }

    public int GetItemCount(string itemID)
    {
        return items.TryGetValue(itemID, out int count) ? count : 0;
    }

    public bool HasItem(string itemID, int count = 1)
    {
        return GetItemCount(itemID) >= count;
    }
    public object SaveState()
    {
        return items;
    }

    public void LoadState(object state)
    {
        items = (Dictionary<string, int>)state;
    }
    
}