using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    [field: SerializeField] public string questID { get; private set; }
    public string questName;
    [TextArea] public string description;
    public Sprite icon;
    public List<QuestStep> steps = new List<QuestStep>();
}