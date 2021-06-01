


[System.Serializable]
public class ResourceDeposit_old
{
    /*public ResourceSourceInstance resourceSource;        // resourceSourceInstance, в который входит данный экземпляр класса ResourceDeposit
    public int extInd;                                   // external index . Внешний индекс в массиве deposit в resourceSourceInstance

    public float amount;                                 // Количество ресурса
    public bool extractable;                             // Находится ли в очереди на добычу
    public bool occupied;                                // Есть ли житель, который собирается взаимодействовать с данным классом
    public Villager owner;                               // Если есть, то ссылка на этого жителя

    public ResourceDeposit(ResourceSourceInstance _source, int _extInd, float _amount)
    {
        resourceSource = _source;
        extInd = _extInd;
        amount = _amount;
        extractable = false;
        occupied = false;
        owner = null;
    }

    public bool Occupy(Villager _owner)
    {
        if (occupied && _owner != owner) return false;
        else
        {
            occupied = true;
            owner = _owner;
            RefreshInfo();
            return true;
        }
    }

    public void RemoveOccupation(Villager _villager)
    {
        if (_villager == owner)
        {
            occupied = false;
            owner = null;

            RefreshInfo();
        }
    }

    public void ForceRemoveFromExtractionQueue()
    {
        if (!extractable) return;

        extractable = false;
        VillageData.extractionQueue.Remove(this);
        if (occupied)
        {
            Villager villager = owner;
            owner = null;
            villager.destResourceDeposit = null;
            villager.DefineBehaviour(7);
        }

        RefreshInfo();
    }

    public void AddToExtractionQueue()
    {
        if (amount == 0)
        {
            Notification.Invoke(NotifType.RESSOURCE);
            return;
        }

        VillageData.extractionQueue.Add(this);
        extractable = true;
        VillagerManager.DefineBehaviourOfFreeLaborers();

        RefreshInfo();
    }

    public void Extract(float _amount = float.MaxValue)
    {
        if (_amount < 0f) return;
        if (_amount > amount) _amount = amount;

        Connector.resourceManager.ExecuteExtracting(this, _amount);
        amount -= _amount;

        if (amount == 0)
        {
            extractable = false;
            VillageData.extractionQueue.Remove(this);

            if (resourceSource.data.resources[extInd].sourceDeletionAfterExhaustion)
            {
                resourceSource.Die();
            }
            else
            {
                Connector.panelInvoker.RefreshResourceSourceInfo();
            }
        }

        RefreshInfo();
    }

    void RefreshInfo()
    {
        Connector.panelInvoker.RefreshResourceSourceInfo();
        resourceSource.SetSmallInfo();
        resourceSource.smallInfo.Refresh();
    }*/
}
