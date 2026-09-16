using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Lab : MonoBehaviour
{
    [Inject]SceneLoaderSC loader;

    [Header("LAB SETTINGS")]
    public static Lab Instance;
    public float currentTemperature {get; private set;}
    [SerializeField] private LiquidType currentLiquid;
    public LiquidType CurrentLiquid => currentLiquid;

    public System.Action<int> OnTemperatureChanged;

    void Awake()
    {
        Instance=this;
    }
    void OnEnable()
    {
        OnTemperatureChanged+=SetTemperature;
    }
    void OnDisable()
    {
        OnTemperatureChanged-=SetTemperature;
    }
    public void btn_Close()
    {
        //SceneManager.LoadScene("VILLAGE");
        loader.LoadSceneFast("VILLAGE");
    }
    
    public void SetLiquid(LiquidType newLiquid)
    {
        currentLiquid = newLiquid;
    }

    public void SetTemperature(int temp)
    {
        currentTemperature = temp;
    }

}
