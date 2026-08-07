using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Yarn.Unity;

public class QuestSystem : MonoBehaviour, IInitializable
{
    [Inject] private SignalBus signalBus;
    
    [Inject] DialogueRunner dialogueRunner;

    public List<QuestInstance> activeQuests { get; private set; } = new List<QuestInstance>();
    private HashSet<string> completedQuests = new HashSet<string>();
    private Dictionary<string, Quest> questsById = new Dictionary<string, Quest>();
    public QuestContext context { get; private set; }
    [Inject]Inventory playerInventory;

   private void Awake()
    {
        signalBus.Subscribe<ItemCollectedSignal>(OnItemCollected);

            context = new QuestContext
            {
                inventory = playerInventory,
                killCounts = new Dictionary<string, int>(),
                talkedNPCs = new List<string>()
            };

        Quest[] loadedQuests = Resources.LoadAll<Quest>("Quests");
        foreach (var quest in loadedQuests)
        {
            if (string.IsNullOrEmpty(quest.questID))
            {
                Debug.LogWarning($"Квест {quest.name} не имеет ID, пропускаем.");
                continue;
            }
            if (questsById.ContainsKey(quest.questID))
                Debug.LogWarning($"Дубликат ID {quest.questID} у квеста {quest.name}");
            else
                questsById[quest.questID] = quest;
        }

        dialogueRunner.AddCommandHandler<string>(
            "start_quest",    
            StartQuestByID 
        );

        dialogueRunner.AddFunction<string, bool>("is_quest_active", questID => IsQuestActive(questID));
        dialogueRunner.AddFunction<string, bool>("is_quest_completed", questID =>  IsQuestCompleted(questID));

    }

    public void Initialize()
    {
        signalBus.Subscribe<EnemyKilledSignal>(OnEnemyKilled);
        signalBus.Subscribe<ItemCollectedSignal>(OnItemCollected);
        signalBus.Subscribe<TalkedToNPCSignal>(OnTalkedToNPC);
        signalBus.Subscribe<MiniGameCompletedSignal>(OnMiniGameCompleted);
    }
    public bool IsQuestCompleted(string questID)
    {
        return completedQuests.Contains(questID);
    }
    public bool IsQuestActive(string questID)
    {
        return activeQuests.Exists(q => q.Quest.questID == questID);
    }
    public void StartQuestByID(string questID)
    {
        if (!questsById.TryGetValue(questID, out Quest quest))
        {
            Debug.LogError($"Квест с ID '{questID}' не найден!");
            return;
        }
        StartQuest(quest);
    }
    public void StartQuest(Quest quest)
    {
        if (IsQuestCompleted(quest.questID)) return;
        if (activeQuests.Exists(q => q.Quest.questID == quest.questID)) return;

        var instance = new QuestInstance(quest);
        activeQuests.Add(instance);
        signalBus.Fire(new QuestStartedSignal(quest));
        UpdateAllQuests(); 
    }

    private void UpdateAllQuests()
    {
        bool anyChanged;
        do
        {
            anyChanged = false;
            foreach (var instance in activeQuests)
            {
                if (instance.IsCompleted) continue;
                if (instance.IsCurrentStepComplete(context))
                {
                    instance.AdvanceStep();
                    anyChanged = true;
                    break;
                }
            }
        } while (anyChanged);

        // Завершённые квесты
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            var instance = activeQuests[i];
            if (instance.IsComplete(context))
            {
                CompleteQuest(instance);
            }
        }
        signalBus.Fire(new QuestUpdatedSignal());
    }

    private void CompleteQuest(QuestInstance instance)
    {
        activeQuests.Remove(instance);
        completedQuests.Add(instance.Quest.questID);
        signalBus.Fire(new QuestCompletedSignal(instance.Quest.questID));
    }

    private void OnEnemyKilled(EnemyKilledSignal signal)
    {
        if (!context.killCounts.ContainsKey(signal.enemyId))
            context.killCounts[signal.enemyId] = 0;
        context.killCounts[signal.enemyId]++;
        UpdateAllQuests();
    }

    private void OnItemCollected(ItemCollectedSignal signal)
    {
        UpdateAllQuests();
    }

    private void OnTalkedToNPC(TalkedToNPCSignal signal)
    {
        if (!context.talkedNPCs.Contains(signal.npcId))
            context.talkedNPCs.Add(signal.npcId);
        UpdateAllQuests();
    }

    private void OnMiniGameCompleted(MiniGameCompletedSignal signal)
    {
        context.miniGameCompleted = true;
        UpdateAllQuests();
    }
}