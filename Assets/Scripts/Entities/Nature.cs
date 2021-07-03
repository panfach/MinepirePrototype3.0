using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : Entity
{
    [SerializeField] NatureData natureData;
    [SerializeField] NatureProperties natureProperties;
    [SerializeField] SmallInfoController smallInfoController;
    [SerializeField] DisplayedItems displayedItems;
    [SerializeField] ResourceDeposit resourceDeposit;
    [SerializeField] UIController uiController;
    [SerializeField] ColliderHandler colliderHandler;
    [SerializeField] Interactive interactive;
    [SerializeField] GridObject gridObject;                                                            // In future

    public override NatureData NtrData { get => natureData; }
    public override NatureProperties NtrProp { get => natureProperties; }
    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }
    public override ResourceDeposit ResourceDeposit { get => resourceDeposit; }
    public override UIController UIController { get => uiController; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override Interactive Interactive { get => interactive; }
    public override GridObject GridObject { get => gridObject; }


    public override void Die()
    {
        NatureManager.natures.Remove(this);

        Destroy(gameObject);
    }
}
