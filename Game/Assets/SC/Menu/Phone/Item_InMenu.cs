using TMPro;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class Item_InMenu : MonoBehaviour
{
    [SerializeField]Image img_icon;
    [SerializeField]TMP_Text txt_name, txt_count;
    public void SetData(ItemData item, int count)
    {
        img_icon.sprite=item.icon;
        txt_name.text=item.name;
        txt_count.text="x"+count;
    }
}
