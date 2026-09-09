using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class ObjectInFlask : MonoBehaviour
{
    [SerializeField] private GameObject essencePrefab;
    [Inject]DiContainer container;

    private Layer[] allLayers;
    private StationLab station;
    private bool isComplete = false;

    // Для управления скоростью слоёв глубиной 1
    private int totalLayersDepth2 = 0;
    private int dissolvedLayersDepth2 = 0;
    private List<Layer> layersDepth1 = new List<Layer>();

    private void Start()
    {
        allLayers = GetComponentsInChildren<Layer>();
        station = GetComponentInParent<StationLab>();
        if (station == null) Debug.LogError("StationLab not found in parent!");

        foreach (var layer in allLayers)
        {
            layer.Initialize(this, station);
        }

        // Подсчитываем слои по глубинам
        foreach (var layer in allLayers)
        {
            if (layer.depth == 2) totalLayersDepth2++;
            if (layer.depth == 1) layersDepth1.Add(layer);
        }

        PutOnLiquid();
    }

    public void PutOnLiquid()
    {
        StartAllDepths();
    }

    private void StartAllDepths()
    {
        // Запускаем растворение ВСЕХ слоёв
        foreach (var layer in allLayers)
        {
            if (!layer.IsDissolved)
            {
                // Для слоёв глубиной 1 устанавливаем начальный множитель 0
                if (layer.depth == 1)
                {
                    float initialMultiplier = (totalLayersDepth2 > 0) ? 0f : 1f;
                    layer.SetSpeedMultiplier(initialMultiplier);
                }
                // Для глубины 2 множитель = 1 (полная скорость)
                else if (layer.depth == 2)
                {
                    layer.SetSpeedMultiplier(1f);
                }
                // Для других глубин (если есть) — можно тоже 1
                else
                {
                    layer.SetSpeedMultiplier(1f);
                }

                layer.StartDissolving();
            }
        }
    }

    public void OnLayerDissolved(Layer layer)
    {


        // Если это слой глубиной 2 — обновляем множитель для слоёв глубиной 1
        if (layer.depth == 2)
        {
            dissolvedLayersDepth2++;
            float newMultiplier = (float)dissolvedLayersDepth2 / totalLayersDepth2;
            foreach (var l1 in layersDepth1)
            {
                if (!l1.IsDissolved)
                {
                    l1.SetSpeedMultiplier(newMultiplier);
                }
            }
        }

        // Проверяем, все ли слои растворены
        bool allDissolved = true;
        foreach (var l in allLayers)
        {
            if (!l.IsDissolved)
            {
                allDissolved = false;
                break;
            }
        }

        if (allDissolved)
        {
            OnAllDissolved();
        }
    }

    public List<Layer> GetLayersAtDepth(int depth)
    {
        List<Layer> result = new List<Layer>();
        foreach (var layer in allLayers)
        {
            if (layer.depth == depth)
                result.Add(layer);
        }
        return result;
    }

    private void OnAllDissolved()
    {
        if (isComplete) return;
        isComplete = true;

        if (essencePrefab != null)
        {
            container.InstantiatePrefab(essencePrefab, transform.position, Quaternion.identity, transform);
        }
    }
}