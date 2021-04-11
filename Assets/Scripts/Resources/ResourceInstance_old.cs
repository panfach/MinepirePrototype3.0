using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceInstance_old : MonoBehaviour
{
    /*public static GameObject selectionPlanePrefab;

    [Header("Data")]
    public Resource data;
    public float size;
    public bool occupied; 
    public Villager owner;
    public SmallResourceInfo smallInfo;

    [Header("Models")]
    public Transform mainModel;                                         // Does it need?

    public bool deletionFlag;

    public bool MouseOver { get; private set; }

    GameObject selectionPlane;

    //public float 

    private void Awake()                                                 // Does it need?
    {
        if (mainModel == null) mainModel = transform.GetChild(0);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        MouseOver = true;
        selectionPlane = Instantiate(selectionPlanePrefab, transform);
        selectionPlane.transform.localScale = new Vector3(0.05f, 0f, 0.05f);
        SetSmallInfo(true);
    }

    private void OnMouseExit()
    {
        MouseOver = false;
        if (selectionPlane != null) Destroy(selectionPlane);
        SetSmallInfo(false);
    }

    public void SetSmallInfo(bool state)
    {
        if (state)
        {
            smallInfo.gameObject.SetActive(true);
            smallInfo.Refresh();
        }
        else
        {
            smallInfo.gameObject.SetActive(false);
        }
    }

    public bool Occupy(Villager villager)
    {
        if ((!occupied || villager == owner) && villager.inventory.CheckPlaceFor(data.index))
        {
            occupied = true;
            owner = villager;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveOccupation(Villager _villager)
    {
        if (_villager == owner)
        {
            occupied = false;
            owner = null;
        }
    }

    public void Die()
    {
        deletionFlag = true;
        smallInfo.Delete();
        VillageData.deletedResourceInstancesQueue.Add(this);

        transform.position = CellMetrics.hidedObjects;
        gameObject.SetActive(false);
    }

    public void SelfDeletion() { Destroy(gameObject); }*/
}
