using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Table : MonoBehaviour
{
    [Inject]Inventory inventory;
    private float destroyDelay = 2f;

    private Dictionary<GameObject, Coroutine> timers = new Dictionary<GameObject, Coroutine>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ObjectInFlask")) return;
        if (timers.ContainsKey(other.gameObject)) return;

        timers[other.gameObject] = StartCoroutine(DestroyAfterDelay(other.gameObject));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("ObjectInFlask")) return;
        if (timers.TryGetValue(other.gameObject, out var routine))
        {
            if (routine != null) StopCoroutine(routine);
            timers.Remove(other.gameObject);
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (target != null)
            Destroy(target);

        inventory.AddItem(target.GetComponent<ObjectInFlask>().data.name);
        timers.Remove(target);      
    }
}
