using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TemperatureRotor : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform Rect;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float sensitivity;
    [SerializeField] private float minTemperature; // выставишь в инспекторе
    [SerializeField] private float maxTemperature; // выставишь в инспекторе

    private StationLab station;
    private float currentAngle = 0f;
    private bool isDragging = false;

    private void Awake()
    {   
        station = GetComponentInParent<StationLab>();
        if (Rect == null)
            Rect = GetComponent<RectTransform>();
        currentAngle = maxAngle;
        ApplyRotation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        float delta = eventData.delta.x * sensitivity;
        currentAngle = Mathf.Clamp(currentAngle + delta, minAngle, maxAngle);
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        Rect.rotation = Quaternion.Euler(0, 0, currentAngle);

        float normalized = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        float temperature = Mathf.Lerp(maxTemperature, minTemperature, normalized);

        station.OnTemperatureChanged?.Invoke((int)temperature);
        Debug.Log($"Температура: {temperature}");
    }

    public void SetValue(float angle)
    {
        currentAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        ApplyRotation();
    }
}