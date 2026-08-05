using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerSpawnSC : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private DiContainer container; // <-- добавляем
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string previousScene = "";

    private SpawnPointSC[] spawnPoints;

    private void Awake()
    {
        signalBus.Subscribe<SceneLoadedSignal>(OnSceneLoaded);
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<SceneLoadedSignal>(OnSceneLoaded);
    }

    private void OnSceneLoaded(SceneLoadedSignal signal)
    {
        spawnPoints = FindObjectsByType<SpawnPointSC>();
        if (spawnPoints.Length < 1) return;

        SpawnPointSC target;
        if (string.IsNullOrEmpty(previousScene))
            target = spawnPoints.FirstOrDefault(obj => obj.Id == "Default");
        else
        {
            target = spawnPoints.FirstOrDefault(obj => obj.Id == previousScene);
            if (target == null) target = spawnPoints.FirstOrDefault(obj => obj.Id == "Default");
        }

        if (target == null)
        {
            Debug.LogError("Нет подходящей точки спавна!");
            return;
        }

        // Создаём игрока через контейнер, чтобы инжекты отработали
        GameObject player = container.InstantiatePrefab(
            playerPrefab,
            target.transform.position,
            Quaternion.identity,
            null
        );

        previousScene = signal.SceneName;
        signalBus.Fire(new PlayerSpawnedSignal(player));
    }
}

