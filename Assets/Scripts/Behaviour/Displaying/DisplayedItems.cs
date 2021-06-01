using System.Collections.Generic;
using UnityEngine;

public class DisplayedItems : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool hasSelectionPlane;
    [SerializeField] bool hasSilhouette;
    [SerializeField] bool hasDisplayedInventory;
    [SerializeField] Transform[] itemSpot;
    [SerializeField] GameObject selectionPlane;
    [SerializeField] GameObject silhouette;

    GameObject[] displayedItem;

    public bool HasSelectionPlane { get => hasSelectionPlane; }


    private void OnEnable()                                                                                           // Add drag link to silhouette
    {
        if (hasSelectionPlane && entity.ColliderHandler != null)
        {
            HideSelectionPlane();
            entity.ColliderHandler.mouseEnterEvent += DisplaySelectionPlane;
            entity.ColliderHandler.mouseExitEvent += HideSelectionPlane;
        }
        if (hasDisplayedInventory && entity.Inventory != null)
        {
            displayedItem = new GameObject[entity.Inventory.PackSize * entity.Inventory.PacksAmount];
            entity.Inventory.invChangedEvent += DisplayInventory;
        }
        if (hasSilhouette && entity.ColliderHandler != null)
        {
            entity.ColliderHandler.mouseDragEvent += DisplaySilhouette;
        }
    }


    public void DisplaySelectionPlane()
    {
        selectionPlane.SetActive(true);
        if (entity.BuildSet != null) selectionPlane.transform.position = new Vector3(entity.BuildSet.ModelPos.x, selectionPlane.transform.position.y, entity.BuildSet.ModelPos.z);
    }

    public void DisplaySilhouette()
    {
        silhouette.SetActive(true);                                                              // Silhouette must be moved by some input controller (in update)
    }

    public void HideSelectionPlane()
    {
        selectionPlane.SetActive(false);
    }

    public void HideSilhouette()
    {
        silhouette.SetActive(false);
    }

    public void DisplayInventory() // This function is shit                      // Rewrite this function after "Inventory" rewriting
    {
        if (!entity.Inventory.enabled) return;
        float shift;

        if (displayedItem != null)
        {
            foreach (GameObject item in displayedItem)  // Возможно это неэффективно всегда удалять отображаемые предметы // 
            {
                if (item != null) Destroy(item);
            }
        }

        for (int i = 0; i < entity.Inventory.PacksAmount; i++)
        {
            entity.Inventory.Look(i, out ResourceIndex index, out float value);
            if (index == ResourceIndex.NONE || index == ResourceIndex.DEERSKIN) continue;
            //Debug.Log("ReadySet RefreshDisplayedItem index = " + index.ToString());
            //if (index == ResourceIndex.NONE) { Debug.Log("ReadySet RefreshDisplayedItem CONTINUE"); continue; }


            shift = 0f;
            for (int j = 0; j <= (int)value / 2; j++)
            {
                Debug.Log(index);
                displayedItem[i * entity.Inventory.PackSize / 2 + j] = Instantiate(DataList.GetResourceModel(index), itemSpot[i]);
                displayedItem[i * entity.Inventory.PackSize / 2 + j].transform.localPosition = new Vector3(0f, shift, 0f);
                //displayedItem[i].transform.localScale = new Vector3(5f, 2.5f, 5f);
                //Destroy(displayedItem[i * entity.Inventory.PackSize / 2 + j].GetComponent<ResourceInstance>());
                shift += 0.1f;
            }
        }
    }

    public void HideInventory()
    {
        for (int i = 0; i < displayedItem.Length; i++)
        {
            if (displayedItem[i] != null) Destroy(displayedItem[i]);
            displayedItem[i] = null;
        }
    }

    public void SetParentOfSelectionPlane(Transform newParent)
    {
        selectionPlane.transform.SetParent(newParent);
    }


    private void OnDisable()
    {
        if (hasSelectionPlane && entity.ColliderHandler != null)
        {
            entity.ColliderHandler.mouseEnterEvent -= DisplaySelectionPlane;
            entity.ColliderHandler.mouseExitEvent += HideSelectionPlane;
        }
        if (hasDisplayedInventory && entity.Inventory != null)
        {
            HideInventory();
            displayedItem = null;
            entity.Inventory.invChangedEvent -= DisplayInventory;
        }
        if (hasSilhouette && entity.ColliderHandler != null)
        {
            entity.ColliderHandler.mouseDragEvent -= DisplaySilhouette;
        }
    }
}
