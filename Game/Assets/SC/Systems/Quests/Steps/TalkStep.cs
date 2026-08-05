using UnityEngine;

[CreateAssetMenu(fileName = "TalkStep", menuName = "Quests/Steps/Talk")]
public class TalkStep : QuestStep
{
    public string npcID;

    public override bool IsComplete(QuestContext context)
    {
        return context.talkedNPCs.Contains(npcID);
    }
}
