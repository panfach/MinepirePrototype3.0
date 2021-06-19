using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(5)]
[RequireComponent(typeof(Inventory))]
public class Production : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] float performance = 1f;
    [SerializeField] Recipe[] recipe;
    [SerializeField] bool requiredVillager;
    [SerializeField] float interactionDuration;

    public int RecipeCount { get => recipe.Length; }
    public Recipe Recipe() { return recipe[0]; }
    public Recipe Recipe(int i) { return recipe[i]; }

    public SimpleEventHandler changedEvent;


    private void OnEnable()
    {
        VillageData.Productions.Add(this);
        changedEvent += Set;
        entity.Inventory.invChangedEvent += Set;
        InitRecipes();
    }


    public void InitRecipes()
    {
        for (int i = 0; i < recipe.Length; i++)
        {
            recipe[i].Init(this);
            for (int j = 0; j < recipe[i].interactionSpotIndex.Length; j++)
            {
                recipe[i].Spot(j).AssignRecipe(i);
            }
        }
    }

    public void Set()
    {
        if (requiredVillager) return;

        foreach (Recipe item in recipe)
        {
            if (entity.Inventory.CheckAllResourceForQuery(item.requiredRes) && !item.Process && !item.Harvest && item.Queue > 0)
                StartCoroutine(Produce(item));
        }
    }

    public void ChangeQueue(int recipeInd, int amount)
    {
        recipe[recipeInd].Queue += amount;
        changedEvent?.Invoke();
    }


    public Recipe GetReapWork()
    {
        foreach (Recipe item in recipe)
        {
            if (item.NeedToReap) return item;
        }

        return null;
    }

    public Recipe GetProduceWork()
    {
        foreach (Recipe item in recipe)
        {
            if (item.NeedToProduce) return item;
        }

        return null;
    }

    IEnumerator Produce(Recipe recipe)
    {
        float amountPerSecond;
        float duration = recipe.laborIntensity / performance;

        entity.Inventory.ClearResource(recipe.requiredRes);
        recipe.Queue -= 1;

        recipe.Process = true;
        recipe.Progress = 0f;
        amountPerSecond = 1f / duration;
        for (int i = 0; i < (int)duration; i++)
        {
            changedEvent?.Invoke();
            yield return new WaitForSeconds(1f);
            recipe.Progress += amountPerSecond;
        }
        recipe.Progress = 1f;

        entity.Inventory.CreateResource(recipe.receivedRes);
        recipe.StopProcess();
        changedEvent?.Invoke();
    } 

    public void Reap(Recipe recipe, GeneralAI worker)
    {
        entity.Inventory.Give(worker.entity.Inventory, new ResourceQuery(recipe.receivedRes));
        recipe.Harvest = false;
        recipe.RemoveOccupation();
        changedEvent?.Invoke();
    }


    private void OnDisable()
    {
        VillageData.Productions.Remove(this);
        changedEvent -= Set;
        entity.Inventory.invChangedEvent -= Set;
    }
}
