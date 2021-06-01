using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceSourceInstance_old : MonoBehaviour
{
    /*public static GameObject selectionPlanePrefab;

    [Header("Data")]
    public ResourceSource data;
    public SmallResourceSourceInfo smallInfo;
    public ResourceDeposit[] deposit;
    public bool deletionFlag = false;
    public float smallInfoHeight;

    [Header("Links")]
    public Transform mainModel;
    public Transform interactPoint;

    bool mouseOver;
    GameObject selectionPlane;

    private void Awake()
    {
        if (mainModel == null) mainModel = transform.GetChild(0);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        mouseOver = true;
        selectionPlane = Instantiate(selectionPlanePrefab, transform);
        selectionPlane.transform.position += CellMetrics.smallCellCenterShift;
        selectionPlane.transform.localScale = new Vector3(0.1f, 0f, 0.1f);
        SetSmallInfo();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            mouseOver = false;
            Connector.panelInvoker.OpenResourceSourceInfo(this);
        }
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        if (selectionPlane != null) Destroy(selectionPlane);
        SetSmallInfo();
    }

    /// <summary>
    /// Данная функция должна сама понимать, включать или выключать SmallInfo
    /// </summary>
    public void SetSmallInfo()
    {
        bool extractable = ExtractableResourceExists();
        if (mouseOver || extractable)
        {
            smallInfo.gameObject.SetActive(true);
            smallInfo.infoBox.SetActive(mouseOver);
            smallInfo.iconBox.SetActive(ExtractableResourceExists());
            smallInfo.Refresh();
        }
        else
        {
            smallInfo.gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        deposit = new ResourceDeposit[data.resources.Length];
        for (int i = 0; i < deposit.Length; i++) deposit[i] = new ResourceDeposit(this, i, data.resources[i].amount);
    }

    public bool ExtractableResourceExists()
    {
        foreach (ResourceDeposit item in deposit)
        {
            if (item.extractable) return true;
        }

        return false;
    }

    public void Die()
    {
        deletionFlag = true;
        VillageData.deletedResourceSourceInstancesQueue.Add(this);
        if (ResourceSourceInfo.activeSource == this) Connector.panelInvoker.CloseResourceSourceInfo();
        if (smallInfo != null) smallInfo.Delete();

        transform.position = CellMetrics.hidedObjects;
        gameObject.SetActive(false);
    }

    public void SelfDeletion() { Destroy(gameObject); }*/
}
