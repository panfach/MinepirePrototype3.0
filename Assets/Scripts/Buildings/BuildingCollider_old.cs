using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingCollider_old : MonoBehaviour
{
    //public static GameObject selectionPlanePrefab;

    //public bool mouseOver;
    //public Building building;

    //GameObject selectionPlane;

    //private void OnMouseEnter()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject()) return;
    //    //EventSystem.current.i
    //    mouseOver = true;
    //    if (building != null)
    //    {
    //        selectionPlane = Instantiate(selectionPlanePrefab, building.transform);
    //        selectionPlane.transform.localScale = new Vector3(0.1f * building.GetSize()[0], 1f, 0.1f * building.GetSize()[1]);
    //        Vector3 posShift = building.currentModelPos - building.transform.position;
    //        posShift.y = selectionPlane.transform.localPosition.y;
    //        selectionPlane.transform.position += posShift;
    //        if (!StateManager.VillagerDragging)
    //            building.SetSmallInfo();
    //    }
    //}

    //private void OnMouseDown()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject()) return;

    //    mouseOver = false;
    //    Connector.panelInvoker.OpenBuildingInfo(building);
    //}

    //private void OnMouseExit()
    //{
    //    mouseOver = false; 
    //    if (selectionPlane != null) Destroy(selectionPlane);
    //    if (!StateManager.VillagerDragging)
    //        building.SetSmallInfo();
    //}

    //private void OnDisable()
    //{
    //    if (building != null)
    //    {
    //        OnMouseExit();
    //        building.SetSmallInfo();
    //    }
    //}
}
