using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Item_toDrag_InLab : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float scatterRadius = 0.3f;

    private Vector3 randomOffset;
    private Camera cam;
    private Coroutine move;
    private bool inLiquid = false;

    private void Awake()
    {
        cam = Camera.main;
        randomOffset = Random.insideUnitCircle * scatterRadius;
        move = StartCoroutine(Move());
        SetPhysic(false);
    }

    private void Update()
    {
        if (inLiquid)
        {
            if (move != null) StopCoroutine(move);
            move = null;
            GetComponent<ObjectInFlask>().PutOnLiquid();
            this.enabled = false;
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (Mouse.current == null) yield break;

            if (!Mouse.current.leftButton.isPressed)
            {
                SetPhysic(true);
                move = null;
                yield break;
            }

            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 world = cam.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -cam.transform.position.z)
            );

            transform.position = world + randomOffset;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Liquid") inLiquid = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Liquid") inLiquid = false;
    }

    private void SetPhysic(bool active)
    {
        if (active)
            rb.constraints = RigidbodyConstraints2D.None;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}