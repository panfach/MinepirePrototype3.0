using System;
using System.Collections.Generic;
using UnityEngine;


public class ResourceDeposit : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] Transform extractPoint;

    [Header("Info")]
    [SerializeField] ResourceIndex[] index;
    [SerializeField] float[] amount;                                 // Количество ресурса
    [SerializeField] bool[] extractable;                             // Находится ли в очереди на добычу
    [SerializeField] bool[] occupied;                                // Есть ли житель, который собирается взаимодействовать с данным классом
    [SerializeField] GeneralAI[] owner;                              // Если есть, то ссылка на этого жителя

    public Transform ExtractPoint { get => extractPoint; }
    public ResourceIndex Index(int i) => index[i];
    public float Amount(int i) => amount[i];
    public bool Extractable(int i) => extractable[i];
    public bool Occupied(int i) => occupied[i];
    public GeneralAI Owner(int i) => owner[i];
    public int Size { get => index.Length; }

    public event SimpleEventHandler resourceChangedEvent;
    public event SimpleEventHandler statusChangedEvent;


    private void OnEnable()
    {
        int size = entity.NtrData.ResourceDepositSize;

        index = new ResourceIndex[size];
        for (int i = 0; i < size; i++) index[i] = entity.NtrData.ResData(i).Index;

        amount = new float[size];
        for (int i = 0; i < size; i++) amount[i] = entity.NtrData.Amount(i);

        extractable = new bool[size];

        occupied = new bool[size];

        owner = new GeneralAI[size];
    }


    public bool Occupy(int ind, GeneralAI _owner)
    {
        if (occupied[ind] && _owner != owner[ind]) return false;
        else
        {
            occupied[ind] = true;
            owner[ind] = _owner;
            statusChangedEvent?.Invoke();
            return true;
        }
    }

    public void RemoveOccupation(int ind, GeneralAI _villager)
    {
        if (_villager != owner[ind]) return;

        occupied[ind] = false;
        owner[ind] = null;
        _villager.ForgetExtractedResource();

        statusChangedEvent?.Invoke();
    }

    public void ForceRemoveFromExtractionQueue(int ind)
    {
        if (!extractable[ind]) return;

        extractable[ind] = false;
        VillageData.ExtractionQueue.Remove(new ExtractedResourceLink(this, ind));
        if (occupied[ind])
        {
            GeneralAI villager = owner[ind];
            RemoveOccupation(ind, villager);
            villager.DefineBehaviour(7);
        }

        statusChangedEvent?.Invoke();
    }

    public void AddToExtractionQueue(int ind)
    {
        if (amount[ind] < 0.001f || ind >= Size)
        {
            Notification.Invoke(NotifType.RESSOURCE);
            return;
        }

        VillageData.ExtractionQueue.Add(new ExtractedResourceLink(this, ind));
        extractable[ind] = true;
        CreatureManager.DefineBehaviourOfFreeLaborers();

        statusChangedEvent?.Invoke();
    }

    public void Extract(int ind, float _amount = float.MaxValue)
    {
        if (_amount < 0.001f) return;
        if (_amount > amount[ind]) _amount = amount[ind];

        Connector.itemManager.ExecuteExtracting(this, ind, _amount);
        amount[ind] -= _amount;

        if (amount[ind] == 0)
        {
            extractable[ind] = false;
            VillageData.ExtractionQueue.Remove(new ExtractedResourceLink(this, ind));

            if (entity.NtrData.DeletionAfterExhaustion(ind))
            {
                entity.Die();
            }
        }

        resourceChangedEvent?.Invoke();
        statusChangedEvent?.Invoke();
    }

    public void Recover()
    {
        for (int i = 0; i < Size; i++) Recover(i);
    }

    public void Recover(int ind)
    {
        if (entity.NtrData.Restorable(ind) && Amount(ind) < entity.NtrData.Amount(ind))
        {
            amount[ind] += entity.NtrData.RecoverySpeed(ind);
            Mathf.Clamp(amount[ind], 0f, entity.NtrData.Amount(ind));
        }
    }

    public bool ExtractableResourceExists()
    {
        for (int i = 0; i < Size; i++)
        {
            if (extractable[i]) return true;
        }

        return false;
    }


    private void OnDisable()
    {
                                                                                  // Handle cases when nature object deletes itself while someone go
                                                                                  // to extract resource or is extracting
                                                                                  // Need to remove extractable resource link form queue
        for (int i = 0; i < Size; i++)
        {
            ForceRemoveFromExtractionQueue(i);
        }
    }
}

public class ExtractedResourceLink : IEquatable<ExtractedResourceLink>
{
    public ResourceDeposit deposit;
    public int ind;

    public ExtractedResourceLink(ResourceDeposit _deposit, int _ind)
    {
        deposit = _deposit;
        ind = _ind;
    }

    public bool Occupy(GeneralAI _owner)
    {
        if (deposit.Occupy(ind, _owner))
        {
            _owner.DestExtractedResource = this;
            return true;
        }

        return false;
    }

    public void RemoveOccupation(GeneralAI _owner)
    {
        deposit.RemoveOccupation(ind, _owner);
    }

    public bool Equals(ExtractedResourceLink obj)
    {
        return (deposit == obj.deposit && ind == obj.ind);
    }
}
