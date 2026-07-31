using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerSpawnSC : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [SerializeField] GameObject PrefPlayer;
    [SerializeField] string PreviousScene="";
    SpawnPointSC[] SpawnPoints;

    void Awake()
    {
        signalBus.Subscribe<SceneLoadedSignal>(OnSceneLoaded);
        DontDestroyOnLoad(gameObject);
    }

        private void OnDestroy()
    {
        signalBus.Unsubscribe<SceneLoadedSignal>(OnSceneLoaded);
    }
    SpawnPointSC target;
    async void OnSceneLoaded(SceneLoadedSignal signal)
    {
        //SpawnPoints=GameObject.FindGameObjectsWithTag("Respawn");
        SpawnPoints=GameObject.FindObjectsByType<SpawnPointSC>();

        if(SpawnPoints.Length<1)return;

        //if(PreviousScene=="") target = SpawnPoints.FirstOrDefault(obj => obj.name == "SpawnPoint_Start");

        if(PreviousScene=="") target = SpawnPoints.FirstOrDefault(obj => obj.Id == "Default");
        else 
        {
            target = SpawnPoints.FirstOrDefault(obj => obj.Id == PreviousScene);
            if(target==null)target = SpawnPoints.FirstOrDefault(obj => obj.Id == "Default");
        }

        GameObject player = Instantiate(PrefPlayer,target.transform.position,Quaternion.identity);

        PreviousScene=signal.SceneName;

        signalBus.Fire(new PlayerSpawnedSignal(player));
    }

}
