using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClickDebugger : MonoBehaviour
{
    private void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (EventSystem.current == null)
        {
            Debug.LogError("EventSystem не найден в сцене!");
            return;
        }

        PointerEventData data = new PointerEventData(EventSystem.current) { position = mousePos };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        if (results.Count == 0)
        {
            Debug.Log("Клик никуда не попал.");
            return;
        }

        Debug.Log($"=== Клик обработал {results.Count} объектов (сверху вниз) ===");
        foreach (var r in results)
        {
            string path = GetFullPath(r.gameObject);
            string raycast = GetRaycastTarget(r.gameObject);
            Debug.Log($"→ {path}\n   layer: {r.gameObject.layer} | raycastTarget: {raycast} | depth: {r.depth}");
        }
    }

    private string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform t = obj.transform.parent;
        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }
        return path;
    }

    private string GetRaycastTarget(GameObject obj)
    {
        var img = obj.GetComponent<Image>();
        if (img != null) return $"Image({img.raycastTarget})";

        var txt = obj.GetComponent<TMP_Text>();
        if (txt != null) return $"TMP_Text({txt.raycastTarget})";

        var raw = obj.GetComponent<RawImage>();
        if (raw != null) return $"RawImage({raw.raycastTarget})";

        return "no-graphic";
    }
}