using UnityEngine;
using Zenject;

public class PlayerDestroyer : MonoBehaviour
{
    [Inject]SignalBus bus;
    void OnEnable()
    {
        bus.Subscribe<SceneLoadedSignal>(Destroy);
    }
    void OnDisable()
    {
        bus.Unsubscribe<SceneLoadedSignal>(Destroy);
    }
    void Destroy(SceneLoadedSignal signal)
    {
        DestroyImmediate(gameObject);
    }

}
