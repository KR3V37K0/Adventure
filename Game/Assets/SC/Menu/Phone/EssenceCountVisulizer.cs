using TMPro;
using UnityEngine;
using Zenject;

public class EssenceCountVisulizer : MonoBehaviour
{
    [SerializeField]TMP_Text txt_count;
    [Inject] SignalBus signalBus;
    [Inject] Inventory inventory;
    void OnEnable()
    {
        signalBus.Subscribe<ItemCollectedSignal>(visualize);
    }
    public void visualize(ItemCollectedSignal signal)
    {
        if(signal.ItemId!="essence")return;;
        txt_count.text=inventory.GetItemCount(signal.ItemId).ToString("D5");
    }
}
