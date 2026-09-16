using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInFlaskConfig", menuName = "Lab/Object in Flask")]
public class ObjectInFlaskConfig : ScriptableObject
{
    public List<LayerConfig> layers = new List<LayerConfig>();
    public int essenceCount = 1;
}