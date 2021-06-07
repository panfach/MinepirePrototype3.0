using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public static List<GameObject> objectsFollowingMouse = new List<GameObject>();

    Ray inputRay;
    RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
        }

        if (objectsFollowingMouse.Count > 0)
        {
            inputRay = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out hit))
            {
                for (int i = 0; i < objectsFollowingMouse.Count; i++)
                {
                    objectsFollowingMouse[i].transform.position = hit.point;
                }
            }
        }
    }
}
