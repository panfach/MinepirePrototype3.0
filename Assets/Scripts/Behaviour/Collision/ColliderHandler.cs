using UnityEngine;
using UnityEngine.EventSystems;

public class ColliderHandler : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    private bool mouseOver;

    public event SimpleEventHandler mouseEnterEvent;
    public event SimpleEventHandler mouseDownEvent;
    public event SimpleEventHandler mouseExitEvent;
    public event SimpleEventHandler mouseDragEvent;
    public event SimpleEventHandler mouseUpEvent;

    public bool MouseOver { get => mouseOver; }


    public void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        mouseOver = true;
        mouseEnterEvent?.Invoke();
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        mouseDownEvent?.Invoke();
    }

    public void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StateManager.VillagerDragging = true;
        mouseDragEvent?.Invoke();
    }

    public void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StateManager.VillagerDragging = false;
        mouseUpEvent?.Invoke();
    }

    public void OnMouseExit()
    {
        mouseOver = false;
        mouseExitEvent?.Invoke();
    }

    public void OnDisable()
    {
        OnMouseExit();
    }
}


