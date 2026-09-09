using UnityEngine;

[CreateAssetMenu(fileName = "NewLiquid", menuName = "Lab/Liquid")]
public class LiquidType : ScriptableObject
{
    public string liquidName;
    public Color color;
    public float baseDissolvePower; // базовый множитель скорости
    public Sprite icon;
}