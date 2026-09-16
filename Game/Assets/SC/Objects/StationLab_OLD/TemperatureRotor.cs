using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class TemperatureRotor : MonoBehaviour
{
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float sensitivity;
    [SerializeField] private float minTemperature;
    [SerializeField] private float maxTemperature;


    private float currentAngle = 0f;
    private bool isDragging = false;
    private Collider2D col;
    private Camera cam;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        cam = Camera.main;
        currentAngle = maxAngle;
        ApplyRotation();
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        if (cam == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = cam.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, -cam.transform.position.z)
        );
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        if (Mouse.current.leftButton.wasPressedThisFrame && col.OverlapPoint(mousePos))
            isDragging = true;

        if (Mouse.current.leftButton.wasReleasedThisFrame)
            isDragging = false;

        if (isDragging)
        {
            float delta = -Mouse.current.delta.ReadValue().x * sensitivity;
            currentAngle = Mathf.Clamp(currentAngle + delta, minAngle, maxAngle);
            ApplyRotation();
        }
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        float normalized = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        int temperature = (int)Mathf.Lerp(maxTemperature, minTemperature, normalized);

        Lab.Instance.OnTemperatureChanged?.Invoke(temperature);
    }

    public void SetValue(float angle)
    {
        currentAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        ApplyRotation();
    }
}