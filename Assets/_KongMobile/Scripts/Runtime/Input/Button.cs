using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool isPressed = default;
    public bool IsPressed { get { return isPressed; } }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

}
