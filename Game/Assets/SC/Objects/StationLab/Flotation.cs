using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Flotation : MonoBehaviour
{
    [Header("Density")]
    [SerializeField] private float objectDensity = 1f;
    [SerializeField] private float liquidDensity = 1f;

    [Header("Buoyancy")]
    [SerializeField] private float buoyancyForce = 15f;  
    [SerializeField] private float linearDragInLiquid;
    [SerializeField] private float angularDragInLiquid;

    [Header("Reference")]
    [SerializeField] private float surfaceY = 0f;        

    private Rigidbody2D rb;
    private bool inLiquid = false;
    private float defaultLinearDrag;
    private float defaultAngularDrag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultLinearDrag = rb.linearDamping;
        defaultAngularDrag = rb.angularDamping;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Liquid")) return;
        inLiquid = true;
        rb.linearDamping = linearDragInLiquid;
        rb.angularDamping = angularDragInLiquid;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Liquid")) return;
        inLiquid = false;
        rb.linearDamping = defaultLinearDrag;
        rb.angularDamping = defaultAngularDrag;
    }

    private void FixedUpdate()
    {
        if (!inLiquid) return;

        float submergedDepth = Mathf.Max(0f, surfaceY - rb.position.y);

        float buoyancy = submergedDepth * liquidDensity * buoyancyForce / Mathf.Max(0.01f, objectDensity);

        rb.AddForce(Vector2.up * buoyancy, ForceMode2D.Force);
    }
}