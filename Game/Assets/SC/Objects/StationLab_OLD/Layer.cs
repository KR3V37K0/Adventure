using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Layer : MonoBehaviour
{
    [SerializeField] public int depth;
    [SerializeField] private Vector2 temperaturePrefer;
    [SerializeField] private List<LiquidSolubilityPair> solubilityData;

    private SpriteRenderer spriteRenderer;
    private Material mat;
    private bool isDissolving = false;
    private float progress = 0f;
    private float speedMultiplier = 1f;
    private ObjectInFlask parentObject;

    public bool IsDissolved => progress >= 1f;

    public void Setup(LayerConfig config)
    {
        depth = config.depth;
        temperaturePrefer = config.temperaturePrefer;
        solubilityData = config.solubilityData;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = config.sprite;
            mat = spriteRenderer.material;

            if (config.sprite != null)
                mat.SetTexture("_texture", config.sprite.texture);
            if (config.alpha != null)
                mat.SetTexture("_alpha_texture", config.alpha.texture);

            mat.SetFloat("_alpha", 1f);
        }
    }

    public void Initialize(ObjectInFlask parent)
    {
        parentObject = parent;
    }

    public void StartDissolving()
    {
        if (IsDissolved) return;
        if (isDissolving) return;
        isDissolving = true;
        progress = 0f;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = Mathf.Clamp01(multiplier);
    }

    private float GetTemperatureMultiplier()
    {
        float temp = Lab.Instance.currentTemperature;
        float min = temperaturePrefer.x;
        float max = temperaturePrefer.y;
        float center = (min + max) / 2f;
        float halfRange = (max - min) / 2f;

        if (halfRange == 0f)
            return Mathf.Approximately(temp, center) ? 1f : 0f;

        float distance = Mathf.Abs(temp - center) / halfRange;
        float multiplier = Mathf.Clamp01(1f - distance);
        multiplier = Mathf.Ceil(multiplier * 10f) / 10f;

        return multiplier;
    }

    private void Update()
    {
        if (!isDissolving) return;

        float solubility = GetSolubility(Lab.Instance.CurrentLiquid);
        float liquidBaseSpeed = Lab.Instance.CurrentLiquid.baseDissolvePower;
        float baseSpeed = (solubility / 100f) * liquidBaseSpeed;
        float speed = baseSpeed * speedMultiplier * GetTemperatureMultiplier();

        progress += speed * Time.deltaTime;

        if (progress >= 1f)
        {
            progress = 1f;
            isDissolving = false;
            if (mat != null) mat.SetFloat("_alpha", 0f);
            if (parentObject != null)
                parentObject.OnLayerDissolved(this);
        }
        else
        {
            if (mat != null) mat.SetFloat("_alpha", 1f-progress);
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