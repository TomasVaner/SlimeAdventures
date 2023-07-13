using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse left");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("Left mouse down");
                break;
            case PointerEventData.InputButton.Right:
                Debug.Log("Right mouse down");
                break;
            case PointerEventData.InputButton.Middle:
                Debug.Log("Middle mouse down");
                break;
            
        }
    }
}
