using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraSC : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    CinemachineTargetGroup target;

    void OnEnable()
    {
        signalBus.Subscribe<PlayerSpawnedSignal>(OnSceneLoaded);
        DontDestroyOnLoad(gameObject);
    }
    void OnDisable()
    {
        signalBus.Unsubscribe<PlayerSpawnedSignal>(OnSceneLoaded);
    }
    void OnSceneLoaded(PlayerSpawnedSignal signal)
    {
        target=FindObjectsByType<CinemachineTargetGroup>()[0];
        if(target==null){Debug.Log("no target camera"); return;}
        target.AddMember(signal.player.transform,1f,1f);
    }
}
