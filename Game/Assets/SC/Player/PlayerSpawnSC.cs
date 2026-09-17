using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerSpawnSC : MonoBehaviour, ISaveable
{
    [Inject] private SignalBus signalBus;
    [Inject] private PlayerControllerSC player;

    [SerializeField] private string previousScene = "";

    private Vector3 savedPosition;
    private bool hasSavedPosition = false;
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

        if (spawnPoints.Length < 1)
        {
            HidePlayer();
            return;
        }

        Vector3 target;

        if (hasSavedPosition)
        {
            target = savedPosition;
            hasSavedPosition = false;
        }
        else
        {
            SpawnPointSC spawnPoint = null;

            if (!string.IsNullOrEmpty(previousScene))
                spawnPoint = spawnPoints.FirstOrDefault(obj => obj.Id == previousScene);

            if (spawnPoint == null)
                spawnPoint = spawnPoints.FirstOrDefault(obj => obj.Id == "Default");

            if (spawnPoint == null)
            {
                Debug.LogError("Нет точки спавна с Id 'Default'!");
                HidePlayer();
                return;
            }

            target = spawnPoint.transform.position;
        }

        SpawnPlayer(target);

        previousScene = signal.SceneName;
        signalBus.Fire(new PlayerSpawnedSignal());
    }

    private void SpawnPlayer(Vector3 position)
    {
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.gameObject.SetActive(true);
        player.transform.position = position;

        if (cc != null) cc.enabled = true;
    }

    private void HidePlayer()
    {
        player.gameObject.SetActive(false);
    }

    public object SaveState()
    {
        return new PlayerSaveData { spawnPosition = player.transform.position };
    }

    public void LoadState(object state)
    {
        var data = (PlayerSaveData)state;
        savedPosition = data.spawnPosition;
        hasSavedPosition = true;
    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public Vector3 spawnPosition;
    }
}