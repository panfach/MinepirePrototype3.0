using UnityEngine;

[DefaultExecutionOrder(50)]
public class UIController : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool openInfoOnClick;
    [SerializeField] bool reactToInventoryChanges;
    [SerializeField] bool reactToHealthChanges;
    [SerializeField] bool reactToSatietyChanges;
    [SerializeField] bool reactToProductionChanges;


    private void OnEnable()
    {
        if (openInfoOnClick && entity.ColliderHandler != null) entity.ColliderHandler.mouseDownEvent += OpenInfo;
        if (reactToInventoryChanges && entity.Inventory != null) entity.Inventory.invChangedEvent += RefreshInfo;
        if (reactToHealthChanges && entity.Health != null) entity.Health.changedValueEvent += RefreshInfo;
        if (reactToSatietyChanges && entity.Satiety != null) entity.Satiety.changedValueEvent += RefreshInfo;
        if (entity.Appointer != null) entity.Appointer.appointmentChangedEvent += RefreshInfo;
        if (entity.ResourceDeposit != null) entity.ResourceDeposit.resourceChangedEvent += RefreshInfo;
        if (reactToProductionChanges && entity.Production != null) entity.Production.changedEvent += RefreshInfo;
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
        if (reactToInventoryChanges && entity.Inventory != null) entity.Inventory.invChangedEvent -= RefreshInfo;
        if (reactToHealthChanges && entity.Health != null) entity.Health.changedValueEvent -= RefreshInfo;
        if (reactToSatietyChanges && entity.Satiety != null) entity.Satiety.changedValueEvent -= RefreshInfo;
        if (entity.Appointer != null) entity.Appointer.appointmentChangedEvent -= RefreshInfo;
        if (entity.ResourceDeposit != null) entity.ResourceDeposit.resourceChangedEvent -= RefreshInfo;
        if (reactToProductionChanges && entity.Production != null) entity.Production.changedEvent -= RefreshInfo;
    }
}
