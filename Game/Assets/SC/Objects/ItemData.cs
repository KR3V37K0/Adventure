using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool inInventory=true;
    [TextArea] public string description;
    public ObjectInFlaskConfig flaskConfig;
}