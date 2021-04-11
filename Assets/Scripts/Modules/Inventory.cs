using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class Inventory : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] MonoBehaviour baseObject;                                                                                        // Fix to Entity

    [Header("Settings")]
    [SerializeField] bool hasOccupiedProperty;
    [SerializeField] bool destroyOnEmpty;
    [SerializeField] int storedPackSize;
    [SerializeField] int storedPacksAmount;

    [Header("Info")]
    [SerializeField] bool occupied;
    [SerializeField] Villager owner;
    [SerializeField] ResourceIndex[] storedRes;
    [SerializeField] float[] storedVal;

    public event SimpleEventHandler invChangedEvent;

    public int PackSize { get => storedPackSize; }
    public int PacksAmount { get => storedPacksAmount; }
    public ResourceIndex[] StoredRes { get => storedRes; }
    public float[] StoredVal { get => storedVal; }

    // -------------------------------------------------------------------------------------------------- //

    private void OnEnable()                                                                                     // Add OnDisable also
    {
        Init();
    }

    // -------------------------------------------------------------------------------------------------- //

    public void Init()
    {
        storedRes = new ResourceIndex[storedPacksAmount];
        storedVal = new float[storedPacksAmount];
    }

    public void Init(int size, int amount) 
    {
        storedPackSize = size;
        storedPacksAmount = amount;
        storedRes = new ResourceIndex[storedPacksAmount];
        storedVal = new float[storedPacksAmount];
    }

    public void ItemInit(ResourceIndex ind, float val)
    {
        PutResource(ind, val);
        VillageData.resources[(int)ind] += val;
        RefreshInfo();
    }

    /// <summary>
    /// Putting a specific amount of resource from specific pack of this inventory into specific inventory
    /// </summary>
    /// <returns>Return amount of resource, that was not moved</returns>
    public float Give(Inventory targetInventory, int packIndex, float value = float.MaxValue)
    {
        if (value < 0.001f) return 0f;

        float freeSpace, giveRemainder = 0f, takeRemainder;
        ResourceIndex resInd = storedRes[packIndex];
        if (value > (freeSpace = targetInventory.FreeSpaceForResource(storedRes[packIndex])))
        {
            giveRemainder = value - freeSpace;
            value = freeSpace;
        }
        takeRemainder = TakeResource(packIndex, value);
        giveRemainder += takeRemainder;
        value -= takeRemainder;
        Debug.Log("Value to put : " + value);
        targetInventory.PutResource(resInd, value);

        RefreshInfo();

        return giveRemainder;
    }

    /// <summary>
    /// Putting a specific amount of resource from this inventory into specific inventory
    /// </summary>
    /// <returns>Return amount of resource, that was not moved</returns>
    public float Give(Inventory targetInventory, ResourceIndex index, float value = float.MaxValue)
    {
        if (value < 0.001f || index == ResourceIndex.NONE || index == ResourceIndex.LOG || index == ResourceIndex.DEERSKIN) return 0f;               // These resources are not in the game yet

        float freeSpace, giveRemainder = 0f, takeRemainder;
        if (value > (freeSpace = targetInventory.FreeSpaceForResource(index)))
        {
            giveRemainder = value - freeSpace;
            value = freeSpace;
        }
        takeRemainder = TakeResource(index, value);
        giveRemainder += takeRemainder;
        value -= takeRemainder;
        targetInventory.PutResource(index, value);

        RefreshInfo();

        return giveRemainder;
    }

    public void LayOut(int packIndex, float value = float.MaxValue)
    {
        Item item = Connector.itemManager.CreateEmptyItem(transform.position);
        Give(item.Inventory, packIndex, value);
    }

    /// <summary>
    /// Calculates how much specific resource can fit into this inventory
    /// </summary>
    public float FreeSpaceForResource(ResourceIndex index)
    {
        float freeSpace = 0f;

        for (int i = 0; i < storedPacksAmount; i++)
        {
            if (storedRes[i] == index) freeSpace += storedPackSize - storedVal[i];
            if (storedRes[i] == ResourceIndex.NONE) freeSpace += storedPackSize;
        }

        return freeSpace;
    }

    /// <summary>
    /// Checks presence the empty space for required resource
    /// </summary>
    public bool CheckPlaceFor(ResourceIndex index)
    {
        if (FreeSpaceForResource(index) > 0.001f) return true;

        return false;
    }

    /// <summary>
    /// Checks presence the empty space for at least one resource
    /// </summary>
    public bool CheckPlaceFor(ResourceIndex[] indices)
    {
        foreach (ResourceIndex index in indices)
        {
            if (FreeSpaceForResource(index) > 0.001f) return true;
        }

        return false;
    }

    /// <summary>
    /// Gets information about inventory pack
    /// </summary>
    public bool Look(int packIndex, out ResourceIndex index, out float value)          
    {
        index = 0; value = 0f;
        if (packIndex >= storedPacksAmount) return false;

        index = storedRes[packIndex];
        value = storedVal[packIndex];
        return true;
    }

    /// <summary>
    /// Returns indices of packs, that contain specific resource
    /// </summary>
    public int[] SearchResource(ResourceIndex ind)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < storedPacksAmount; i++)
        {
            if (storedRes[i] == ind) result.Add(i);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Returns total amount of specific resource in inventory
    /// </summary>
    public float AmountOfResource(ResourceIndex ind)
    {
        float result = 0f;

        for (int i = 0; i < storedPacksAmount; i++)
        {
            if (storedRes[i] == ind) result += storedVal[i];
        }

        return result;
    }

    /// <summary>
    /// Irrevocably deletes specific amount of resource in pack
    /// </summary>
    /// <returns>Remainder, if amount more than actual value</returns>
    public float ClearPack(int packIndex, float amount = float.MaxValue)
    {
        float remainder = 0;
        if (amount > storedVal[packIndex])
        {
            remainder = amount - storedVal[packIndex];
            amount = storedVal[packIndex];
        }

        VillageData.resources[(int)storedRes[packIndex]] -= amount;
        TakeResource(packIndex, amount);
        InfoDisplay.Refresh();

        return remainder;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < PacksAmount; i++)
        {
            if (storedVal[i] > 0.001f) return false;
        }
        return true;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < storedPacksAmount; i++)
        {
            ClearPack(i, float.MaxValue);
        }
    }

    public float CreateResource(ResourceIndex ind, float val)
    {
        return PutResource(ind, val);
    }

    public void RefreshInfo()                                                                     // Turn it into events in future
    {
        if (baseObject is Building)
        {
            Connector.panelInvoker.RefreshBuildingInfo();
            ((Building)baseObject).SmallInfoController.Refresh();
            invChangedEvent?.Invoke();
        }
        if (baseObject is Item)
        {
            ((Item)baseObject).SmallInfoController.Refresh();
            invChangedEvent?.Invoke();
        }
        else if (baseObject is Villager)
        {
            Connector.panelInvoker.RefreshVillagerInfo();
            ((Villager)baseObject).smallInfo.Refresh();
        }
    }

    public bool Occupy(Villager villager)
    {
        Debug.Log((!occupied || villager == owner) && villager.inventory.CheckPlaceFor(storedRes));
        if ((!occupied || villager == owner) && villager.inventory.CheckPlaceFor(storedRes))
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

    /// <summary>
    /// Putting a specific amount of resource in the inventory
    /// </summary>
    /// <returns>Return amount of resource, that didn't fit into inventory</returns>
    private float PutResource(ResourceIndex index, float value)
    {
        if (value < 0.001f) return 0f;
        if (index == ResourceIndex.NONE || index == ResourceIndex.LOG || index == ResourceIndex.DEERSKIN) return 0f;

        for (int i = 0; i < storedPacksAmount; i++)
        {
            // If pack contains the same resource
            if (storedRes[i] == index && storedVal[i] < storedPackSize)
            {
                storedVal[i] += value;
                if (storedVal[i] <= storedPackSize)
                {
                    value = 0f;
                    break;
                }
                else
                {
                    value = storedVal[i] - storedPackSize;
                    storedVal[i] = storedPackSize;
                }
            }
            // If pack is empty
            if (storedRes[i] == ResourceIndex.NONE)
            {
                storedRes[i] = index;
                storedVal[i] = value;
                if (storedVal[i] <= storedPackSize)
                {
                    value = 0f;
                    break;
                }
                else
                {
                    value = storedVal[i] - storedPackSize;
                    storedVal[i] = storedPackSize;
                }
            }
        }

        Debug.Log("Now in target inventory " + storedRes[0] + " : " + storedVal[0]);

        return value;
    }

    /// <summary>
    /// Deletes specific amount of resource in the inventory
    /// </summary>
    /// <returns>Returns a remainder, if value more than amount of resource in the inventory</returns>
    private float TakeResource(ResourceIndex index, float value)
    {
        float remainder = value;

        for (int i = 0; i < storedPacksAmount; i++)
        {
            if (storedRes[i] == index)
            {
                remainder -= storedVal[i];
                if (remainder <= 0)
                {
                    storedVal[i] = -remainder;
                    remainder = 0;
                    break;
                }
                else
                {
                    storedVal[i] = 0;
                }
            }
        }

        if (destroyOnEmpty && IsEmpty()) baseObject.GetComponent<Entity>().Die();
        return remainder;
    }

    /// <summary>
    /// Deletes specific amount of resource in the specific pack
    /// </summary>
    private float TakeResource(int packIndex, float amount)
    {
        if (packIndex >= storedPacksAmount) return amount;

        float remainder = 0;

        if (amount >= storedVal[packIndex])
        {
            remainder = amount - storedVal[packIndex];
            storedRes[packIndex] = ResourceIndex.NONE;
            storedVal[packIndex] = 0f;
        }
        else
        {
            storedVal[packIndex] -= amount;
        }

        if (destroyOnEmpty && IsEmpty()) baseObject.GetComponent<Entity>().Die();
        return remainder;
    }


    /// <summary>
    /// Deletes specific amount of resource in the inventory
    /// </summary>
    /// <returns></returns>
    /*private float TakeResource(int packIndex, float amount, out ResourceIndex index, out float value)
    {
        index = 0; value = 0f;
        if (packIndex >= storedPacksAmount) return false;

        index = storedRes[packIndex];
        if (amount >= storedVal[packIndex])
        {
            value = storedVal[packIndex];
            storedRes[packIndex] = ResourceIndex.NONE;
            storedVal[packIndex] = 0f;
        }
        else
        {
            value = amount;
            storedVal[packIndex] -= amount;
        }

        RefreshInfo();
        return true;
    }*/

    /// <summary>
    /// Checks presence the empty space for required resource
    /// </summary>
    //public bool CheckPlaceFor(ResourceIndex index)
    //{
    //    bool result = false;

    //    for (int i = 0; i < storedPacksAmount; i++)
    //    {
    //        if (storedRes[i] == ResourceIndex.NONE || (storedRes[i] == index && storedVal[i] < storedPackSize)) result = true;
    //    }
    //    return result;
    //}

    // -------------------------------------------------------------------------------------------------- //

    private void OnDisable()
    {
        ClearInventory();
        invChangedEvent = null;
    }
}
