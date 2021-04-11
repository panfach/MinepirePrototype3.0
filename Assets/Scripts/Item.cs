using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    [SerializeField] SmallInfoController smallInfoController;
    [SerializeField] Inventory inventory;
    [SerializeField] ColliderHandler colliderHandler;
    [SerializeField] DisplayedItems displayedItems;

    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override Inventory Inventory { get => inventory; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }


    public override void Die()
    {
        ItemManager.items.Remove(this);

        Destroy(gameObject);
    }
}
