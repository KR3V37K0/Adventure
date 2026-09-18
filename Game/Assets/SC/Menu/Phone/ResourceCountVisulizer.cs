
using System.Threading.Tasks;
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
        signalBus.Subscribe<SaveLoadedSignal>(OnLoaded);
        
    }
    void OnDisable()
    {
        signalBus.Unsubscribe<ItemCollectedSignal>(visualize);
        signalBus.Unsubscribe<SaveLoadedSignal>(OnLoaded);
    }

    public async void OnLoaded(SaveLoadedSignal signal)
    {
        await Task.Delay(1000);
        visualize(new ItemCollectedSignal(resource.name,0));
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
