using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт должен висеть на GameManager.
/// </summary>
public class GeneralBuilder : MonoBehaviour
{
    public static BuildingAngle buildingAngle = new BuildingAngle(0);

    //[Header("Buildset prefabs")]
    //public BuildingList buildSetsList;
    public Color[] cellPointerColors;
    /*public GameObject buildModeBarn;
    public GameObject buildModeHut;
    public GameObject buildModeHunterHut;
    public GameObject buildModelBuilderHut;
    public GameObject buildModeRoad;
    public GameObject buildModeWoodcutterShack;
    public GameObject buildModeMine;
    public GameObject buildModeWell; */

    //[Header("Additional prefabs")]
    //public GameObject clickOnGroundEffect;
    //public GameObject putHouseEffect;

    //[Header("Classes")]
    //public BuildTooltipTrigger buildTooltipTrigger;
    //public BuildingInfoTrigger infoTrigger;
    //public NotificationTrigger notification;

    //[Header("Others")]
    //[SerializeField] float roadbuildingTimeDelay = 0.05f;

    //[Header("Execution data")]
    //public int wrongPlaceCounter = 0;
    //public int rotAngle;
    //[SerializeField] int roadBuildStage, buildMode;
    //int oldxDiff, oldzDiff;
    [SerializeField] float raycastLength = 1000 /*road_time*/;
    //public bool mouseOverUI = false, roadBuildMode = false, rayIgnoresBuildings = false, settlingBuildManager, roadBuildingPermission = true;
    public static Vector3 hitPointBuild, oldPos;
    //[SerializeField] List<GameObject> roadTail;
    public static GameObject buildModeObject;

    //// Дополнительно
    //Transform thisTransform;
    Transform activeBuildingTransform;
    Building activeBuilding;
    //GameObject canvas;
    //TurnManager turnManager;
    //StateManager sm;
    //GameData gameData;
    //CameraMove cameraMove;
    //CameraShake cameraShake;
    RaycastHit hit;
    Ray ray;

    private void Awake()
    {
        CellPointer.colors = cellPointerColors;
    }

    private void Start()
    {
        //List<int> sss = new List<int>();
        //Debug.Log((sss.ToArray().Length));
        //thisTransform = transform;
        //canvas = GameObject.Find("Canvas");
        //turnManager = GetComponent<TurnManager>();
        //sm = GetComponent<StateManager>();
        //gameData = GetComponent<GameData>();
        //cameraMove = mainCamera.GetComponent<CameraMove>();
        //cameraShake = mainCamera.GetComponent<CameraShake>();
    }

    // --------------------------- Нажатие на кнопку здания --------------------------- //
    public void BuildModeActivate(BuildingIndex buildingIndex = BuildingIndex.PRIMHUT)
    {


        if (StateManager.GeneralState != GameState.BUILD) 
        {
            if (StateManager.SetBuildMode()) /*|| sm.getShowInfoMode()*/
            {
                Vector3 _pos = hit.point;

                // 0 - PrimevalHut
                // 1 - StorageTent

                //if (buildingName.Equals("")) return;
                buildModeObject = Instantiate(DataList.GetBuildingObj(buildingIndex), _pos, Quaternion.identity);

                activeBuildingTransform = buildModeObject.transform;
                activeBuilding = buildModeObject.GetComponent<Building>();
                buildingAngle.Set(activeBuilding.GridObject.angle.Index);
                RefreshModelPosition();

                Connector.panelInvoker.buildingRotationTooltip.SetActive(true);
            }
        }
        else
        {
            StateManager.SetOrdinaryMode();
            if (buildModeObject != null) Destroy(buildModeObject);

            Connector.panelInvoker.buildingRotationTooltip.SetActive(false);
        }

        /*
        if (!sm.getBuildMode() && (sm.getOrdinaryMode() || sm.getShowInfoMode()))
        {
            if (sm.getShowInfoMode())
            {
                infoTrigger.TurnOff();
            }
            buildTooltipTrigger.TurnOn();
            sm.setBuildMode(true);
            buildMode = mode;
            Vector3 _pos = hit.point;
            _pos.y = 0.5f;

            // 1 - Barn
            // 2 - Hut
            // 3 - Hunter hut
            // 4 - Builder hut
            // 5 - Road
            // 6 - Woodcutter Shack
            // 7 - Mine
            // 8 - Well

            switch (mode)
            {
                case 1:
                    buildModeObject = Instantiate(buildModeBarn, _pos, Quaternion.identity) as GameObject;
                    break;
                case 2:
                    buildModeObject = Instantiate(buildModeHut, _pos, Quaternion.identity) as GameObject;
                    break;
                case 3:
                    buildModeObject = Instantiate(buildModeHunterHut, _pos, Quaternion.identity) as GameObject;
                    break;
                case 4:
                    buildModeObject = Instantiate(buildModelBuilderHut, _pos, Quaternion.identity) as GameObject;
                    break;
                case 5:
                    roadBuildMode = true;
                    roadBuildStage = 1;
                    roadTail = null;
                    buildModeObject = Instantiate(buildModeRoad, _pos, Quaternion.identity) as GameObject;
                    roadTail = new List<GameObject>();
                    break;
                case 6:
                    buildModeObject = Instantiate(buildModeWoodcutterShack, _pos, Quaternion.identity) as GameObject;
                    infoTrigger.TurnOnTF2Color();
                    break;
                case 7:
                    buildModeObject = Instantiate(buildModeMine, _pos, Quaternion.identity) as GameObject;
                    break;
                case 8:
                    buildModeObject = Instantiate(buildModeWell, _pos, Quaternion.identity) as GameObject;
                    break;
            }
            rotAngle = 0;
            buildModeObject.GetComponent<BuildsetScript>().mode = mode;
            objTransform = buildModeObject.transform;
            return;
        }
        if (sm.getBuildMode())
        {
            sm.OnOrdinaryMode();
            buildTooltipTrigger.TurnOff();
            Destroy(buildModeObject);
        }*/
    }

