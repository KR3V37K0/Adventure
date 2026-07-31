using UnityEngine;
using Zenject;

public class TransitionTriggerSC : MonoBehaviour
{
    [Inject]SceneLoaderSC sceneLoader;
    [SerializeField]string sceneName;

    void OnTriggerEnter2D(Collider2D coll)
    {
        sceneLoader.LoadScene(sceneName);
    }
}
