using System.Collections.Generic;

public class QuestContext
{
    public Dictionary<string, int> killCounts = new Dictionary<string, int>();
    public Inventory inventory;
    public List<string> talkedNPCs = new List<string>();
    public bool miniGameCompleted;
}