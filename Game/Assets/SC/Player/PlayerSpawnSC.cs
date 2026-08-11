using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerSpawnSC : MonoBehaviour,ISaveable
{
    [Inject] private SignalBus signalBus;
    [Inject] private DiContainer container; 
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string previousScene = "";
    Vector3 SpawnPosition { get; set; }
    GameObject player;


    public object SaveState()
    {
        return new PlayerSaveData
        {
            spawnPosition = player.transform.position,
        };
    }

    public void LoadState(object state)
    {
        var data = (PlayerSaveData)state;
        SpawnPosition = data.spawnPosition;
    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public Vector3 spawnPosition;
    }

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

        Vector3 target;
        if (SpawnPosition != null)
        {
            target = SpawnPosition;
        }
        else
        {
            if (string.IsNullOrEmpty(previousScene))
                target = spawnPoints.FirstOrDefault(obj => obj.Id == "Default").transform.position;
            else
            {
                target = spawnPoints.FirstOrDefault(obj => obj.Id == previousScene).transform.position;
                if (target == null) target = spawnPoints.FirstOrDefault(obj => obj.Id == "Default").transform.position;
            }
        }

        if (target == null)
        {
            Debug.LogError("Нет подходящей точки спавна!");
            return;
        }

        SpawnPlayer(target);

        previousScene = signal.SceneName;
        signalBus.Fire(new PlayerSpawnedSignal(player));
    }
    void SpawnPlayer(Vector3 position)
    {   player = container.InstantiatePrefab(
            playerPrefab,
            position,
            Quaternion.identity,
            null
        );
    }
}

