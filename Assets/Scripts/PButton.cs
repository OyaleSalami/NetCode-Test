using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressed = false;
    public UnityEvent onClick = new();

    void Update()
    {
        if (pressed)
        {
            onClick.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
