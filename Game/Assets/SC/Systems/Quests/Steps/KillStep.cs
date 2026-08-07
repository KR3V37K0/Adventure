using UnityEngine;

[CreateAssetMenu(fileName = "KillStep", menuName = "Quests/Steps/Kill")]
public class KillStep : QuestStep
{
    public string enemyTag;

    public override bool IsComplete(QuestContext context)
    {
        return context.killCounts.TryGetValue(enemyTag, out int count) && count >= requiredCount;
    }
    public override int GetProgress(QuestContext context)
    {
        return context.killCounts.TryGetValue(enemyTag, out int count) ? count : 0;
    }
}