    public GameObject InstantBuilding(BuildingIndex buildingIndex, SCCoord coord, int uniqueIndex = 0, int angl = 0)
    {
        Vector3 _pos = SCCoord.GetCorner(coord, SCCoord.GetHeight(coord));
        hitPointBuild = _pos;

        buildModeObject = Instantiate(DataList.GetBuildingObj(buildingIndex), _pos, Quaternion.identity);

        activeBuildingTransform = buildModeObject.transform;
        activeBuilding = buildModeObject.GetComponent<Building>();
        RotateBuildingClockwise(angl);
        buildingAngle.Set(activeBuilding.GridObject.angle.Index);
        RefreshModelPosition();

        InstantlyBuild(uniqueIndex);
        return buildModeObject;
    }

    public GameObject InstantConstruction(BuildingIndex buildingIndex, SCCoord coord, int uniqueIndex = 0, int processValue = 0, int angl = 0)
    {
        Vector3 _pos = SCCoord.GetCorner(coord, SCCoord.GetHeight(coord));
        hitPointBuild = _pos;

        buildModeObject = Instantiate(DataList.GetBuildingObj(buildingIndex), _pos, Quaternion.identity);

        activeBuildingTransform = buildModeObject.transform;
        activeBuilding = buildModeObject.GetComponent<Building>();
        RotateBuildingClockwise(angl);
        buildingAngle.Set(activeBuilding.GridObject.angle.Index);
        RefreshModelPosition();

        InstantlyConstruct(uniqueIndex, processValue);
        return buildModeObject;
    }


