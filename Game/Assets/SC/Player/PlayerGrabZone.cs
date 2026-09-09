using UnityEngine;

public class PlayerGrabZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        IGrabable grabable = other.GetComponent<IGrabable>();
        if (grabable != null)
        {
            grabable.GrabTo(transform);
        }
    }
}
