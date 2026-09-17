using System;
using UnityEngine;
using Yarn.Unity;
using Zenject;

public class Installer : MonoInstaller
{
	[SerializeField] GameObject SceneLoader;
	[SerializeField] GameObject PlayerSpawner;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject DialogueSystem;
	[SerializeField] GameObject PhoneUI;
	[SerializeField] GameObject itemPool;
	public override void InstallBindings()
	{
		SignalBusInstaller.Install(Container);

		Container.Bind<SceneLoaderSC>()
			.FromComponentInNewPrefab(SceneLoader)
			.AsSingle();

		Container.Bind<PlayerSpawnSC>()
			.FromComponentInNewPrefab(PlayerSpawner)
			.AsSingle()
			.NonLazy(); 
		
		Container.Bind<PlayerControllerSC>()
			.FromComponentInNewPrefab(playerPrefab)
			.AsSingle();

		Container.Bind<CameraSC>()
			.FromNewComponentOnNewGameObject()
			.AsSingle()
			.NonLazy();
		//Container.Bind<CameraSC>().FromComponentInNewPrefab(CameraManager).AsSingle().NonLazy();

		Container.Bind<DialogueRunner>()
			.FromComponentInNewPrefab(DialogueSystem)
			.AsSingle()
			.NonLazy();

		Container.Bind<MainDialogueUI>()
			.FromResolveGetter<DialogueRunner>(runner => runner.GetComponent<MainDialogueUI>())
			.AsSingle()
			.NonLazy();
		Container.Bind<EscMenu>()
			.FromComponentInNewPrefab(PhoneUI)
			.AsSingle()
			.NonLazy();
		Container.Bind<ItemsDatabase>()
			.FromNewComponentOnNewGameObject()
			.AsSingle()
			.NonLazy();
		Container.Bind<ItemPool>()
			.FromComponentInNewPrefab(itemPool)
			.AsSingle()
			.NonLazy();

		Container.DeclareSignal<SceneLoadedSignal>();
		Container.DeclareSignal<PlayerSpawnedSignal>();
	}
}


public class SceneLoadedSignal
{
    public string SceneName { get; }
    public SceneLoadedSignal(string sceneName)
    {
        SceneName = sceneName;
    }
}
/*public class PlayerSpawnedSignal
{
    public GameObject player { get; }
    public PlayerSpawnedSignal(GameObject _player)
    {
        player = _player;
    }
}*/
public class PlayerSpawnedSignal
{

}