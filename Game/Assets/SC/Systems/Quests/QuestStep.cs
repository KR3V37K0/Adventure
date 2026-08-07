using UnityEngine;

public abstract class QuestStep : ScriptableObject
{
    public string stepDescription;
    public int requiredCount;
    public abstract bool IsComplete(QuestContext context);
    public abstract int GetProgress(QuestContext context); 
}
