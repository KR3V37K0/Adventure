using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class ObjectInFlask : MonoBehaviour
{
    [SerializeField] private GameObject essencePrefab;
    [SerializeField] Material material;
    [Inject] private DiContainer container;
    public ItemData data {get;private set;}

    private Layer[] allLayers;
    private bool isComplete = false;

    private Dictionary<int, List<Layer>> layersByDepth = new Dictionary<int, List<Layer>>();
    private Dictionary<int, int> dissolvedCountByDepth = new Dictionary<int, int>();
    private List<int> sortedDepths = new List<int>(); // от большей к меньшей

    private int essenceCount = 1;
    private float essenceScatterRadius = 0.3f;


    public void BuildFromConfig(ItemData _data)
    {

        ObjectInFlaskConfig config = _data.flaskConfig;
        data = _data;

        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        foreach (var layerConfig in config.layers)
        {
            GameObject layerObj = new GameObject($"Layer_{layerConfig.depth}_{layerConfig.name}");
            layerObj.transform.SetParent(transform, false);

            SpriteRenderer sr = layerObj.AddComponent<SpriteRenderer>();
            sr.sortingOrder = layerConfig.depth + 1;

            sr.material=material;

            Layer layer = layerObj.AddComponent<Layer>();
            layer.Setup(layerConfig);
        }

        essenceCount = config.essenceCount;
        Init();
    }

    public void Init()
    {
        allLayers = GetComponentsInChildren<Layer>();

        foreach (var layer in allLayers)
            layer.Initialize(this);

        layersByDepth.Clear();
        dissolvedCountByDepth.Clear();

        foreach (var layer in allLayers)
        {
            if (!layersByDepth.ContainsKey(layer.depth))
            {
                layersByDepth[layer.depth] = new List<Layer>();
                dissolvedCountByDepth[layer.depth] = 0;
            }
            layersByDepth[layer.depth].Add(layer);
        }

        sortedDepths = layersByDepth.Keys.OrderByDescending(d => d).ToList();
    }

    public void PutOnLiquid()
    {
        StartAllDepths();
    }

    private void StartAllDepths()
    {
        for (int i = 0; i < sortedDepths.Count; i++)
        {
            int depth = sortedDepths[i];
            foreach (var layer in layersByDepth[depth])
            {
                if (layer.IsDissolved) continue;

                float initialMultiplier = (i == 0) ? 1f : 0f;
                layer.SetSpeedMultiplier(initialMultiplier);
                layer.StartDissolving();
            }
        }
    }

    public void OnLayerDissolved(Layer layer)
    {
        dissolvedCountByDepth[layer.depth]++;

        int layerIndex = sortedDepths.IndexOf(layer.depth);
        int totalAtDepth = layersByDepth[layer.depth].Count;
        int dissolvedAtDepth = dissolvedCountByDepth[layer.depth];

        if (dissolvedAtDepth >= totalAtDepth)
        {
            for (int i = layerIndex + 1; i < sortedDepths.Count; i++)
            {
                int depth = sortedDepths[i];
                int totalHigher = 0;
                int dissolvedHigher = 0;
                for (int j = 0; j < i; j++)
                {
                    int d = sortedDepths[j];
                    totalHigher += layersByDepth[d].Count;
                    dissolvedHigher += dissolvedCountByDepth[d];
                }

                float multiplier = totalHigher > 0 ? (float)dissolvedHigher / totalHigher : 1f;

                foreach (var l in layersByDepth[depth])
                {
                    if (!l.IsDissolved)
                        l.SetSpeedMultiplier(multiplier);
                }
            }
        }

        if (allLayers.All(l => l.IsDissolved))
            OnAllDissolved();
    }

    public List<Layer> GetLayersAtDepth(int depth)
    {
        return layersByDepth.TryGetValue(depth, out var list) ? list : new List<Layer>();
    }

    private void OnAllDissolved()
    {
        if (isComplete) return;
        isComplete = true;

        if (essencePrefab == null) return;

        for (int i = 0; i < essenceCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * essenceScatterRadius;
            Vector3 pos = transform.position + new Vector3(offset.x, offset.y, -6f);
            container.InstantiatePrefab(essencePrefab, pos, Quaternion.identity, transform.parent);

        }
        Destroy(this.gameObject);
    }
}