    // Отображение "хвоста" при строительстве дороги
    /*public void DrawTail()
    {
        int i;
        int straightLength;
        int obliqueLength;
        int direction = 4; // 0, 1 - x; 2, 3 - z; 4 - point
        int xDiff = (int)((hitPointBuild.x - oldPos.x) / 2);
        int zDiff = (int)((hitPointBuild.z - oldPos.z) / 2);
        Vector3 pos = oldPos;
        GameObject obj;

        if (xDiff != oldxDiff || zDiff != oldzDiff)
        {
            roadBuildingPermission = false;
            road_time = Time.time;

            wrongPlaceCounter = 0;
            oldxDiff = xDiff;
            oldzDiff = zDiff;

            // Delete old tail
            for (i = roadTail.Count - 1; i >= 0; i--)
            {
                obj = roadTail[i];
                roadTail.Remove(obj);
                Destroy(obj);
            }

            // Create new tail
            if (Math.Abs(xDiff) > Math.Abs(zDiff))
            {
                if (xDiff > 0)
                    direction = 0;
                if (xDiff < 0)
                    direction = 1;
            }
            else
            {
                if (zDiff > 0)
                    direction = 2;
                if (zDiff < 0)
                    direction = 3;
                if (zDiff == 0)
                    direction = 4;
            }

            straightLength = Math.Max(Math.Abs(xDiff), Math.Abs(zDiff)) - Math.Min(Math.Abs(xDiff), Math.Abs(zDiff)) + 1;
            obliqueLength = Math.Min(Math.Abs(xDiff), Math.Abs(zDiff)) + 1;

            if (straightLength != 1 && obliqueLength != 1) settlingBuildManager = false;

            // Draw straight
            for (i = 1; i <= straightLength - 1; i++)
            {
                if (obliqueLength == 1 && i == straightLength - 1)
                {
                    break;
                }
                switch (direction)
                {
                    case 0:
                        pos.x += 2;
                        break;
                    case 1:
                        pos.x -= 2;
                        break;
                    case 2:
                        pos.z += 2;
                        break;
                    case 3:
                        pos.z -= 2;
                        break;
                }
                obj = Instantiate(buildModeObject, pos, Quaternion.identity) as GameObject;
                obj.GetComponent<BuildsetScript>().mode = 5;
                roadTail.Add(obj);
            }
            // Draw oblique
            for (i = 1; i <= obliqueLength - 1; i++)
            {
                switch (direction)
                {
                    case 0:
                    case 1:
                        if (zDiff > 0)
                            pos.z += 2;
                        else
                            pos.z -= 2;
                        break;
                    case 2:
                    case 3:
                        if (xDiff > 0)
                            pos.x += 2;
                        else
                            pos.x -= 2;
                        break;
                }
                obj = Instantiate(buildModeObject, pos, Quaternion.identity) as GameObject;
                obj.GetComponent<BuildsetScript>().mode = 5;
                roadTail.Add(obj);

                // Последний особый кусок oblique составляющей хвоста
                if (i != obliqueLength - 1)
                {
                    switch (direction)
                    {
                        case 0:
                            pos.x += 2;
                            break;
                        case 1:
                            pos.x -= 2;
                            break;
                        case 2:
                            pos.z += 2;
                            break;
                        case 3:
                            pos.z -= 2;
                            break;
                    }
                    obj = Instantiate(buildModeObject, pos, Quaternion.identity) as GameObject;
                    obj.GetComponent<BuildsetScript>().mode = 5;
                    roadTail.Add(obj);
                }
            }
        }
    }*/

    /*private void CommitEffect()
    {
        StartCoroutine(cameraShake.Shake(0.15f, 0.7f));
        Vector3 offset = new Vector3(0f, 1f, 0f);
        if (rotAngle == 0)
        {
            offset.x += gameData.get_size(buildMode)[0];
            offset.z += gameData.get_size(buildMode)[1];
        }
        if (rotAngle == 1)
        {
            offset.x += gameData.get_size(buildMode)[0];
            offset.z -= gameData.get_size(buildMode)[1];
        }
        if (rotAngle == 2)
        {
            offset.x -= gameData.get_size(buildMode)[0];
            offset.z -= gameData.get_size(buildMode)[1];
        }
        if (rotAngle == 3)
        {
            offset.x -= gameData.get_size(buildMode)[0];
            offset.z += gameData.get_size(buildMode)[1];
        }
        Quaternion rot = Quaternion.Euler(90f, 0f, 0f);
        GameObject effect = (GameObject)Instantiate(putHouseEffect, (hitPointBuild + offset), rot);
        effect.GetComponent<ParticleSystem>().collision.SetPlane(0, GameObject.Find("TerrainMain").GetComponent<TerrainCollider>().transform);
        Destroy(effect, 2f);
    }*/

