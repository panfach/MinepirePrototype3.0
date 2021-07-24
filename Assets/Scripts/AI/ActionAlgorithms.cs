using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using ActSequenceSystem;

public static class ActionAlgorithms
{
    static float tickTime = 0.05f;
    static float longTickTime = 0.1f;
    static WaitForSeconds tick = new WaitForSeconds(tickTime);
    static WaitForSeconds longTick = new WaitForSeconds(longTickTime);

    /// <summary>
    /// Each act sequence graph must contain StartAction node in the beginning
    /// </summary>
    public static IEnumerator StartAction(Creature creature, ActSequenceSystem.StartAction action)
    {
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator IsEndOfDay(Creature creature, ActSequenceSystem.IsEndOfDay action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        if (Sunlight.theEndOfDay)
            TrueReaction(creature, action);
        else
            FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator SwitchSequence(Creature creature, ActSequenceSystem.SwitchSequence action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        creature.GeneralAI.SwitchCurrentSequence(action.Sequence);
        //TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator EnterBuilding(Creature creature, ActSequenceSystem.EnterBuilding action)
    {
        // Checking the possibility of execution
        if (action.Building == null || action.Building.CreatureContainer == null)
        {
            Debug.Log("FALSE 1");
            FalseReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        if (action.Building.CreatureContainer == null || !action.Building.CreatureContainer.Add(creature))
        {
            Debug.Log("FALSE 2");
            FalseReaction(creature, action);
            yield break;
        }
        creature.Agent.enabled = false;
        //creature.GeneralAI.StopAngleControl();
        Debug.Log("Changing enter angle");
        LeanTween.rotateY(creature.gameObject, GeneralAI.GetViewAngle(action.Building.GridObject.GetCenter() - creature.transform.position), longTickTime);
        yield return longTick;
        //        creature.gameObject.transform.rotation = Quaternion.Euler(0f, GeneralAI.GetViewAngle(action.Building.GridObject.GetCenter() - creature.transform.position), 0f);
        LeanTween.move(creature.gameObject, action.Building.GridObject.GetCenter() + creature.CrtProp.HeightVector, creature.CrtData.timeOfBuildingEntering);
        yield return new WaitForSeconds(creature.CrtData.timeOfBuildingEntering);

        if (action.Building == null)
        {
            creature.Agent.enabled = true;
            creature.CrtProp.PlaceOfStay = null;
            Debug.Log("FALSE 3");
            FalseReaction(creature, action);
            yield break;
        }

        // Final delay
        yield return new WaitForSeconds(action.finalDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator RandomWalk(Creature creature, ActSequenceSystem.RandomWalk action)
    {
        // Body
        Vector3 dest;
        while (true)
        {
            float delayTime = Random.Range(1f, creature.CrtData.maxRandomWalkDelay);
            yield return new WaitForSeconds(delayTime);

            dest = creature.transform.position;
            dest += new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
            dest.y = SCCoord.GetHeight(dest);
            creature.Agent.SetDestination(dest);

            if (action.mode == RandomWalkMode.ONETIME) break;
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GoToBuilding(Creature creature, ActSequenceSystem.GoToBuilding action)
    {
        // Checking the possibility of execution
        if (action.Building == null || creature.CrtProp.PlaceOfStay != null)
        {
            FalseReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Building destBuilding = action.Building;
        Vector3 destPoint = creature.transform.position;
        switch (action.mode)
        {
            case GoToBuildingMode.ENTER:
                destPoint = destBuilding.CreatureContainer.Enter.position;
                break;
            case GoToBuildingMode.CENTER:
                destPoint = destBuilding.GridObject.GetCenter();
                break;
        }
        creature.Agent.SetDestination(destPoint);
        while (true)
        {
            if (destBuilding == null)
            {
                FalseReaction(creature, action);
                yield break;
            }

            if (Vector3.SqrMagnitude(destPoint - creature.transform.position) < creature.CrtData.defaultActionDistance)
            {
                yield return new WaitForSeconds(action.finalDelay);
                TrueReaction(creature, action);
                yield break;
            }

            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GetBuilding(Creature creature, ActSequenceSystem.GetBuilding action)
    {
        // Body
        Building building = null;
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        switch (action.target)
        {
            case GetBuildingMode.PLACEOFSTAY:
                building = (Building)creature.CrtProp.PlaceOfStay?.entity;
                break;
            case GetBuildingMode.HOME:
                building = (Building)creature.Appointer?.Home?.entity;
                break;
            case GetBuildingMode.WORK:
                building = (Building)creature.Appointer?.Work?.entity;
                break;
            case GetBuildingMode.DESTBUILDING:
                building = (Building)creature.GeneralAI.DestEntity;
                break;
        }
        if (building != null)
        {
            action.building = building;
            TrueReaction(creature, action);
        }
        else FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Equality(Creature creature, ActSequenceSystem.Equality action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        if (action.Object1 == action.Object2)
            TrueReaction(creature, action);
        else
            FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator ExitBuilding(Creature creature, ActSequenceSystem.ExitBuilding action)
    {
        // Checking the possibility of execution
        if (creature.CrtProp.PlaceOfStay == null)
        {
            TrueReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        creature.gameObject.transform.rotation = Quaternion.Euler(0f, GeneralAI.GetViewAngle(creature.CrtProp.PlaceOfStay.Enter.position - creature.transform.position), 0f);
        LeanTween.move(creature.gameObject, creature.CrtProp.PlaceOfStay.Enter.position + creature.CrtProp.HeightVector, creature.CrtData.timeOfBuildingEntering);
        yield return new WaitForSeconds(creature.CrtData.timeOfBuildingEntering);
        creature.CrtProp.PlaceOfStay.Remove(creature);
        creature.Agent.enabled = true;

        // Final delay
        yield return new WaitForSeconds(action.finalDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Rest(Creature creature, ActSequenceSystem.Rest action)
    {
        // Body
        while (true)
        {
            yield return new WaitForSeconds(12.345f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator CheckIndicator(Creature creature, ActSequenceSystem.CheckIndicator action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        float value;
        bool result = false;
        switch (action.indicator)
        {
            case IndicatorType.HEALTH:
                if (creature.Health == null)
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                value = creature.Health.Value;
                break;
            case IndicatorType.SATIETY:
                if (creature.Satiety == null)
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                value = creature.Satiety.Value;
                break;
            default:
                FalseReaction(creature, action);
                yield break;
        }

        switch (action.@operator)
        {
            case ComparingOperator.EQUAL:
                result = (value == action.value);
                break;
            case ComparingOperator.GREATER:
                result = (value > action.value);
                break;
            case ComparingOperator.GREATEREQUAL:
                result = (value >= action.value);
                break;
            case ComparingOperator.LESS:
                result = (value < action.value);
                break;
            case ComparingOperator.LESSEQUAL:
                result = (value <= action.value);
                break;
        }

        if (result)
            TrueReaction(creature, action);
        else
            FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GetQuery(Creature creature, ActSequenceSystem.GetQuery action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        switch (action.mode)
        {
            case GetQueryMode.EAT:
                action.query = new ResourceQuery(ResourceType.FOOD, VillageData.FoodServing);
                break;
            case GetQueryMode.RECIPE:
                action.query = new ResourceQuery(creature.GeneralAI.DestRecipe.requiredRes);
                break;
        }
        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator FindBuilding(Creature creature, ActSequenceSystem.FindBuilding action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        List<Building> buildings = new List<Building>();

        foreach (Building building in VillageData.Buildings)
        {
            if (building.BldData.BldType == action.bldType)
            {
                if (action.mode == FindBuildingMode.NEARQUERY && (building.Inventory == null || !building.Inventory.enabled || !building.Inventory.CheckResourceForQuery(action.ResourceQuery)))
                    continue;

                if (action.mode == FindBuildingMode.NEARINTERACT && (building.Interactive == null || !building.Interactive.enabled || building.Interactive.IsOccupied))
                    continue;

                if (action.mode == FindBuildingMode.NEARFREEPLACEFORQUERY && (building.Inventory == null || !building.Inventory.enabled || !building.Inventory.CheckPlaceFor(action.ResourceQuery)))
                    continue;

                if (action.mode == FindBuildingMode.NEARFREEPLACEFORINV && (building.Inventory == null || !building.Inventory.enabled || !building.Inventory.CheckPlaceFor(creature.Inventory)))
                    continue;

                buildings.Add(building);
            }
        }

        if (buildings.Count == 0)
        {
            FalseReaction(creature, action);
            yield break;
        }

        action.building = VillageData.GetNearestBuilding(buildings, creature.transform.position);
        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Take(Creature creature, ActSequenceSystem.Take action)
    {
        // Checking the possibility of execution
        if (action.mode == TakeMode.QUERY && (action.ResourceQuery == null || creature.CrtProp.PlaceOfStay == null) ||
            action.mode == TakeMode.ITEM && (action.Item == null || creature.CrtProp.PlaceOfStay != null))
        {
            FalseReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        if (action.mode == TakeMode.QUERY)
        {
            creature.CrtProp.PlaceOfStay.entity.Inventory.Give(creature.Inventory, action.ResourceQuery);
            action.outputQuery = action.ResourceQuery;
        }
        else if (action.mode == TakeMode.ITEM) 
        {
            if (!action.Item.Inventory.Occupy(creature))
            {
                action.Item.Inventory.RemoveOccupation(creature);
                yield return new WaitForSeconds(action.finalDelay);
                FalseReaction(creature, action);
                yield break;
            }
            creature.Agent.SetDestination(action.Item.transform.position);
            while (true)
            {
                if (action.Item == null)
                {
                    yield return new WaitForSeconds(action.finalDelay);
                    FalseReaction(creature, action);
                    yield break;
                }

                if (Vector3.SqrMagnitude(creature.transform.position - action.Item.transform.position) < creature.CrtData.defaultActionDistance)
                {
                    action.Item.Inventory.Give(creature.Inventory);
                    break;
                }

                yield return longTick;
            }
            action.Item.Inventory.RemoveOccupation(creature);
        }

        // Final delay
        yield return new WaitForSeconds(action.finalDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Wait(Creature creature, ActSequenceSystem.Wait action)
    {
        // Body
        yield return new WaitForSeconds(action.Delay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GoToInteraction(Creature creature, ActSequenceSystem.GoToInteraction action)
    {
        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Entity destEntity = action.Entity;

        switch (action.mode)
        {
            case GoToInteractionMode.NEAR:
                if (!destEntity.Interactive.OccupyNearest(creature.GeneralAI))
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                break;
            case GoToInteractionMode.NEARRECIPE:
                if (!destEntity.Interactive.OccupyNearestRecipe(creature.GeneralAI))
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                break;
            case GoToInteractionMode.RECIPEINDOORS:
                InteractionSpot _spot = null;
                foreach (int ind in creature.GeneralAI.DestRecipe.interactionSpotIndex)
                {
                    if (creature.CrtProp.PlaceOfStay.entity.Interactive.Occupy(ind, creature.GeneralAI))
                    {
                        _spot = creature.CrtProp.PlaceOfStay.entity.Interactive.Spot(ind);
                        Debug.Log("ind: " + ind + " true");
                        break;
                    }
                    else
                    {
                        Debug.Log("ind: " + ind + " false");
                    }
                }
                action.spot = _spot;
                if (action.spot != null)
                {
                    TrueReaction(creature, action);
                    yield break;
                }
                else
                {
                    FalseReaction(creature, action);
                    yield break;
                }
        }

        InteractionSpot spot = creature.GeneralAI.DestInteractionSpot;
        Vector3 destPoint = spot.Spot.position;
        creature.Agent.SetDestination(destPoint);
        while (true)
        {
            if (spot == null)
            {
                FalseReaction(creature, action);
                yield break;
            }

            if (Vector3.SqrMagnitude(destPoint - creature.transform.position) < creature.CrtData.defaultAttackDistance)
            {
                yield return new WaitForSeconds(action.finalDelay);
                action.spot = spot;

                // Final delay
                yield return new WaitForSeconds(action.finalDelay);

                TrueReaction(creature, action);
                yield break;
            }

            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Interact(Creature creature, ActSequenceSystem.Interact action)
    {
        // Checking the possibility of execution
        if (action.Spot == null)
        {
            FalseReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        action.Spot.Interact(creature, action.InteractionType);

        InteractionSpot spot = action.Spot;
        while(true)
        {
            if (spot == null)
            {
                FalseReaction(creature, action);
                yield break;
            }
            if (creature.GeneralAI.DestInteractionSpot == null && !spot.InteractionProcess) break;
            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        }

        // Final delay
        yield return new WaitForSeconds(action.finalDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Check(Creature creature, ActSequenceSystem.Check action)
    {
        // Body
        yield return null;

        switch (action.mode)
        {
            case CheckMode.EMPTYQUERY:
                ResourceQuery query = action.ResourceQuery;
                if (query.index != null)
                {
                    for (int i = 0; i < query.index.Length; i++)
                    {
                        if (query.index[i] != ResourceIndex.EXECUTEDQUERY)
                        {
                            FalseReaction(creature, action);
                            yield break;
                        }
                    }
                }
                if (query.type != null)
                {
                    for (int i = 0; i < query.type.Length; i++)
                    {
                        if (query.type[i] != ResourceType.EXECUTEDQUERY)
                        {
                            FalseReaction(creature, action);
                            yield break;
                        }
                    }
                }
                break;
            case CheckMode.EMPTYINV:
                {
                    for (int i = 0; i < creature.Inventory.PacksAmount; i++)
                    {
                        if (creature.Inventory.StoredRes[i] != ResourceIndex.NONE)
                        {
                            FalseReaction(creature, action);
                            yield break;
                        }
                    }
                }
                break;
            case CheckMode.AMILEADER:
                if (creature.Appointer.Work == null || (creature.Appointer.Work.CheckAppointmentIndex(creature.Appointer) != 0))
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                break;
            case CheckMode.PLACEFORITEM:
                if (action.Item == null || !creature.Inventory.CheckPlaceFor(action.Item.Inventory))
                {
                    FalseReaction(creature, action);
                    yield break;
                }
                break;
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator MathComparing(Creature creature, ActSequenceSystem.MathComparing action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        bool result = false;

        switch (action.@operator)
        {
            case MathComparingOperator.EQUAL:
                result = (action.Value1 == action.Value2);
                break;
            case MathComparingOperator.GREATER:
                result = (action.Value1 > action.Value2);
                break;
            case MathComparingOperator.GREATEREQUAL:
                result = (action.Value1 >= action.Value2);
                break;
            case MathComparingOperator.LESS:
                result = (action.Value1 < action.Value2);
                break;
            case MathComparingOperator.LESSEQUAL:
                result = (action.Value1 <= action.Value2);
                break;
        }

        if (result)
            TrueReaction(creature, action);
        else
            FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GetAmount(Creature creature, ActSequenceSystem.GetAmount action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        float amount = 0f;
        switch (action.target)
        {
            case GetAmountTarget.RESINDEX:                                          // In future amounts of resources will be stored in VillageData (exactly amounts in warehouses)
                foreach (Building building in VillageData.Buildings)                // Now it consumes performance
                {
                    if (building.BldData.BldType == BuildingType.WAREHOUSE)
                        amount += building.Inventory.AmountOfResource(action.resIndex);
                }
                break;
            case GetAmountTarget.RESTYPE:
                foreach (Building building in VillageData.Buildings)
                {
                    if (building.BldData.BldType == BuildingType.WAREHOUSE)
                        amount += building.Inventory.AmountOfResource(action.resType);
                }
                break;
        }

        action.amount = amount;
        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator RandomGenerator(Creature creature, ActSequenceSystem.RandomGenerator action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        switch (action.mode)
        {
            case RandomGeneratorMode.NUM:
                action.rndNumber = Random.Range(action.minRndNumber, action.maxRndNumber);
                break;
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator FindWork(Creature creature, ActSequenceSystem.FindWork action)
    {
        // Body
        yield return tick;

        if (creature.Appointer.Work == null)
        {
            for (int j = 0; j < 6; j++)
            {
                int rndIndex = Random.Range(0, 4);
                switch (rndIndex)
                {
                    case 0:
                        foreach (Item item in ItemManager.items)
                        {
                            for (int i = 0; i < item.Inventory.PacksAmount; i++)
                            {
                                if (VillageData.FreeSpaceForResource(item.Inventory.StoredRes[i]) > 0.001f && item.Inventory.Occupy(creature))
                                {
                                    action.sequence = ActSequenceIndex.VILLAGER_CARRIER;
                                    TrueReaction(creature, action);
                                    yield break;
                                }
                            }
                        }
                        break;
                    case 1:
                        foreach (ExtractedResourceLink item in VillageData.ExtractionQueue)
                        {
                            if (item.Occupy(creature.GeneralAI))
                            {
                                action.sequence = ActSequenceIndex.VILLAGER_EXTRACT;
                                TrueReaction(creature, action);
                                yield break;
                            }
                        }
                        break;
                    case 2:
                        Recipe recipe;
                        foreach (Production item in VillageData.Productions)
                        {
                            if ((recipe = item.GetReapWork()) != null)
                            {
                                recipe.Occupy(creature.GeneralAI);
                                action.sequence = ActSequenceIndex.VILLAGER_REAP;
                                TrueReaction(creature, action);
                                yield break;
                            }

                            if ((recipe = item.GetProduceWork()) != null)
                            {
                                recipe.Occupy(creature.GeneralAI);
                                action.sequence = ActSequenceIndex.VILLAGER_PRODUCE;
                                TrueReaction(creature, action);
                                yield break;
                            }
                        }
                        break;
                    case 3:
                        foreach (Building item in VillageData.Constructions)
                        {
                            if (item.BuildSet.ConstrStatus != ConstructionStatus.CONSTR) continue;

                            action.sequence = ActSequenceIndex.VILLAGER_CONSTRUCT;
                            creature.GeneralAI.DestEntity = item;
                            TrueReaction(creature, action);
                            yield break;
                        }
                        break;
                }
            }
        }
        else
        {
            if (creature.Appointer.Profession == Profession.HUNTER)
            {
                action.sequence = ActSequenceIndex.VILLAGER_HUNT;
                TrueReaction(creature, action);
                yield break;
            }
            
            if (creature.Appointer.Profession == Profession.ARTISAN)
            {
                action.sequence = ActSequenceIndex.VILLAGER_ARTISAN;
                TrueReaction(creature, action);
                yield break;
            }

            if (creature.Appointer.Profession == Profession.TAILOR)
            {
                action.sequence = ActSequenceIndex.VILLAGER_ARTISAN;
                TrueReaction(creature, action);
                yield break;
            }

            if (creature.Appointer.Profession == Profession.FISHERMAN)
            {
                action.sequence = ActSequenceIndex.VILLAGER_FISH;
                TrueReaction(creature, action);
                yield break;
            }
        }

        FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Put(Creature creature, ActSequenceSystem.Put action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        CreatureContainer placeOfStay;
        if ((placeOfStay = creature.CrtProp.PlaceOfStay) != null)
        {
            creature.Inventory.Give(placeOfStay.entity.Inventory);
        }
        else
        {
            creature.Inventory.LayOut();
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Construct(Creature creature, ActSequenceSystem.Construct action)
    {
        // Checking the possibility of execution
        if (action.Building == null || action.Building.BuildSet.ConstrStatus == ConstructionStatus.READY)
        {
            FalseReaction(creature, action);
            yield break;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Building construction = action.Building;
        while (true)
        {
            if (construction == null || action.Building.BuildSet.ConstrStatus == ConstructionStatus.READY)
                break;
            construction.BuildSet.Process += 1f;
            yield return new WaitForSeconds(creature.CrtData.constructionDelay);
        }

        // Final delay
        yield return new WaitForSeconds(action.finalDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GoToCreature(Creature creature, ActSequenceSystem.GoToCreature action)
    {
        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Creature target;
        Creature leader;
        switch (action.mode)
        {
            case GoToCreatureMode.DEFAULT:
                //...
                break;
            case GoToCreatureMode.WORKGROUP:
                float sqrReqDistance = Mathf.Pow(action.requiredDistance, 2), sqrGroupDistance = Mathf.Pow(action.groupDistance, 2);
                leader = (Creature)creature.Appointer.Work.GetPeople(0).entity;
                List<Transform> groupCreatures = new List<Transform>();

                if (creature == leader)
                {
                    target = (Creature)action.Creature.entity;
                    foreach (Appointer item in creature.Appointer.Work.GetPeople())
                    {
                        groupCreatures.Add(item.transform);
                        item.entity.GeneralAI.DestEntity = target;
                    }

                    while (true)
                    {
                        if (target == null || target.Health.Value < 0.001f)
                        {
                            FalseReaction(creature, action);
                            yield break;
                        }

                        creature.Agent.SetDestination(target.transform.position);
                        creature.GeneralAI.SwitchToWalk();

                        if (Distance.AvgSqrDistance(groupCreatures, target.transform) < sqrReqDistance)
                        {
                            yield return new WaitForSeconds(action.finalDelay);
                            TrueReaction(creature, action);
                            yield break;
                        }

                        if (Distance.AvgSqrDistance(groupCreatures, target.transform) < sqrReqDistance)
                        {
                            yield return new WaitForSeconds(action.finalDelay);
                            TrueReaction(creature, action);
                            yield break;
                        }

                        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
                    }
                }
                else
                {
                    while (true)
                    {
                        creature.Agent.SetDestination(creature.Appointer.Work.GetPeople(0).transform.position);
                        if (Vector3.SqrMagnitude(creature.transform.position - leader.transform.position) > sqrGroupDistance)
                            creature.GeneralAI.SwitchToRun();
                        else
                            creature.GeneralAI.SwitchToWalk();

                        if (leader.GeneralAI.CurrentAction == ActionType.ATTACK)               // Temporal
                        {
                            yield return new WaitForSeconds(action.finalDelay);
                            TrueReaction(creature, action);
                            yield break;
                        }

                        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
                    }
                }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Attack(Creature creature, ActSequenceSystem.Attack action)
    {
        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        creature.GeneralAI.SwitchToRun();
        float sqrAttackRange = Mathf.Pow(creature.CrtData.defaultAttackDistance, 2);
        while (true)
        {
            if (action.mode == AttackMode.WORKGROUP)
            {
                if (creature.Appointer.Work.CheckAppointmentIndex(creature.Appointer) == 0)
                {
                    creature.Appointer.Work.entity.Workplace.attackPermission = true;
                }
                else if (!creature.Appointer.Work.entity.Workplace.attackPermission)
                {
                    creature.GeneralAI.SwitchToWalk();

                    // Final delay
                    yield return new WaitForSeconds(action.finalDelay);

                    TrueReaction(creature, action);
                    yield break;
                }
            }

            if (action.Target == null ||action.Target.Value < 0.001f)
            {
                if (creature.Appointer.Work.CheckAppointmentIndex(creature.Appointer) == 0)
                {
                    creature.Appointer.Work.entity.Workplace.attackPermission = false;
                }
                creature.GeneralAI.SwitchToWalk();

                // Final delay
                yield return new WaitForSeconds(action.finalDelay);

                TrueReaction(creature, action);
                yield break;
            }

            if (Vector3.SqrMagnitude(creature.transform.position - action.Target.transform.position) < sqrAttackRange)
            {
                creature.AttackController.Attack(action.Target);
                //animator.Play("Armature|Hit");

                if (action.Target == null || action.Target.Value < 0.001f)
                {
                    if (action.mode == AttackMode.WORKGROUP && creature.AttackController.DropItemsCount > 0)
                        creature.Appointer.Work.entity.Workplace.AssignItems(creature.AttackController.DropItems);
                    creature.GeneralAI.SwitchToWalk();

                    if (creature.Appointer.Work.CheckAppointmentIndex(creature.Appointer) == 0)
                    {
                        creature.Appointer.Work.entity.Workplace.attackPermission = false;
                    }

                    // Final delay
                    yield return new WaitForSeconds(action.finalDelay);

                    TrueReaction(creature, action);
                    yield break;
                }
                else
                    yield return new WaitForSeconds(creature.AttackController.Duration);
            }
            else creature.Agent.SetDestination(action.Target.transform.position);

            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator FindTarget(Creature creature, ActSequenceSystem.FindTarget action)
    {
        // Body
        yield return tick;

        List<Health> targets = new List<Health>();

        if (action.mode == FindTargetMode.NEARANIMAL)
        {
            foreach (Creature item in CreatureManager.Animals)
            {
                if (item.Health.Value < 0.001f)
                    continue;

                targets.Add(item.Health);
            }

            if (targets.Count == 0)
            {
                FalseReaction(creature, action);
                yield break;
            }

            action.target = EntitySearch.ChooseNearestTarget(targets, creature.transform.position);
        }
        else if (action.mode == FindTargetMode.DESTENTITY)
        {
            if (creature.GeneralAI.DestEntity == null)
            {
                FalseReaction(creature, action);
                yield break;
            }
            action.target = creature.GeneralAI.DestEntity.Health;
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator FindItem(Creature creature, ActSequenceSystem.FindItem action)
    {
        // Body
        yield return tick;

        if (action.mode == FindItemMode.WORKITEM)
        {
            Item item;
            if ((item = creature.Appointer.Work.entity.Workplace.GetItem()) == null)
            {
                FalseReaction(creature, action);
                yield break;
            }
            item.Inventory.Occupy(creature);
            action.item = item;
        }
        else if (action.mode == FindItemMode.DROP)
        {
            Item item;
            if ((item = creature.AttackController.GetFirstDropItem()) == null || !item.Inventory.Occupy(creature))
            {
                FalseReaction(creature, action);
                yield break;
            }
            action.item = item;
        }
        else if (action.mode == FindItemMode.RADIUS)
        {
            Item item = EntitySearch.ChooseNearestItem(creature.transform.position);
            if (item == null || !item.Inventory.Occupy(creature) || Vector3.SqrMagnitude(item.transform.position - creature.transform.position) > Mathf.Pow(action.radius, 2))
            {
                FalseReaction(creature, action);
                yield break;
            }
            action.item = item;
        }
        else if (action.mode == FindItemMode.DESTINV)
        {
            Item item = (Item)creature.GeneralAI.DestInv.entity;
            if (item == null || !item.Inventory.Occupy(creature))
            {

                FalseReaction(creature, action);
                yield break;
            }
            action.item = item;
        }
        else if (action.mode == FindItemMode.HASSPACEINWAREHOUSE)
        {
            Item item = EntitySearch.ChooseNearestItemWithFreeSpaceInWarehouse(creature.transform.position);
            if (item == null || !item.Inventory.Occupy(creature))
            {
                FalseReaction(creature, action);
                yield break;
            }
            action.item = item;
        }

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GetNature(Creature creature, ActSequenceSystem.GetNature action)
    {
        // Body
        Nature nature = null;
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        switch (action.target)
        {
            case GetNatureMode.DESTEXTRACTEDRES:
                nature = creature.GeneralAI.DestExtractedResource.deposit.entity as Nature;
                break;
        }
        if (nature != null)
        {
            action.nature = nature;
            TrueReaction(creature, action);
        }
        else FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator FindRecipe(Creature creature, ActSequenceSystem.FindRecipe action)
    {
        // Body
        yield return tick;

        Recipe recipe;
        if (action.mode == FindRecipeMode.PROFPRODUCEWORK)
        {
            if ((recipe = creature.Appointer.Work.entity.Production.GetProduceWork(true)) != null)
            {
                recipe.Occupy(creature.GeneralAI);
                TrueReaction(creature, action);
                yield break;
            }
        }
        else if (action.mode == FindRecipeMode.PROFREAPWORK)
        {
            if ((recipe = creature.Appointer.Work.entity.Production.GetReapWork(true)) != null)
            {
                recipe.Occupy(creature.GeneralAI);
                TrueReaction(creature, action);
                yield break;
            }
        }

        FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GoTo(Creature creature, ActSequenceSystem.GoTo action)
    {
        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Vector3 targetPos = creature.transform.position;
        switch (action.mode)
        {
            case GoToMode.NEARCOAST:
                targetPos = SCCoord.GetCenter(SmallCellGrid.GetNearestCoastCellState(creature.transform.position).coord);
                break;
        }
        creature.Agent.SetDestination(targetPos);
        while (true)
        {
            if (Vector3.SqrMagnitude(targetPos - creature.transform.position) < 0.8f)
            {
                // Final delay
                yield return new WaitForSeconds(action.finalDelay);

                TrueReaction(creature, action);
                yield break;
            }

            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator Extract(Creature creature, ActSequenceSystem.Extract action)
    {
        // Body
        yield return new WaitForSeconds(action.duration);
        creature.Inventory.CreateResource(action.query);

        TrueReaction(creature, action);
    }





    /*public void FindLaborerWork(ActionUnit action)
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
    }*/







    static void TrueReaction(Creature creature, Node action)
    {
        creature.GeneralAI.SwitchCurrentAction(action.GetOutputPort("trueConnection").Connection?.node as ActionNode);
    }

    static void FalseReaction(Creature creature, Node action)
    {
        creature.GeneralAI.SwitchCurrentAction(action.GetOutputPort("falseConnection").Connection?.node as ActionNode);
    }
}