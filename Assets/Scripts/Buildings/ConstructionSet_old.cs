using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class ConstructionSet_old : Building
{
    //public BuildingCollider activeCollider;

    //float constructionProcess;

    //public float Process
    //{
    //    get => constructionProcess;
    //    set
    //    {
    //        if (deletionFlag == true) return;

    //        constructionProcess = value;
    //        if (constructionMode == ConstructionMode.INSTBLD)
    //        {
    //            FinishConstructionWithParameters();
    //        }
    //        else if (constructionProcess >= GetConstrCost())
    //        {
    //            FinishConstruction();
    //        }

    //        Connector.panelInvoker.RefreshBuildingInfo();
    //    }
    //}

    //// -------------------------------------------------------------------------------------------------- //

    //public void IncreaseProcess(float value)
    //{
    //    Process += value;
    //}

    //public ConstructionSet() { }

    //public ConstructionSet(BuildSet buildSet): base(buildSet)
    //{
    //    constructionProcess = 0;
    //}

    //public override void SetSmallInfo()
    //{
    //    if (smallInfo == null) return;

    //    if (activeCollider.mouseOver)
    //    {
    //        smallInfo.gameObject.SetActive(true);
    //        smallInfo.infoBox.SetActive(activeCollider.mouseOver);
    //        smallInfo.iconBox.SetActive(false);
    //        smallInfo.Refresh();
    //    }
    //    else
    //    {
    //        smallInfo.gameObject.SetActive(false);
    //    }
    //}

    //void FinishConstruction()
    //{
    //    TransformIntoReady();
    //    deletionFlag = true;
    //    VillageData.finishedConstructionsQueue.Add(this);
    //}

    //void FinishConstructionWithParameters()
    //{
    //    TransformIntoReady();
    //    deletionFlag = true;
    //    VillageData.finishedConstructionsQueue.Add(this);
    //}

    //ReadySet TransformIntoReady()
    //{
    //    ReadySet rs = CreateReadyScript();                          // Создание ReadySet и добавление его в главный список зданий
    //    VillageData.AddBuilding(rs);

    //    if (buildModel != null) Destroy(buildModel);                // Удаление ненужных моделей и активация нужной 
    //    Destroy(constructionModel);
    //    readyModel.SetActive(true);

    //    rs.currentModel = readyModel.transform.GetChild(0).gameObject; // Корректировка расположения модели
    //    rs.currentModel.transform.position = new Vector3(rs.currentModelPos.x, rs.currentModel.transform.position.y, rs.currentModelPos.z);

    //    DynamicGameCanvas.buildingSmallInfoList.Remove(smallInfo);  // Пересоздание SmallInfo
    //    Connector.dynamicGameCanvas.SpawnInfo(rs);
    //    rs.SetSmallInfo();

    //    if (BuildingInfo.activeBuilding == this) Connector.panelInvoker.CloseBuildingInfo();

    //    return rs;
    //}

    //ReadySet CreateReadyScript()
    //{
    //    ReadySet rs = gameObject.AddComponent<ReadySet>();
    //    BuildingCopyComponents(rs, this);
    //    rs.uniqueIndex = uniqueIndex;
    //    rs.SetDefaultPeople();
    //    rs.enter.building = rs;
    //    rs.LinkToCollider(readyModel.GetComponentInChildren<BuildingCollider>());

    //    //currentModel = readyModel.transform.GetChild(0).gameObject;
    //    //currentModel.transform.position = currentModelPos;

    //    if (GetBldType() == BuildingType.WAREHOUSE)
    //    {
    //        rs.DefineInventory(8, 8);
    //    }
    //    /*else if (GetBldType() == BuildingType.SKIN)
    //    {
    //        rs.DefineInventory(1, 2);
    //    }*/

    //    tag = tags[2]; //Debug.Log("ConstructionSet CreateReadyScript now tag = " + tag);

    //    return rs;
    //}

    //public void LinkToCollider(BuildingCollider collider)
    //{
    //    activeCollider = collider;
    //    activeCollider.building = this;
    //}

    //public static int CountActive()
    //{
    //    var seq = from item in VillageData.Constructions
    //              where item.deletionFlag == false && item.isActiveAndEnabled
    //              select item;
    //    return seq.Count();
    //}

    ///*public override void Delete()
    //{
    //    deletionFlag = true;
    //    VillageData.finishedConstructionsQueue.Add(this);

    //    //Debug.Log("people = " + people);
    //    // Controlling villagers, which were tide to the building
    //    int count = people;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (GetBldType() == BuildingType.LIVING)
    //            peopleList[0].Evict(this);
    //        else if (GetBldType() == BuildingType.HUNT)
    //            peopleList[0].Dismiss(this);
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
    //}*/

    //public override string ToString()
    //{
    //    return $"{GetName()} ConstructionSet";
    //}

    //public void ScriptSelfDeletion() { Destroy(this); }

    //public override void SelfDeletion() { Destroy(gameObject); }
}
