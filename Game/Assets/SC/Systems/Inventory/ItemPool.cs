using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int prewarmCount = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    [Inject] private DiContainer container;

    private void Awake()
    {
        for (int i = 0; i < prewarmCount; i++)
        {
            GameObject obj = CreateNew();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private GameObject CreateNew()
    {
        // Создаём через Zenject, чтобы все зависимости инжектировались
        return container.InstantiatePrefab(itemPrefab);
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = container.InstantiatePrefab(itemPrefab, position, rotation, null);
        }
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}