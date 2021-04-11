using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Linq;
using System;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class Villager : MonoBehaviour
{
    public static GameObject selectionPlanePrefab, silhouettePrefab, silhouette;
    const float timeOfBuildingEntering = 1f, angleControlDelay = 0.18f, checkEventsDelay = 0.11f;
    const float defaultAttackDistance = 0.2f, attackDelay = 0.9f, constructionDistance = 1f;
    const float constructionDelay = 1f, defaultActionDistance = 0.2f, takingDelay = 0.0f;

    [Header("Settings")]
    public float delayTime;
    public float maxDistFromHome;

    [Header("Data")]
    public VillagerData data;

    [Header("Links")]
    public NavMeshAgent agent;
    public SmallVillagerInfo smallInfo;
    public Animator animator;
    public Inventory inventory;
    public Transform itemSpot;
    GameObject[] displayedItem;

    [Header("Work")]
    public int workInd;
    public ActionUnit[] actions;
    public ActionUnit currAction;
    public ActionType currentWorkAction;
    GameObject[] futureDestObj;
    GameObject[] destObj;
    public ExtractedResourceLink destExtractedResource;
    public int sequenceLink;
    int currentBehaviourPriority;
    bool deferredDefineBehaviour;
    int deferredBehaviourPriority;
    bool nullReaction = false;
    //bool refreshWorkActions = false;
    ResourceQuery resQuery;
    bool inaccessibleFood = false;

    [Header("Others")]
    public bool deletionFlag;
    public Building placeOfStay;
    public Building destinationBuilding;
    public VillagerState state;
    bool mouseOver;

    static float villagerHeight = 0.4f;
    static Vector3 villagerHeightVector = new Vector3(0f, villagerHeight / 2, 0f);
    Vector3 homeDirection, dest, oldPos;
    GameObject selectionPlane;
    Transform _transform;

    // -------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        data.villagerAgent = this;
        _transform = transform;
        //animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        DefineBehaviour();
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        mouseOver = true;
        selectionPlane = Instantiate(selectionPlanePrefab, _transform);
        selectionPlane.transform.position += new Vector3(0f, -0.2f, 0f);
        if (!StateManager.VillagerDragging)
            SetSmallInfo();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Connector.panelInvoker.OpenVillagerInfo(this);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (silhouette == null)
           silhouette = Instantiate(silhouettePrefab, _transform.position, Quaternion.identity);
        if (!StateManager.VillagerDragging)
        {
            StateManager.VillagerDragging = true;
            SmallInfoController.SetAllBuilding();
        }
    }

    private void OnMouseUp()
    {
        if (data.TryToSettleByDragging())
            Connector.effectSoundManager.PlaySmoothClick();
        else
            Connector.effectSoundManager.PlayCancelSound();
        StateManager.VillagerDragging = false;
        SmallInfoController.SetAllBuilding();
        SmallInfoController.SetSmallInfoByMouse();
        Destroy(silhouette);
        SetSmallInfo();
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        if (selectionPlane != null) Destroy(selectionPlane);
        if (!StateManager.VillagerDragging)
            SetSmallInfo();
    }

    // -------------------------------------------------------------------------------------------------- //

    public void SetFutureDestObj(GameObject obj)
    {
        futureDestObj = new GameObject[1] { obj };
    }

    public void SetFutureDestObj(GameObject[] obj)
    {
        futureDestObj = obj;
    }

    //public void DefineInventory(int packSize = 1, int packsAmount = 2)
    //{
    //    inventory.Init(this, packSize, packsAmount);
    //}

    public void DefineBehaviour(int priority = 0)                                    
    {
        //Debug.Log("DefineBehaviour: actions =? null : " + actions == null);
        //Debug.Log("DefineBehaviour: actions =? null : " + actions == null + " workInd = " + workInd + " actions[workInd] : " + actions[workInd].type.ToString());
        if (actions != null && workInd < actions.Length && priority < currentBehaviourPriority/*actions[workInd].priority*/)
        {
            //Debug.Log("DefineBehaviour: deferred");
            deferredDefineBehaviour = true;
            deferredBehaviourPriority = priority;
            return;
        }

        FreeResources();
        ClearTargets();
        StopAllCoroutines();
        currentWorkAction = ActionType.NONE;

        if (Sunlight.theEndOfDay)
        {
            if (data.home != null)
                GoSleep();
            else
                StartCoroutine(RandomWalk());
        }
        else if (data.home != null && data.satiety < 2.0f && !inaccessibleFood)
        {
            Eat();
            StartCoroutine(AngleControl());
        }
        else if (placeOfStay != null)
        {
            ExitBuilding();
        }
        else if (data.profession != Profession.NONE)
        {
            Act();                 
            StartCoroutine(AngleControl());
        }
        else
        {
            StartCoroutine(RandomWalk());
        }
    }

    public void SetDestinationBuilding(Building building)
    {
        if (!agent.enabled) // КОСТЫЛЬ
        {
            if (placeOfStay == null)
                agent.enabled = true;
        }
        destinationBuilding = building;
        SetDestination(building.PeopleContainer.enter[0].transform.position);
        state = VillagerState.GOTOHOUSE;

        // Мгновенное исполнение функции EnterBuilding без участия BuildingEnterTrigger
        if ((_transform.position - building.PeopleContainer.enter[0].transform.position).sqrMagnitude < 0.25f) EnterBuilding(building);
    }

    public void EnterBuilding(Building building)
    {
        StopCoroutine(EndBuildingEntering(timeOfBuildingEntering));

        agent.enabled = false;
        currentBehaviourPriority = 80;
        LeanTween.move(gameObject, building.GridObject.GetCenter() + villagerHeightVector, timeOfBuildingEntering);
        StartCoroutine(EndBuildingEntering(timeOfBuildingEntering));
        state = VillagerState.ENTERING;
    }

    public void ExitBuilding()
    {
        if (placeOfStay == null) return;
        StopCoroutine(EndBuildingExiting(timeOfBuildingEntering));

        currentBehaviourPriority = 80;
        Vector3 exitTargetPosition = placeOfStay.PeopleContainer.enter[0].transform.position + villagerHeightVector + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0f, UnityEngine.Random.Range(-0.1f, 0.1f));
        LeanTween.move(gameObject, exitTargetPosition, timeOfBuildingEntering);
        StartCoroutine(EndBuildingExiting(timeOfBuildingEntering));
        state = VillagerState.EXITING;
    }

    public void TakeItem(Item item)
    {
        item.Inventory.Give(inventory, 0);
        RefreshDisplayedItem();
    }

    public void SetSmallInfo()
    {
        if (smallInfo == null) return;

        if (mouseOver || (data.home == null))
        {
            smallInfo.gameObject.SetActive(true);
            smallInfo.infoBox.SetActive(mouseOver);
            smallInfo.iconBox.SetActive(data.home == null);
            smallInfo.Refresh();
        }
        else
        {
            smallInfo.gameObject.SetActive(false);
        }
    }

    public void MorningFlagRefresh()
    {
        inaccessibleFood = false;
    }

    // -------------------------------------------------------------------------------------------------- //

    void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
        
        //DefineAngle(dest);
    }

    void GoSleep()
    {
        workInd = 0;
        SetDestinationBuilding(data.home);
        StartCoroutine(AngleControl());
    }

    void Attack(Creature target)                                                               // Make Attack script
    {
        target.Health.GetDamage(gameObject, data.defaultDamage);
        animator.Play("Armature|Hit");
        //animator.SetBool("Hit", true);
        //StopCoroutine();
    }

    //IEnumerator Attack

    void Build(Building construction)
    {
        construction.BuildSet.Process += 1f;
    }

    float SqrDistToHome()
    {
        Vector3 diff = (data.home.GridObject.GetCenter()) - _transform.position;
        homeDirection = diff;
        return diff.sqrMagnitude;
    }

    void DefineAngle(Vector3 dest)
    {
        //Debug.Log("DefineAngle(Vector3 dest) of dest = " + dest);
        float angle = 360f * Mathf.Atan(dest.x / dest.z) / (2 * Mathf.PI);
        if (!double.IsNaN(angle))
        {
            if (dest.z < 0)
            {
                angle += 180f;
            }

            //Debug.Log("DefineAngle.angle = " + angle);
            //_transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            LeanTween.rotateLocal(gameObject, new Vector3(0f, angle, 0f), angleControlDelay);
        }
    }

    void RefreshDisplayedItem()
    {
        float shift = 0f;

        if (displayedItem != null)
        {
            foreach (GameObject item in displayedItem)  // Возможно это неэффективно всегда удалять отображаемые предметы
            {
                if (item != null) Destroy(item);
            }
        }

        displayedItem = new GameObject[inventory.PacksAmount];
        for (int i = 0; i < inventory.PacksAmount; i++)
        {
            //inventory.Look(i, out ResourceIndex index, out float value);
            if (inventory.StoredRes[i] != ResourceIndex.NONE)
            {
                displayedItem[i] = Instantiate(DataList.GetResourceModel(inventory.StoredRes[i]), itemSpot);
                displayedItem[i].transform.localPosition = new Vector3(0f, shift, 0f);
                displayedItem[i].transform.localScale = new Vector3(1f, 1f, 1f);
                Destroy(displayedItem[i].GetComponent<Item>());
                Destroy(displayedItem[i].GetComponent<BoxCollider>());
                shift += 0.05f;
            }
        }
    }

    void FreeResources()
    {
        if (destObj != null)
        {
            for (int i = 0; i < destObj.Length; i++)
            {
                if (destObj[i].tag == "Resource")
                {
                    destObj[i].GetComponent<Item>().Inventory.RemoveOccupation(this);
                }
            }
        }

        if (destExtractedResource != null)
        {
            destExtractedResource.deposit.RemoveOccupation(destExtractedResource.ind, this);
        }
    }

    void ClearTargets()
    {
        actions = null;
        currAction = null;
        futureDestObj = null;
        destObj = null;
        destExtractedResource = null;
        deferredDefineBehaviour = false;
        deferredBehaviourPriority = 0;
        destinationBuilding = null;
        resQuery = null;
    }

    
    void Act()
    {
        if (actions == null || actions.Length == 0)
        {
            actions = data.workSequence.actions;
            workInd = 0;
        }

        if (workInd > actions.Length - 1)
        {
            DefineBehaviour();
            return;
        }

        ActionUnit action = actions[workInd];
        currentWorkAction = action.type;

        if (deferredDefineBehaviour && action.priority <= deferredBehaviourPriority)
        {
            currentWorkAction = 0;
            deferredDefineBehaviour = false;
            DefineBehaviour(deferredBehaviourPriority);
            return;
        }

        //agent.enabled = true;
        currAction = action; // In future delete all parameters "action" in action coroutines

        //if (placeOfStay == null) Debug.Log("=3= placeOfStay == null !!!!!!!!");

        switch (action.type)
        {
            case ActionType.NONE:
                StartCoroutine(RandomWalk());
                break;
            case ActionType.GOTOWORK:
                StartCoroutine(GoToWork(action));
                break;
            case ActionType.TAKE:
                StartCoroutine(TakeItem(action));
                break;
            case ActionType.EXITHOUSE:
                StartCoroutine(ExitHouse(action));
                break;
            case ActionType.FIND:
                StartCoroutine(FindPrefab(action));
                break;
            case ActionType.KILLANIMAL:
                StartCoroutine(KillAnimal(action));
                break;
            case ActionType.FINDLABORERWORK:
                FindLaborerWork(action);
                break;
            case ActionType.BUILD:
                StartCoroutine(BuildConstruction(action));
                break;
            case ActionType.GOTOWARE:
                StartCoroutine(GoToWarehouse(action));
                break;
            case ActionType.FREEINV:
                StartCoroutine(FreeInventory(action));
                break;
            case ActionType.EXTRACT:
                StartCoroutine(ExtractResource(action));
                break;
            case ActionType.EAT:
                StartCoroutine(EatFood(action));
                break;
            case ActionType.GOTOPOINT:
                StartCoroutine(GoToPoint(action));
                break;
            case ActionType.CHECK:
                StartCoroutine(CheckCondition(action));
                break;
        }
    }

    void Eat()
    {
        resQuery = new ResourceQuery(ResourceType.FOOD, VillageData.foodServing);
        actions = ActSequenceList.GetGeneralSequence(GeneralActSequenceIndex.EAT).actions;
        inaccessibleFood = true;
        Act();
    }

    void ChangeSequenceIndexAfterFinish(ActionUnit action)
    {
        switch (action.reaction[0])
        {
            case SequenceAction.NONE:
                workInd = 0;
                break;
            case SequenceAction.NEXT:
                workInd++;
                break;
            case SequenceAction.PREV:
                workInd--;
                break;
            case SequenceAction.GOTO:
                workInd = action.actVar[0];
                break;
            case SequenceAction.ANEW:
                break;
            case SequenceAction.SHIFT:
                workInd += action.actVar[0];
                break;
        }
    }

    void ChangeSequenceIndexAfterNull(ActionUnit action)
    {
        SequenceAction reaction;
        if (action.reaction.Length >= 2)
            reaction = action.reaction[1];
        else
            reaction = action.reaction[0];

        switch (reaction)
        {
            case SequenceAction.NONE:
                workInd = 0;
                break;
            case SequenceAction.NEXT:
                workInd++;
                break;
            case SequenceAction.PREV:
                workInd--;
                break;
            case SequenceAction.GOTO:
                workInd = action.actVar[1];
                break;
            case SequenceAction.ANEW:
                break;
            case SequenceAction.SHIFT:
                workInd += action.actVar[1];
                break;
        }
    }
    

    // -------------------------------------------------------------------------------------------------- //

    IEnumerator AngleControl()
    {
        oldPos = _transform.position;
        Vector3 dest;

        while (true)
        {
            yield return new WaitForSeconds(angleControlDelay);
            //Debug.Log("AngleControl() iteration");
            dest = _transform.position - oldPos;
            //Debug.Log("dest.sqrMagnitude = " + dest.sqrMagnitude);
            if (dest.sqrMagnitude > 0.001f) DefineAngle(dest);
            oldPos = _transform.position;
            //animator.SetFloat("Velocity", dest.sqrMagnitude);
        }
    }

    Vector3 oldOldPos;
    Vector3 destDest;

    private void Update()
    {
        destDest = _transform.position - oldOldPos;
        animator.SetFloat("Velocity", destDest.sqrMagnitude * 10000);
        oldOldPos = _transform.position;
    }



    IEnumerator RandomWalk()
    {
        StopCoroutine(AngleControl());
        StartCoroutine(AngleControl());
        state = VillagerState.RNDWALK;

        ClearTargets();

        while (true)
        {
            delayTime = UnityEngine.Random.Range(1f, 5f);
            yield return new WaitForSeconds(delayTime);

            dest = _transform.position;
            if (data.home != null && SqrDistToHome() > maxDistFromHome)
            {
                dest += homeDirection;
                SetDestination(dest);
            }
            else
            {
                dest += new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
                dest.y = SCCoord.GetHeight(dest);
                SetDestination(dest);
            }
        }
    }

    IEnumerator EndBuildingEntering(float delay)
    {
        //Debug.Log("Start of end building entering countdown");
        yield return new WaitForSeconds(delay);
        placeOfStay = destinationBuilding;
        if (BuildingInfo.activeBuilding == placeOfStay) Connector.panelInvoker.RefreshBuildingInfo();
        destinationBuilding = null;
        if (actions != null) currentBehaviourPriority = actions[workInd].priority;
        else currentBehaviourPriority = 0;
        state = VillagerState.INDOORS;
        //Debug.Log("End of end building entering countdown");
    }

    IEnumerator EndBuildingExiting(float delay)
    {
        //Debug.Log("Start of end building exiting countdown");
        yield return new WaitForSeconds(delay);
        agent.enabled = true;
        Building oldPlaceOfStay = placeOfStay;
        //Debug.Log("Nullify place of stay .....");
        placeOfStay = null;
        if (BuildingInfo.activeBuilding == oldPlaceOfStay) Connector.panelInvoker.RefreshBuildingInfo();
        state = VillagerState.RNDWALK;
        currentBehaviourPriority = 0;
        //Debug.Log("End of end building exiting countdown");
        if (currentWorkAction != ActionType.EXITHOUSE) DefineBehaviour();
    }

    
    IEnumerator GoToWork(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOHOUSE);

        // ========== START ALGORITHM ========== //
        SetDestinationBuilding(data.work);
        destObj = new GameObject[1] { data.work.gameObject };

        while (true)
        {
            if (placeOfStay != null && placeOfStay.BldProp.UniqueIndex == destObj[0].GetComponent<Building>().BldProp.UniqueIndex)
            {
                if (placeOfStay == data.work) break;
                else ExitBuilding();
            }

            if (destObj[0] == null)
            {
                nullReaction = true;
                break;
            }

            yield return new WaitForSeconds(checkEventsDelay);
        }
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator TakeItem(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.TAKINGITEM);

        //if (placeOfStay == null) Debug.Log("=5= placeOfStay == null !!!!!!!!");

        // ========== START ALGORITHM ========== //
        if (destObj == null && resQuery == null)
            nullReaction = true;
        else if (destObj != null)
        {
            Item resScript;
            bool occupiedAtLeastOneItem = false;

            for (int i = 0; i < destObj.Length; i++)
            {
                if (placeOfStay == null && (resScript = destObj[i].GetComponent<Item>()) != null && resScript.Inventory.Occupy(this))
                {
                    Vector2 selfPos, objPos = new Vector2(destObj[i].transform.position.x, destObj[i].transform.position.z);
                    SetDestination(new Vector3(objPos.x, SCCoord.GetHeight(objPos), objPos.y));

                    occupiedAtLeastOneItem = true;
                    while (true)
                    {
                        selfPos = new Vector2(_transform.position.x, _transform.position.z);

                        if (destObj[i] == null) 
                            break;
                        else if (Vector2.SqrMagnitude(selfPos - objPos) < defaultActionDistance)
                        {
                            TakeItem(resScript);
                            FreeResources();
                            break;
                        }

                        yield return new WaitForSeconds(checkEventsDelay);
                    }
                }
            }
            if (!occupiedAtLeastOneItem)
                nullReaction = true;
        }
        else if (resQuery != null && placeOfStay != null && placeOfStay.Inventory != null)
        {
            float remainder;

            for (int i = 0; i < ((resQuery.index != null) ? resQuery.index.Length : 0); i++)
            {
                remainder = placeOfStay.Inventory.Give(inventory, resQuery.index[i], resQuery.indexVal[i]);
                resQuery.indexVal[i] = remainder;                                                                        // Add Properties in resQuery
            }

            for (int i = 0; i < ((resQuery.type != null) ? resQuery.type.Length : 0); i++)
            {
                for (int k = 0; k < DataList.GetResourceIndices(resQuery.type[i]).Length; k++)
                {
                    ResourceIndex resInd = DataList.GetResourceIndices(resQuery.type[i])[k];
                    remainder = placeOfStay.Inventory.Give(inventory, resInd, resQuery.typeVal[i]);                      // maybe do fuction Give with resource type
                    resQuery.typeVal[i] = remainder;
                    if (remainder < 0.001f) break;
                }
            }
        }
        RefreshDisplayedItem();
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator ExitHouse(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.EXITING);

        // ========== START ALGORITHM ========== //
        yield return StartCoroutine(ExitHouseAlgorithm(action));
        //Debug.Log("This message must show itself after 1f delay");
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator FindPrefab(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.FINDING);

        // ========== START ALGORITHM ========== //
        List<GameObject> objects = new List<GameObject>();
        float minSqrDist = float.MaxValue, dist;
        Vector2 selfPos = new Vector2(transform.position.x, transform.position.z);

        if (action.searchTarget[0] is ResourceData)
        {
            if (ItemManager.items.Count > 0)
            {
                ResourceIndex[] indexArray = (from ind in action.searchTarget select ((ResourceData)ind).Index).ToArray();

                foreach (Item item in ItemManager.items)
                {
                    if (Array.Exists(indexArray, i => i == item.Inventory.StoredRes[0]))
                    {
                        if (action.mode == ActionMode.WIDE)
                        {
                            objects.Add(item.gameObject);
                        }
                        else if ((dist = Vector2.SqrMagnitude(selfPos - new Vector2(item.transform.position.x, item.transform.position.z))) < minSqrDist)
                        {
                            minSqrDist = dist;
                            objects = new List<GameObject>() { item.gameObject };
                        }
                    }
                }
            }
            yield return new WaitForSeconds(checkEventsDelay);
        }
        else if (action.searchTarget[0] is CreatureData)
        {
            if (CreatureManager.animalPopulation > 0)
            {
                foreach (Creature item in CreatureManager.Creatures)
                {
                    if ((dist = Vector2.SqrMagnitude(selfPos - new Vector2(item.transform.position.x, item.transform.position.z))) < minSqrDist)
                    {
                        minSqrDist = dist;
                        objects = new List<GameObject>() { item.gameObject };
                    }
                }
            }
            yield return new WaitForSeconds(checkEventsDelay);
        }
        else if (action.searchTarget[0] is BuildingData)
        {
            BuildingIndex bldIndex = ((BuildingData)action.searchTarget[0]).Index;

            foreach (Building item in VillageData.Buildings)
            {
                if (item.BldData.Index == bldIndex &&
                    (dist = Vector2.SqrMagnitude(selfPos - new Vector2(item.transform.position.x, item.transform.position.z))) < minSqrDist)
                {
                    minSqrDist = dist;
                    objects = new List<GameObject>() { item.gameObject };
                }
            }
        }

        objects.Sort(delegate (GameObject a, GameObject b)
        {
            float aDist = Vector2.SqrMagnitude(selfPos - new Vector2(a.transform.position.x, a.transform.position.z));
            float bDist = Vector2.SqrMagnitude(selfPos - new Vector2(b.transform.position.x, b.transform.position.z));
            return aDist.CompareTo(bDist);
        });

        if (objects.Count == 0)
            nullReaction = true;
        else
            futureDestObj = objects.ToArray();
        // ========== END ALGORITHM ========== //

        //StartCoroutine(KillAnimal(action));
        //WaitUntil()

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator KillAnimal(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOKILL);

        // ========== START ALGORITHM ========== //
        Creature animal = destObj[0].GetComponent<Creature>();
        Vector2 selfPos, objPos;

        while (true)
        {
            if (animal.Health.Value <= 0)
            {
                nullReaction = !(animal.Health.lastAttacker == gameObject);
                break;
            }

            selfPos = new Vector2(transform.position.x, transform.position.z);
            objPos = new Vector2(animal.transform.position.x, animal.transform.position.z);

            if (Vector2.SqrMagnitude(selfPos - objPos) > defaultAttackDistance)
            {
                SetDestination(new Vector3 (objPos.x, SCCoord.GetHeight(objPos), objPos.y)); 
            }
            else
            {
                Attack(animal);
                yield return new WaitForSeconds(attackDelay);
            }

            yield return new WaitForSeconds(checkEventsDelay);
        }
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    public void FindLaborerWork(ActionUnit action)
    {
        int count;

        state = VillagerState.FINDING;

        if ((count = VillageData.Constructions.Count) > 0)
        {
            for (int i = 0; i < count; i++)
            {
                futureDestObj = new GameObject[1] { VillageData.Constructions[i].gameObject };
                //Debug.Log("--- 1 --- = " + ((futureDestObj[0] == null) ? "NULL" : "OK"));
                workInd = 1; // Building
                Act();
                return;
            }
        }

        if ((count = VillageData.extractionQueue.Count) > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (VillageData.extractionQueue[i].deposit.Occupy(VillageData.extractionQueue[i].ind, this)) 
                {
                    destExtractedResource = VillageData.extractionQueue[i];
                    //Debug.Log("DEST RESOURCE DEPOSIT WAS ASSIGNED");
                    futureDestObj = new GameObject[1] { destExtractedResource.deposit.gameObject };
                    workInd = 2; // Extracting
                    Act();
                    return;
                }
            }
        }

        StartCoroutine(RandomWalk());
    }

    IEnumerator BuildConstruction(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOBUILD);

        // ========== START ALGORITHM ========== //
        //Debug.Log("--- 1 --- = " + ((destObj == null) ? "NULL" : "OK"));
        Building construction = destObj[0].GetComponent<Building>();
        Vector2 selfPos, objPos = new Vector2(construction.GridObject.GetCenter().x, construction.GridObject.GetCenter().z);

        SetDestination(new Vector3(objPos.x, SCCoord.GetHeight(objPos), objPos.y));

        while (true)
        {
            if (placeOfStay != null || construction == null || construction.BuildSet.Process >= construction.BldData.ConstrCost)
            {
                yield return new WaitForSeconds(checkEventsDelay);
                break;
            }

            selfPos = new Vector2(transform.position.x, transform.position.z);

            if (Vector2.SqrMagnitude(selfPos - objPos) > constructionDistance)
            {
                //SetDestination(new Vector3(objPos.x, SCCoord.GetHeight(objPos), objPos.y));
            }
            else
            {
                Build(construction);
                yield return new WaitForSeconds(constructionDelay);
            }

            yield return new WaitForSeconds(checkEventsDelay);
        }
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }
    
    IEnumerator GoToWarehouse(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOHOUSE);

        // ========== START ALGORITHM ========== //
        float minSqrDist = float.MaxValue, dist;
        Vector2 selfPos = new Vector2(_transform.position.x, _transform.position.z);
        Building warehouse = null;

        foreach (Building item in VillageData.Buildings)
        {
            if (item.BuildSet.ConstrStatus == ConstructionStatus.READY && item.BldData.BldType == BuildingType.WAREHOUSE)
            {
                if (action.mode == ActionMode.PUT)
                {
                    bool absenceOfFreeSpace = true;
                    for (int i = 0; i < inventory.PacksAmount; i++)
                    {
                        if (item.Inventory.CheckPlaceFor(inventory.StoredRes[i]))
                        {
                            absenceOfFreeSpace = false;
                            break;
                        }
                    }
                    if (absenceOfFreeSpace) continue;
                }

                if (action.mode == ActionMode.TAKE)
                {
                    bool absenceOfNecessaryResource = true;
                    if (resQuery.index != null)
                    {
                        for (int i = 0; i < resQuery.index.Length; i++)
                        {
                            if (inventory.SearchResource(resQuery.index[i]).Length > 0)
                            {
                                absenceOfNecessaryResource = false;
                                break;
                            }
                        }
                    }
                    if (absenceOfNecessaryResource && resQuery.type != null)
                    {
                        for (int i = 0; i < resQuery.type.Length; i++)
                        {
                            ResourceType type = resQuery.type[i];

                            foreach (ResourceIndex ind in DataList.GetResourceIndices(type))
                            {
                                if (item.Inventory.SearchResource(ind).Length > 0)
                                {
                                    absenceOfNecessaryResource = false;
                                    break;
                                }
                            }

                            if (!absenceOfNecessaryResource) break;
                        }
                    }
                    if (absenceOfNecessaryResource) { Debug.Log("absenceOfNecessaryResource = " + absenceOfNecessaryResource + ". continue..."); continue; }
                }

                if ((dist = Vector2.SqrMagnitude(selfPos - new Vector2(item.transform.position.x, item.transform.position.z))) < minSqrDist)
                {
                    minSqrDist = dist;
                    warehouse = item;
                }
            }
        }

        if (warehouse != null)
        {
            SetDestinationBuilding(warehouse);

            while (true)
            {
                if (warehouse == null)
                {
                    //Debug.Log("GoToWarehouse warehouse == null  !!!");
                    nullReaction = true;
                    break;
                }
                if (placeOfStay != null && placeOfStay.BldProp.UniqueIndex == warehouse.BldProp.UniqueIndex) break;
                yield return new WaitForSeconds(checkEventsDelay);
            }
        }
        else
        {
            Debug.Log("WAREHOUSE NULL REACTION");
            nullReaction = true;
        }
        // ========== END ALGORITHM ========== //

        //if (placeOfStay == null) Debug.Log("=1= placeOfStay == null !!!!!!!!");

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        //if (placeOfStay == null) Debug.Log("=2= placeOfStay == null !!!!!!!!");
        Act();
    }

    IEnumerator FreeInventory(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.PUTTINGITEM);

        // ========== START ALGORITHM ========== //
        if (placeOfStay != null && placeOfStay.Inventory != null && placeOfStay.Inventory.enabled)
        {
            for (int i = 0; i < inventory.PacksAmount; i++)
            {
                inventory.Give(placeOfStay.Inventory, i);
            }

            RefreshDisplayedItem();
        }
        else
        {
            for (int i = 0; i < inventory.PacksAmount; i++)
            {
                inventory.LayOut(i);
            }
        }
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator ExtractResource(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOEXTRACT);

        // ========== START ALGORITHM ========== //
        //if (destResourceDeposit == null) Debug.Log(" NULL !!! ");
        if (destExtractedResource.deposit.Occupy(destExtractedResource.ind, this))
        {
            Nature nature = destObj[0].GetComponent<Nature>();
            Vector2 selfPos, objPos = new Vector2(nature.ResourceDeposit.ExtractPoint.position.x, nature.ResourceDeposit.ExtractPoint.position.z);

            while (true)
            {
                if (nature == null || nature.ResourceDeposit.Amount(destExtractedResource.ind) == 0f)
                {
                    nullReaction = true;
                    break;
                }

                selfPos = new Vector2(transform.position.x, transform.position.z);

                if (Vector2.SqrMagnitude(selfPos - objPos) > defaultActionDistance)
                {
                    SetDestination(new Vector3(objPos.x, SCCoord.GetHeight(objPos), objPos.y));
                }
                else
                {
                    state = VillagerState.EXTRACTING;
                    yield return new WaitForSeconds(1.0f * nature.NtrData.ExtractSpeed(destExtractedResource.ind));
                    nature.ResourceDeposit.Extract(destExtractedResource.ind, 1.0f);
                    break;
                }

                yield return new WaitForSeconds(checkEventsDelay);
            }
        }
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator EatFood(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.EATING);

        // ========== START ALGORITHM ========== //
        EatFoodAlgorithm(action);
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator CheckCondition(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.FINDING);

        // ========== START ALGORITHM ========== //
        switch (action.mode)
        {
            case ActionMode.INVISEMPTY:
                nullReaction = !inventory.IsEmpty();
                break;
        }

        yield return new WaitForSeconds(checkEventsDelay);
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }

    IEnumerator GoToPoint(ActionUnit action)
    {
        StartWorkActionHandler(VillagerState.GOTOPOINT);

        // ========== START ALGORITHM ========== //
        if (destObj[0].GetComponent<Building>()?.BldData.Index == BuildingIndex.BONFIRE) // КОСТЫЛЬ!!!!!
        {
            Vector3 randPos = destObj[0].GetComponent<Building>().GridObject.GetCenter();
            float randAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            randPos += new Vector3(Mathf.Cos(randAngle), 0f, Mathf.Sin(randAngle)) * 0.4f;
            yield return StartCoroutine(GoToPointAlgorithm(action, randPos));
        }
        else yield return StartCoroutine(GoToPointAlgorithm(action, destObj[0].transform.position));
        // ========== END ALGORITHM ========== //

        yield return new WaitForSeconds(action.afterDelay);
        ChangeSequenceIndex(action);
        EndWorkActionHandler();
        Act();
    }


    IEnumerator ExitHouseAlgorithm(ActionUnit action)
    {
        ExitBuilding();

        while (true)
        {
            if (placeOfStay == null) break;
            yield return new WaitForSeconds(checkEventsDelay);
        }
    }

    void EatFoodAlgorithm(ActionUnit action)
    {
        inventory.Look(0, out ResourceIndex ind1, out float val1);       ///////// INCONVENIENT
        inventory.Look(1, out ResourceIndex ind2, out float val2);
        inventory.ClearInventory();
        RefreshDisplayedItem();
        data.satiety += val1 + val2;
        if (data.satiety <= 0.0f)
        {
            Die();
            if (VillageData.Population <= 0)
            {
                SaveLoader.mapIsLoaded = false;
                Connector.panelInvoker.pauseMenu.Open(PauseMenuSection.Load);
            }
            return;
        }
        data.satiety = Mathf.Clamp(data.satiety, 0.0f, 2.0f);
        if (VillagerInfo.activeVillager == this) Connector.panelInvoker.RefreshVillagerInfo();
        inaccessibleFood = false;
    }

    IEnumerator GoToPointAlgorithm(ActionUnit action, Vector3 targetPos)
    {

        SetDestination(targetPos);
        Vector2 selfPos, objPos = new Vector2(targetPos.x, targetPos.z);

        while (true)
        {
            selfPos = new Vector2(transform.position.x, transform.position.z);

            if (Vector2.SqrMagnitude(selfPos - objPos) <= defaultActionDistance) break;

            yield return new WaitForSeconds(checkEventsDelay);
        }
    }


    void StartWorkActionHandler(VillagerState _state)
    {
        nullReaction = false;
        destObj = futureDestObj;
        futureDestObj = null;
        currentBehaviourPriority = actions[workInd].priority;
        state = _state;
    }

    void ChangeSequenceIndex(ActionUnit action)
    {
        if (nullReaction) ChangeSequenceIndexAfterNull(action);
        else ChangeSequenceIndexAfterFinish(action);
    }

    void EndWorkActionHandler()
    {
        nullReaction = false;
        destObj = null;
    }


    public void Die()
    {
        StopAllCoroutines();

        //if (data.work != null) data.Dismiss();
        if (data.home != null) data.Evict();

        deletionFlag = true;
        VillageData.RemoveVillager(data);
        VillageData.homeless--;
        smallInfo.Delete();
        if (VillagerInfo.activeVillager == this) Connector.panelInvoker.CloseVillagerInfo();
        VillageData.deletedVillagersQueue.Add(this);

        agent.enabled = false;
        transform.position = CellMetrics.hidedObjects;
        gameObject.SetActive(false);

        InfoDisplay.Refresh();
    }

    public void SelfDeletion() { Destroy(gameObject); }
}

public class WorkActionFlags
{
    bool _null;

}

public enum VillagerState
{
    NONE,
    RNDWALK,
    GOTOHOUSE,
    ENTERING,
    INDOORS,
    EXITING,
    FINDING,
    GOTOKILL,
    TAKINGITEM,
    GOTOBUILD,
    PUTTINGITEM,
    GOTOEXTRACT,
    EXTRACTING,
    EATING,
    GOTOPOINT
}
