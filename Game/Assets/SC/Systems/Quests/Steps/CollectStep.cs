using UnityEngine;

[CreateAssetMenu(fileName = "CollectStep", menuName = "Quests/Steps/Collect")]
public class CollectStep : QuestStep
{
    public string itemID;
    public int requiredCount;

    public override bool IsComplete(QuestContext context)
    {
        Debug.Log(@$"собрано {context.inventory.GetItemCount(itemID)} из {requiredCount}. Выполнение = {context.inventory.GetItemCount(itemID) >= requiredCount}");
        return context.inventory != null && context.inventory.GetItemCount(itemID) >= requiredCount;
    }
}
