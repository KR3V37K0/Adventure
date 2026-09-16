using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item_InLab : Item_InMenu
{
    [SerializeField]TMP_Text txt_current_count;
    [SerializeField]Slider slider;
    [SerializeField]GameObject counting_group;
    [SerializeField]GrabableLabImage grabableLabImage;
    public int current_count {get;private set;}
    public override void SetData(ItemData item, int count)
    {
        current_count=1;
        base.SetData(item,count);
        this.Visualize();
    }
    public override void Visualize()
    {
        base.Visualize();

        grabableLabImage.item_InLab=this;

        if (count == 1)
        {
            txt_current_count.text="x"+count;
            counting_group.SetActive(false);
        }
        else
        {
            counting_group.SetActive(true);
            slider.maxValue=count;
            slider.value=1;
            txt_current_count.text="x1";
        }
    }
    public void OnSliderValueChanged()
    {
        txt_current_count.text="x"+slider.value;
        current_count=(int)slider.value;

    }
}
