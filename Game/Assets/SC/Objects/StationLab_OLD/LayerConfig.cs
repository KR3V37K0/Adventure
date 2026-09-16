using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLayerConfig", menuName = "Lab/Layer Config")]
public class LayerConfig : ScriptableObject
{
    public Sprite sprite;
    public Sprite alpha;
    public int depth;
    public Vector2 temperaturePrefer;
    public List<LiquidSolubilityPair> solubilityData;
}