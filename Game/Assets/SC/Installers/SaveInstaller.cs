using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.IO;

public class SaveInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveSystem>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<ISaveable>().To<QuestSystem>().FromResolve();
        Container.Bind<ISaveable>().To<Inventory>().FromResolve();
        Container.Bind<ISaveable>().To<PlayerSpawnSC>().FromResolve();
        Container.Bind<YarnSaveAdapter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<ISaveable>().To<YarnSaveAdapter>().FromResolve();

        Container.Bind<List<ISaveable>>().FromMethod(ctx =>
        {
            var list = new List<ISaveable>();
            list.Add(ctx.Container.Resolve<QuestSystem>());
            list.Add(ctx.Container.Resolve<Inventory>());
            list.Add(ctx.Container.Resolve<PlayerSpawnSC>());
            list.Add(ctx.Container.Resolve<YarnSaveAdapter>());
            return list;
        }).AsSingle();

        Container.DeclareSignal<SaveLoadedSignal>();
    }
    
}
public class SaveLoadedSignal { }
