using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class ReadySet_old : Building
{
    //[Header("ReadySet values")]
    //public GameObject[] displayedItems;
    //public BuildingCollider activeCollider;
    //public Inventory inventory;
    //public ReadySet warehouse;
    //int people;
    //List<VillagerData> peopleList = new List<VillagerData>();

    //public int People
    //{
    //    get => people;
    //    private set { people = value; }
    //}

    //public VillagerData GetPeople(int i)
    //{
    //    return peopleList[i];
    //}


    //// -------------------------------------------------------------------------------------------------- //

    //public override void SetSmallInfo()
    //{
    //    if (smallInfo == null) return;

    //    if (activeCollider.mouseOver || StateManager.VillagerDragging || (people == 0 && GetMaxPeople() != 0))
    //    {
    //        smallInfo.gameObject.SetActive(true);
    //        smallInfo.infoBox.SetActive(activeCollider.mouseOver || StateManager.VillagerDragging);
    //        smallInfo.iconBox.SetActive(people == 0 && GetMaxPeople() != 0);
    //        smallInfo.Refresh();
    //    }
    //    else
    //    {
    //        smallInfo.gameObject.SetActive(false);
    //    }
    //}

    //public void LinkToCollider(BuildingCollider collider)
    //{
    //    activeCollider = collider;
    //    activeCollider.building = this;
    //}

    //public void DefineInventory(int packSize = 4, int packsAmount = 8)
    //{
    //    inventory = gameObject.AddComponent<Inventory>();
    //    inventory.Init(this, packSize, packsAmount);
    //    inventory.invChangedEvent += RefreshDisplayedItem;

    //    //
    //    //inventory.PutResource(ResourceIndex.RAWVENISON, 8.0f);
    //    //inventory.PutResource(ResourceIndex.RAWDEERSKIN, 8.0f);
    //    //VillageData.resources[(int)ResourceIndex.RAWVENISON] += 8.0f;
    //    //VillageData.resources[(int)ResourceIndex.RAWDEERSKIN] += 8.0f;
    //    RefreshDisplayedItem();
    //    InfoDisplay.Refresh();
    //    //
    //}

    //public bool AddPeople(VillagerData villager)
    //{
    //    if (People < GetMaxPeople())
    //    {
    //        peopleList.Add(villager);
    //        People++;
    //        Connector.panelInvoker.RefreshBuildingInfo();
    //        return true;
    //    }
    //    return false;
    //}

    //public bool RemovePeople(VillagerData villager)
    //{
    //    /*Debug.Log($"ReadySet.RemovePeople() Trying to remove people {villager.Name} {villager.Age}");
    //    foreach(VillagerData item in peopleList)
    //    {
    //        Debug.Log($"peopleList: {item.Name} {item.Age}");
    //    }*/

    //    if (peopleList.Remove(villager))
    //    {
    //        People--;
    //        Connector.panelInvoker.RefreshBuildingInfo();
    //        return true;
    //    }
    //    return false;
    //}

    //public void ForgetPeopleAssignment()
    //{
    //    People = 0;
    //    peopleList.Clear();
    //}

    //public void SetDefaultPeople()
    //{
    //    People = 0;
    //}

    //void RefreshDisplayedItem() // This function is shit
    //{
    //    float shift;

    //    if (displayedItem != null)
    //    {
    //        foreach (GameObject item in displayedItem)  // Возможно это неэффективно всегда удалять отображаемые предметы // 
    //        {
    //            if (item != null) Destroy(item);
    //        }
    //    }

    //    displayedItem = new GameObject[inventory.PackSize * inventory.PacksAmount];
    //    for (int i = 0; i < inventory.PacksAmount; i++)
    //    {
    //        inventory.Look(i, out ResourceIndex index, out float value);
    //        if (index == ResourceIndex.NONE || index == ResourceIndex.DEERSKIN) continue;
    //        //Debug.Log("ReadySet RefreshDisplayedItem index = " + index.ToString());
    //        //if (index == ResourceIndex.NONE) { Debug.Log("ReadySet RefreshDisplayedItem CONTINUE"); continue; }
            

    //        shift = 0f;
    //        for (int j = 0; j < (int)value / 2; j++)
    //        {
    //            Debug.Log(index);
    //            displayedItem[i * inventory.PackSize / 2 + j] = Instantiate(DataList.GetResourceObj(index), itemSpot[i]);
    //            displayedItem[i * inventory.PackSize / 2 + j].transform.localPosition = new Vector3(0f, shift, 0f);
    //            //displayedItem[i].transform.localScale = new Vector3(5f, 2.5f, 5f);
    //            Destroy(displayedItem[i * inventory.PackSize / 2 + j].GetComponent<ResourceInstance>());
    //            shift += 0.1f;
    //        }
    //    }
    //}

    //public static int CountReadyBuildings()
    //{
    //    var seq = from item in VillageData.Buildings
    //              where item.deletionFlag == false && item.isActiveAndEnabled
    //              select item;
    //    return seq.Count();
    //}

    //public override void Delete()
    //{
    //    deletionFlag = true;
    //    VillageData.deletedBuildingsQueue.Add(this);

    //    // Clear inventory, if this is a warehouse
    //    if (inventory != null) inventory.DestroyAllResources();

    //    // Controlling villagers, which were tide to the building
    //    int count = people;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (GetBldType() == BuildingType.LIVING)
    //            peopleList[0].Evict();
    //        else if (GetBldType() == BuildingType.HUNT)
    //            peopleList[0].Dismiss();
    //    }

    //    // Controlling UI things
    //    smallInfo.Delete();
    //    Connector.panelInvoker.CloseBuildingInfo();

    //    // Clear small grid cells from building
    //    foreach (CellPointer item in cellPointer)
    //    {
    //        item.Free();
    //    }

    //    // Hiding object (Now the object is waiting for its final deletion)
    //    transform.position = CellMetrics.hidedObjects;
    //    gameObject.SetActive(false);
    //}

    //public override string ToString()
    //{
    //    return $"{GetName()} ReadySet";
    //}

    //public override void SelfDeletion() { Destroy(gameObject); }
}
