using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool openInfoOnClick;


    private void OnEnable()
    {
        if (openInfoOnClick && entity.ColliderHandler != null) entity.ColliderHandler.mouseDownEvent += OpenInfo;
        if (entity.PeopleAppointer != null) entity.PeopleAppointer.peopleChangedEvent += RefreshInfo;
        if (entity.ResourceDeposit != null) entity.ResourceDeposit.resourceChangedEvent += RefreshInfo;
    }


    public void OpenInfo()
    {
        Connector.panelInvoker.OpenInfo(entity);
    }

    public void RefreshInfo()
    {
        Connector.panelInvoker.RefreshInfo(entity);
    }

    public void CloseInfo()
    {
        Connector.panelInvoker.CloseInfo(entity);
    }


    private void OnDisable()
    {
        CloseInfo();
        if (openInfoOnClick && entity.ColliderHandler != null) entity.ColliderHandler.mouseDownEvent -= OpenInfo;
        if (entity.PeopleAppointer != null) entity.PeopleAppointer.peopleChangedEvent -= RefreshInfo;
        if (entity.ResourceDeposit != null) entity.ResourceDeposit.resourceChangedEvent -= RefreshInfo;
    }
}
