using UnityEngine;

public abstract class QuestStep : ScriptableObject
{
    public string stepDescription;
    public abstract bool IsComplete(QuestContext context);
}
