using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class BuildSet_old : Building
{
    //[Header("BuildSet values")]
    //public BuildingCollider activeCollider;

    //int desiredUniqueIndex, desiredProcess;
    ////CellPointer[] pointers = null;

    //// -------------------------------------------------------------------------------------------------- //

    //void Awake()
    //{
    //    buildModel.GetComponentInChildren<BoxCollider>().enabled = false;
    //}

    //// -------------------------------------------------------------------------------------------------- //

    //public bool TryBuild(int _uniqueIndex = 0)
    //{
    //    if (!CheckResourceCost() || !TryOccupyPlace()) return false;

    //    SpendResource();
    //    desiredUniqueIndex = _uniqueIndex;
    //    TransformIntoConstruction();
        
    //    return true;
    //}

    //public bool TryConstruct(int _uniqueIndex = 0, int _process = 0)
    //{
    //    if (!CheckResourceCost() || !TryOccupyPlace()) return false;
    //    desiredUniqueIndex = _uniqueIndex;
    //    TransformIntoConstruction(_process);

    //    return true;
    //}

    //// -------------------------------------------------------------------------------------------------- //

    //bool TryOccupyPlace()
    //{
    //    if (constructionMode == ConstructionMode.ORD)
    //    {
    //        bool enterAvialability = false;
    //        foreach (CellPointer pointer in cellPointer)
    //        {
    //            if (pointer.enterCellPointer)
    //            {
    //                if (pointer.isEnableForBuild) enterAvialability = true;
    //            }
    //            else 
    //            {
    //                if (!pointer.isEnableForBuild)
    //                {
    //                    Notification.Invoke(NotifType.PLACEBUILD);
    //                    return false;
    //                }
    //            }
    //        }

    //        if (!enterAvialability)
    //        {
    //            Notification.Invoke(NotifType.PLACEBUILD);
    //            return false;
    //        }
    //    }

    //    SCCoord coord = SCCoord.FromPos(GeneralBuilder.hitPointBuild + CellMetrics.smallCellCenterShift);
    //    int[] size = GeneralBuilder.buildModeObject.GetComponent<Building>().GetSize();
    //    OccupyPlace();
    //    coordinates = coord;
    //    HideCellPointers();

    //    return true;
    //}

    
    //bool CheckResourceCost()
    //{
    //    if (constructionMode == ConstructionMode.ORD)
    //    {
    //        float value;
    //        ResourceQuery resQuery = GetResourceCost();
    //        IEnumerable<ReadySet> warehousesArray = from item in VillageData.Buildings
    //                                                let _item = (ReadySet)item
    //                                                where (_item.GetBldType() == BuildingType.WAREHOUSE && !_item.deletionFlag)
    //                                                select _item;

    //        for (int i = 0; i < resQuery.index.Length; i++)
    //        {
    //            value = resQuery.indexVal[i];

    //            foreach (ReadySet rs in warehousesArray)
    //            {
    //                for (int j = 0; j < rs.inventory.PacksAmount; j++)
    //                {
    //                    rs.inventory.Look(j, out ResourceIndex outInd, out float outValue);
    //                    //Debug.Log(" : : : Checking warehouse: j=" + j + " resQuery.index[i]" + resQuery.index[i] + " outInd" + outInd + " outValue" + outValue);
    //                    if (outInd != resQuery.index[i]) continue;
    //                    value -= outValue;
    //                    if (value <= 0) break;
    //                }
    //                if (value <= 0) break;
    //            }

    //            if (value > 0) { /*Debug.Log("Not enough resources! Need " + value + " " + resQuery.index[i] + " more");*/ Notification.Invoke(NotifType.RESBUILD); return false; }
    //            //else Debug.Log("There is necessary amount of " + resQuery.index[i]);
    //        }
    //    }

    //    //if (VillageData.resources[(int)resQuery.index[i]] < resQuery.indexVal[i]) return false;

    //    return true;
    //}

    //void SpendResource()
    //{
    //    float value;
    //    ResourceQuery resQuery = GetResourceCost();
    //    IEnumerable<ReadySet> warehousesArray = from item in VillageData.Buildings
    //                                            let _item = (ReadySet)item
    //                                            where (_item.GetBldType() == BuildingType.WAREHOUSE && !_item.deletionFlag)
    //                                            select _item;

    //    for (int i = 0; i < resQuery.index.Length; i++)
    //    {
    //        value = resQuery.indexVal[i];
    //        VillageData.resources[(int)resQuery.index[i]] -= value;

    //        foreach (ReadySet rs in warehousesArray)
    //        {
    //            for (int j = 0; j < rs.inventory.PacksAmount; j++)
    //            {
    //                rs.inventory.Look(j, out ResourceIndex outInd, out float outValue);
    //                if (outInd != resQuery.index[i]) continue;
    //                rs.inventory.TakeResource(j, value, out ResourceIndex takeInd, out float takeValue);
    //                value -= takeValue;
    //                if (value <= 0)
    //                {
    //                    if (value < 0) rs.inventory.PutResource(resQuery.index[i], -value);
    //                    break;
    //                }
    //            }
    //            if (value <= 0) break;
    //        }

    //        if (value > 0) return;
    //    }

    //    InfoDisplay.Refresh();
    //}

    ////======================================= Метка =====================================//
    //ConstructionSet TransformIntoConstruction(int _process = 0)
    //{
    //    desiredProcess = _process;

    //    Destroy(buildModel);                                 // Удаление ненужных моделей и активация нужной 
    //    constructionModel.SetActive(true);

    //    ConstructionSet cs = CreateConstructionScript();     // Создание ConstructionSet и добавление его в главный список строящихся зданий
    //    VillageData.AddConstruction(cs);

    //    cs.currentModel = constructionModel.transform.GetChild(0).gameObject; // Корректировка расположения модели
    //    cs.currentModel.transform.position = new Vector3(cs.currentModelPos.x, cs.currentModel.transform.position.y, cs.currentModelPos.z);

    //    Connector.dynamicGameCanvas.SpawnInfo(cs);           // Создание SmallInfo
    //    cs.SetSmallInfo();

    //    VillagerManager.DefineBehaviourOfFreeLaborers();

    //    return cs;
    //}

    //ConstructionSet CreateConstructionScript()
    //{
    //    ConstructionSet cs = gameObject.AddComponent<ConstructionSet>();
    //    BuildingCopyComponents(cs, this);
    //    cs.AssignUniqueIndex(desiredUniqueIndex);
    //    //Debug.Log("maxUniqueIndex = " + maxUniqueIndex);
    //    //Debug.Log("desiredUnuqueIndex = " + desiredUniqueIndex);
    //    VillageData.uniqIndexDict.Add(cs.uniqueIndex, gameObject);
    //    cs.LinkToCollider(constructionModel.GetComponentInChildren<BuildingCollider>());

    //    //currentModel = constructionModel.transform.GetChild(0).gameObject;
    //    //currentModel.transform.position = currentModelPos;

    //    tag = tags[1];

    //    cs.IncreaseProcess(desiredProcess);

    //    return cs;
    //}

    //void HideCellPointers()
    //{
    //    foreach (CellPointer item in cellPointer)
    //    {
    //        item.gameObject.SetActive(false);
    //    }
    //}

    //void OccupyPlace()
    //{
    //    foreach (CellPointer item in cellPointer) 
    //    {
    //        item.Occupy();
    //    }
    //}

    //public override string ToString()
    //{
    //    return $"{GetName()} BuildSet";
    //}
}
