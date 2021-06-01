using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideListener : MonoBehaviour
{
    [Header("Handler")]
    public ColliderHandler colliderHandler;

    private void OnMouseEnter() =>colliderHandler.OnMouseEnter();
    private void OnMouseDown() => colliderHandler.OnMouseDown();
    private void OnMouseExit() => colliderHandler.OnMouseExit();
}
