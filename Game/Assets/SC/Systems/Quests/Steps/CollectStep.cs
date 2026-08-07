using UnityEngine;

[CreateAssetMenu(fileName = "CollectStep", menuName = "Quests/Steps/Collect")]
public class CollectStep : QuestStep
{
    public string itemID;
    public override bool IsComplete(QuestContext context)
    {
        //Debug.Log(@$"собрано {context.inventory.GetItemCount(itemID)} из {requiredCount}. Выполнение = {context.inventory.GetItemCount(itemID) >= requiredCount}");
        return context.inventory != null && context.inventory.GetItemCount(itemID) >= requiredCount;
    }
    public override int GetProgress(QuestContext context)
    {
        return context.inventory?.GetItemCount(itemID) ?? 0;
    }
}
