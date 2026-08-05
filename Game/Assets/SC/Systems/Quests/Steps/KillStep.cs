using UnityEngine;

[CreateAssetMenu(fileName = "KillStep", menuName = "Quests/Steps/Kill")]
public class KillStep : QuestStep
{
    public string enemyTag;
    public int requiredCount;

    public override bool IsComplete(QuestContext context)
    {
        return context.killCounts.TryGetValue(enemyTag, out int count) && count >= requiredCount;
    }
}
