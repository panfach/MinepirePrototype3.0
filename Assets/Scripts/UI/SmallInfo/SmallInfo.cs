using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallInfo : MonoBehaviour
{
    public Entity instance;
    public Transform _transform, objectTransform;
    public bool deletionFlag;

    protected bool needToActivate;


    private void Awake()
    {
        _transform = transform;
    }


    public void Set() 
    {
        if (!enabled) return;

        needToActivate = false;
        SmallInfoController controller = instance.SmallInfoController;

        SetInfoBox(controller.ReactToMouseEnter && instance.ColliderHandler.MouseOver || 
                   controller.ReactToMouseDrag && StateManager.VillagerDragging);
        SetIconBox(controller.ReactToZeroAppointedPeople && instance.Appointer.enabled && instance.Appointer.People == 0 ||
                   controller.ReactToResourceDepositStatus && instance.ResourceDeposit.ExtractableResourceExists() ||
                   controller.ReactToHomeAbsence && instance.Appointer.Home == null);
        gameObject.SetActive(needToActivate);
        Refresh();
    }

    public virtual void SetInfoBox(bool state) { }
    public virtual void SetIconBox(bool state) { }
    public virtual void Refresh() { }


    public void Delete()
    {
        Destroy(gameObject);
    }
}

public enum SmallInfoType
{
    NONE,
    BUILDING,
    VILLAGER,
    ANIMAL
}
