using TMPro;
using UnityEngine;
using Zenject;

public class ResourceCountVisulizer : MonoBehaviour
{
    [SerializeField]ItemData resource;
    [SerializeField]TMP_Text txt_count;
    [SerializeField]GameObject panel;
    [Inject] SignalBus signalBus;
    [Inject] Inventory inventory;
    void OnEnable()
    {
        signalBus.Subscribe<ItemCollectedSignal>(visualize);
    }
    public void visualize(ItemCollectedSignal signal)
    {
        if(signal.ItemID!=resource.name)return;;
        if (inventory.GetItemCount(signal.ItemID) == 0)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
        txt_count.text=inventory.GetItemCount(signal.ItemID).ToString("D5");
    }
}
