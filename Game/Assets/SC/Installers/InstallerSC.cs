using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
	[SerializeField] GameObject SceneLoader;
	[SerializeField] GameObject PlayerSpawner;
	[SerializeField] GameObject CameraManager;
	[SerializeField] GameObject DialogueSystem;
	public override void InstallBindings()
	{
		SignalBusInstaller.Install(Container);

		Container.Bind<SceneLoaderSC>().FromComponentInNewPrefab(SceneLoader).AsSingle();
		Container.Bind<PlayerSpawnSC>().FromComponentInNewPrefab(PlayerSpawner).AsSingle().NonLazy(); //ля, перепиши чтоб не нужно было префаб создавать
		Container.Bind<CameraSC>().FromComponentInNewPrefab(CameraManager).AsSingle().NonLazy();
		Container.Bind<MainDialogueUI>().FromComponentInNewPrefab(DialogueSystem).AsSingle().NonLazy();

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
public class PlayerSpawnedSignal
{
    public GameObject player { get; }
    public PlayerSpawnedSignal(GameObject _player)
    {
        player = _player;
    }
}