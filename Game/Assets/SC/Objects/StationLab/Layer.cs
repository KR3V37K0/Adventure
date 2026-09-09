using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Layer : MonoBehaviour
{
    [SerializeField] public int depth;
    [SerializeField] private Vector2 temperaturePrefer; 
    [SerializeField] private List<LiquidSolubilityPair> solubilityData;

    private Image image;
    private bool isDissolving = false;
    private float progress = 0f;
    private float speedMultiplier = 1f;
    private ObjectInFlask parentObject;
    private StationLab station;

    public bool IsDissolved => progress >= 1f;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
            Debug.LogError("Image component not found on Layer object!");
    }

    public void Initialize(ObjectInFlask parent, StationLab lab)
    {
        parentObject = parent;
        station = lab;
    }

    public void StartDissolving()
    {
        if (IsDissolved) return;
        isDissolving = true;
        progress = 0f;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = Mathf.Clamp01(multiplier);
    }

    private float GetTemperatureMultiplier()
    {
        if (station == null) return 1f;

        float temp = station.currentTemperature;
        float min = temperaturePrefer.x;
        float max = temperaturePrefer.y;
        float center = (min + max) / 2f;
        float halfRange = (max - min) / 2f;

        // Если диапазон нулевой — проверяем точное совпадение
        if (halfRange == 0f)
            return Mathf.Approximately(temp, center) ? 1f : 0f;

        // Нормализованное расстояние от центра (0 в центре, 1 на границе)
        float distance = Mathf.Abs(temp - center) / halfRange;
        float multiplier = Mathf.Clamp01(1f - distance);

        // Округление до десятых вверх
        multiplier = Mathf.Ceil(multiplier * 10f) / 10f;

///Debug.Log(multiplier);

        return multiplier;
    }

    private void Update()
    {
        if (!isDissolving) return;

        float solubility = GetSolubility(station.CurrentLiquid);
        float liquidBaseSpeed = station.CurrentLiquid.baseDissolvePower;
        float baseSpeed = (solubility / 100f) * liquidBaseSpeed;
        float speed = baseSpeed * speedMultiplier * GetTemperatureMultiplier();

        progress += speed * Time.deltaTime;

        if (progress >= 1f)
        {
            progress = 1f;
            isDissolving = false;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            if (parentObject != null)
                parentObject.OnLayerDissolved(this);
        }
        else
        {
            float alpha = 1f - progress;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    }

    public float GetSolubility(LiquidType liquid)
    {
        foreach (var pair in solubilityData)
        {
            if (pair.liquid == liquid)
                return pair.solubilityPercent;
        }
        return 0f;
    }
}

[System.Serializable]
public class LiquidSolubilityPair
{
    public LiquidType liquid;
    [Range(0f, 100f)] public float solubilityPercent;
}