using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class StationLab : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject window;
    [Inject] private PlayerInput input;

    [Header("LAB SETTINGS")]
    public float currentTemperature {get; private set;}
    [SerializeField] private LiquidType currentLiquid;
    public LiquidType CurrentLiquid => currentLiquid;

    public System.Action<int> OnTemperatureChanged;

    void OnEnable()
    {
        OnTemperatureChanged+=SetTemperature;
    }
    void OnDisable()
    {
        OnTemperatureChanged-=SetTemperature;
    }
    private void Awake()
    {
        window.SetActive(false);
    }

    

    /// OUTER

    public void Interact()
    {
        window.SetActive(true);
    }

    


    // INNER
    
    public void SetLiquid(LiquidType newLiquid)
    {
        currentLiquid = newLiquid;
    }

    public void SetTemperature(int temp)
    {
        currentTemperature = temp;
    }
}