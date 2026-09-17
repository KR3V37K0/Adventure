using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraSC : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [Inject]PlayerControllerSC player;
    CinemachineTargetGroup[] targets;

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
        targets=FindObjectsByType<CinemachineTargetGroup>();
        if(targets.Length==0){Debug.Log("no target camera"); return;}
        targets[0].AddMember(player.transform,1f,1f);
    }
}
