using UnityEngine;

public class BuildingEnterTrigger : MonoBehaviour
{
    public Building building;
    Villager villager;

    private void OnTriggerEnter(Collider other)
    {
        if ((villager = other.GetComponent<Villager>()) != null && building != null)
        {
            villager = other.GetComponent<Villager>();
            if (villager.state == VillagerState.GOTOHOUSE && villager.destinationBuilding == building)
            {
                //Debug.Log("Try to enter ...");
                villager.EnterBuilding(building);
            }
        }
    }
}
