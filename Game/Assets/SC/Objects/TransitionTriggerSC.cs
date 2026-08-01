using UnityEngine;
using Zenject;

public class TransitionTriggerSC : MonoBehaviour
{
    [Inject]SceneLoaderSC sceneLoader;
    [SerializeField]string sceneName;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Player"))
            sceneLoader.LoadScene(sceneName);
    }
}
