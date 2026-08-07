using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class QuestInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        //SignalBusInstaller.Install(Container);
        Container.Bind<QuestSystem>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.Bind<Inventory>().FromNewComponentOnNewGameObject().AsSingle();           //точно ли этот способ?

        Container.DeclareSignal<QuestStartedSignal>();
        Container.DeclareSignal<QuestCompletedSignal>();
        Container.DeclareSignal<EnemyKilledSignal>();
        Container.DeclareSignal<ItemCollectedSignal>();
        Container.DeclareSignal<TalkedToNPCSignal>();
        Container.DeclareSignal<MiniGameCompletedSignal>();
        Container.DeclareSignal<QuestUpdatedSignal>();
    }
}
public class QuestUpdatedSignal { }
public class QuestStartedSignal
{
    public Quest Quest { get; }
    public QuestStartedSignal(Quest quest) => Quest = quest;
}

public class QuestCompletedSignal
{
    public string QuestID { get; }
    public QuestCompletedSignal(string questID) => QuestID = questID;
}

public class EnemyKilledSignal
{
    public string enemyId { get; }
    public EnemyKilledSignal(string id) => enemyId = id;
}

public class ItemCollectedSignal
{
    public string ItemId { get; }
    public ItemCollectedSignal(string id) => ItemId = id;
}

public class TalkedToNPCSignal
{
    public string npcId { get; }
    public TalkedToNPCSignal(string id) => npcId = id;
}

public class MiniGameCompletedSignal
{
    public string gameId { get; }
    public MiniGameCompletedSignal(string id) => gameId = id;
}