    void Update()
    {
        /*if (sm.getBuildMode() && !rayIgnoresBuildings)                                     Этот участок для отключения коллайдеров построек во время строительства
        {
            GameObject[] colliders;
            colliders = GameObject.FindGameObjectsWithTag("ColliderModel");
            foreach (GameObject item in colliders)
            {
                item.layer = 2;
            }
            rayIgnoresBuildings = true;
        }
        if (!sm.getBuildMode() && rayIgnoresBuildings)
        {
            GameObject[] colliders;
            colliders = GameObject.FindGameObjectsWithTag("ColliderModel");
            foreach (GameObject item in colliders)
            {
                item.layer = 0;
            }
            rayIgnoresBuildings = false;
        }*/

        //hitPointBuild.y = 0.5f;

        //if (EventSystem.current.currentSelectedGameObject != null)
        //    Debug.Log(EventSystem.current.IsPointerOverGameObject.name);

        if (StateManager.GeneralState == GameState.BUILD)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastLength) && hit.collider.tag == "TerrainMesh")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    RotateBuildingClockwise(-1);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    RotateBuildingClockwise(1);
                }

                AdjustBuildModeObjectPosition();

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (StateManager.GodMode) InstantlyBuild(++BuildingProperties.maxUniqueIndex);
                    else TryToBuild();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    BuildModeActivate(); // 
                }
            }
        }

        /*if (Physics.Raycast(ray, out hit, raycastLength) && hit.collider.name == "TerrainMain")
        {
            if (Input.GetMouseButtonDown(1) && sm.getOrdinaryMode())
            {
                canvas.GetComponent<UIAuido>().PlaySmoothClick();
                GameObject clickTargetObj = Instantiate(clickOnGroundEffect, hit.point, Quaternion.identity) as GameObject;
                clickTargetObj.name = "MousePointerBox instantiated";
            }

            if (sm.getBuildMode() == false) wrongPlaceCounter = 0;

            if (sm.getBuildMode() == true)
            {
                hitPointBuild.x = (float)(Math.Floor((hit.point.x / 2) + 0.505) * 2);
                hitPointBuild.z = (float)(Math.Floor((hit.point.z / 2) + 0.505) * 2);
                hitPointBuild.x = Mathf.Clamp(hitPointBuild.x, cameraMove.BORDER, (turnManager.SIZE - cameraMove.BORDER) * 2);
                hitPointBuild.z = Mathf.Clamp(hitPointBuild.z, cameraMove.BORDER, (turnManager.SIZE - cameraMove.BORDER) * 2);
                objTransform.position = hitPointBuild;

                if (roadBuildMode && (roadBuildStage == 2))
                {
                    DrawTail();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    objTransform.Rotate(0f, 90f, 0f);
                    buildModeObject.GetComponent<BuildsetScript>().rotAngle = (buildModeObject.GetComponent<BuildsetScript>().rotAngle + 1) % 4;
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    objTransform.Rotate(0f, -90f, 0f);
                    if (buildModeObject.GetComponent<BuildsetScript>().rotAngle == 0)
                        buildModeObject.GetComponent<BuildsetScript>().rotAngle = 3;
                    else
                        buildModeObject.GetComponent<BuildsetScript>().rotAngle -= 1;
                }

                if (Input.GetMouseButtonDown(0) && !roadBuildMode && settlingBuildManager)
                {
                    if (wrongPlaceCounter == 0 && !mouseOverUI && turnManager.CheckResForBuild(buildMode))
                    {
                        buildTooltipTrigger.TurnOff();
                        wrongPlaceCounter = 0;
                        sm.OnOrdinaryMode();

                        if (buildMode == 6)
                            infoTrigger.TurnOffTF2Color();
                        buildModeObject.GetComponent<BuildsetScript>().RememberPlace();
                        buildModeObject.GetComponent<BuildsetScript>().MakeBuilding(hitPointBuild);
                        CommitEffect();
                        Destroy(buildModeObject);
                    }
                    else if (!turnManager.CheckResForBuild(buildMode))
                    {
                        canvas.GetComponent<UIAuido>().PlayCancelSound();
                        notification.TurnOn("res");
                    }
                    if (wrongPlaceCounter != 0)
                    {
                        canvas.GetComponent<UIAuido>().PlayCancelSound();
                        notification.TurnOn("place");
                    }
                }

                if (Input.GetMouseButtonDown(0) && roadBuildMode && settlingBuildManager)
                {
                    if (!roadBuildingPermission)
                    {
                        if (Time.time - road_time > roadbuildingTimeDelay) roadBuildingPermission = true;
                    }
                    if (wrongPlaceCounter <= 0 && roadBuildingPermission && !mouseOverUI)
                    {
                        if (roadBuildStage == 1)
                        {
                            roadBuildStage = 2;
                        }

                        oldPos = hitPointBuild;
                        roadTail.Add(buildModeObject);
                        for (int i = roadTail.Count - 1; i >= 0; i--)
                        {
                            GameObject item = roadTail[i];
                            item.GetComponent<BuildsetScript>().RememberPlace();
                            item.GetComponent<BuildsetScript>().MakeBuilding(item.transform.position); // Отсюда доделать MakeBuilding
                            roadTail.Remove(item);
                            Destroy(item);
                        }
                        wrongPlaceCounter = 0;
                        buildModeObject = Instantiate(buildModeRoad, hit.point, Quaternion.identity) as GameObject;
                        buildModeObject.GetComponent<BuildsetScript>().mode = buildMode;
                        objTransform = buildModeObject.transform;
                        CommitEffect();
                    }
                    else if (wrongPlaceCounter != 0)
                    {
                        canvas.GetComponent<UIAuido>().PlayCancelSound();
                        notification.TurnOn("place");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    if (buildMode == 6)
                        infoTrigger.TurnOffTF2Color();
                    if (roadBuildMode)
                    {
                        for (int i = roadTail.Count - 1; i >= 0; i--)
                        {
                            roadBuildMode = false;
                            GameObject obj = roadTail[i];
                            roadTail.Remove(obj);
                            Destroy(obj);
                        }
                    }
                    buildTooltipTrigger.TurnOff();
                    sm.OnOrdinaryMode();
                    canvas.GetComponent<UIAuido>().PlaySmoothClick();
                    Destroy(buildModeObject);
                }
            }
        }*/

        //Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.yellow);
    }

    void RotateBuildingClockwise(int _angle = 1)
    {
        activeBuilding.GridObject.angle.Turn(_angle);
        //Debug.Log("Now angle = " + objBuildSet.angle.Angle);
        buildingAngle.Set(activeBuilding.GridObject.angle.Index);
        activeBuildingTransform.localEulerAngles = new Vector3(0f, activeBuilding.GridObject.angle.Angle, 0f);
    }

    /*void RotateBuildingAnticlockwise(int _angle = )
    {
        objBuildSet.angle.Turn(-1);
        //Debug.Log("Now angle = " + objBuildSet.angle.Angle);
        buildingAngle.Set(objBuildSet.angle.Index);
        objTransform.localEulerAngles = new Vector3(0f, objBuildSet.angle.Angle, 0f);
    }*/

    Vector3 CalculateHitPointBuildUnrotated(RaycastHit hit)
    {
        Vector3 result;

        result.x = (float)(Math.Floor(hit.point.x + 0.5f) - 0.5f);
        result.z = (float)(Math.Floor(hit.point.z + 0.5f) - 0.5f);
        result.y = SCCoord.GetHeight(hit.point);

        hitPointBuild.x = Mathf.Clamp(hitPointBuild.x, CellGrid.xMin, CellGrid.xMax);
        hitPointBuild.z = Mathf.Clamp(hitPointBuild.z, CellGrid.zMin, CellGrid.zMax);

        return result;
    }

    Vector3 CalculateHitPointBuild(RaycastHit hit)
    {
        Vector3 result;

        SCCoord.GetRotationShift(activeBuilding.GridObject.angle.Index, out int xShift, out int zShift);
        result.x = (float)(Math.Floor(hit.point.x + 0.5f + xShift) - 0.5f);
        result.z = (float)(Math.Floor(hit.point.z + 0.5f + zShift) - 0.5f);
        result.y = SCCoord.GetHeight(hit.point);

        hitPointBuild.x = Mathf.Clamp(hitPointBuild.x, CellGrid.xMin, CellGrid.xMax);
        hitPointBuild.z = Mathf.Clamp(hitPointBuild.z, CellGrid.zMin, CellGrid.zMax);

        return result;

    }

    void AdjustBuildModeObjectPosition()
    {
        if (activeBuilding == null) return;

        hit.point = CellMesh.ReversePerturb(hit.point);
        activeBuildingTransform.position = CalculateHitPointBuild(hit);

        if (activeBuildingTransform.position.x != oldPos.x || activeBuildingTransform.position.z != oldPos.z)
        {
            activeBuilding.GridObject.coordinates = SCCoord.FromPos(CalculateHitPointBuildUnrotated(hit));
            activeBuilding.GridObject.RefreshCellPointers();
            RefreshModelPosition();
        }

        oldPos = activeBuildingTransform.position;
    }

    void RefreshModelPosition()
    {
        SCCoord.GetRotationShift(activeBuilding.GridObject.angle.Index, out int xShift, out int zShift);

        Vector3[] cornerVertices = new Vector3[4];
        int[] mainCoord = new int[2] { SCCoord.FromPos(activeBuildingTransform.position + CellMetrics.smallCellCenterShift).X - xShift, 
                                       SCCoord.FromPos(activeBuildingTransform.position + CellMetrics.smallCellCenterShift).Z - zShift };
        int[] size = activeBuilding.BldData.Size;
        size = SCCoord.RotateCellSize(size);

        Vector3[] rotShifts = SCCoord.GetRotationShifts(buildingAngle.Index);
        cornerVertices[0] = SCCoord.GetCornersRotated(buildingAngle.Index, new SCCoord(mainCoord[0] + (int)rotShifts[0].x * (size[0] - 1), mainCoord[1] + (int)rotShifts[0].z * (size[1] - 1)))[0];
        cornerVertices[1] = SCCoord.GetCornersRotated(buildingAngle.Index, new SCCoord(mainCoord[0] + (int)rotShifts[1].x * (size[0] - 1), mainCoord[1] + (int)rotShifts[1].z * (size[1] - 1)))[1];
        cornerVertices[2] = SCCoord.GetCornersRotated(buildingAngle.Index, new SCCoord(mainCoord[0] + (int)rotShifts[2].x * (size[0] - 1), mainCoord[1] + (int)rotShifts[2].z * (size[1] - 1)))[2];
        cornerVertices[3] = SCCoord.GetCornersRotated(buildingAngle.Index, new SCCoord(mainCoord[0] + (int)rotShifts[3].x * (size[0] - 1), mainCoord[1] + (int)rotShifts[3].z * (size[1] - 1)))[3];

        //Debug.Log(rotShifts[0]);
        //Debug.Log(rotShifts[1]);
        //Debug.Log(rotShifts[2]);
        //Debug.Log(rotShifts[3]);

        //Debug.Log("SIZE : " + size[0] + " " + size[1]);

        /*Instantiate(DataList.GetResourceObj(ResourceIndex.RAWVENISON), cornerVertices[0], Quaternion.identity);
        Instantiate(DataList.GetResourceObj(ResourceIndex.RAWVENISON), cornerVertices[1], Quaternion.identity);
        Instantiate(DataList.GetResourceObj(ResourceIndex.RAWVENISON), cornerVertices[2], Quaternion.identity);
        Instantiate(DataList.GetResourceObj(ResourceIndex.RAWVENISON), cornerVertices[3], Quaternion.identity);*/

        Vector3 avg = new Vector3();
        for (int i = 0; i < 4; i++)
        {
            avg += CellMesh.Perturb(cornerVertices[i]);
        }
        avg = avg / 4f;
        avg.y = activeBuilding.BuildSet.ModelPos.y;

        activeBuilding.BuildSet.ModelPos = avg;
    }

    bool TryToBuild()
    {
        BuildingProperties.constructionMode = ConstructionMode.ORD;                                                               // This variable must defined here, in GeneralBuilder
        if (activeBuilding.BuildSet.TryToBuild())
        {
            Connector.effectSoundManager.PlayConstructionSound();

            //Destroy(activeBuilding);
            activeBuildingTransform = null;
            activeBuilding = null;

            StateManager.SetOrdinaryMode();
            Connector.panelInvoker.buildingRotationTooltip.SetActive(false);

            return true;
        }

        return false;
    }

    bool InstantlyBuild(int uniqueIndex)
    {
        BuildingProperties.constructionMode = ConstructionMode.INSTBLD;
        if (activeBuilding.BuildSet.TryToBuild(uniqueIndex))
        {
            //Destroy(activeBuilding);
            activeBuilding = null;
            return true;
        }
        return false;
    }

    bool InstantlyConstruct(int uniqueIndex, int processValue)
    {
        BuildingProperties.constructionMode = ConstructionMode.INSTCONSTR;
        if (activeBuilding.BuildSet.TryToBuild(uniqueIndex, processValue))
        {
            //Destroy(activeBuilding);
            activeBuilding = null;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        foreach (Building item in VillageData.Buildings)
        {
            item.Die();
        }

        foreach (Building item in VillageData.Constructions)
        {
            item.Die();
        }
    }
}

[Serializable]
public class BuildingAngle
{
    int index = 0;

    public BuildingAngle(int _index)
    {
        Turn(_index);
    }

    public void Turn(int value)
    {
        index += value;
        if (index < 0) index = 4 * -index + index;
        if (index > 3) index = index % 4;
    }

    public void Set(int value)
    {
        if (value < 0) value = 0;
        else if (value > 3) value = 3;
        index = value;
    }

    public int Index
    {
        get => index;
    }

    public float Angle
    {
        get
        {
            return index * 90f;
        }
    }
}