using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorInteraction : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] float duration;
    [SerializeField] bool restoreSatiety;
    [SerializeField] bool spendFood;


    public IEnumerator Interact(Creature creature, InteractionSpot spot)
    {
        float amount, amountPerSecond;

        if (restoreSatiety)
        {
            spot.InteractionProcess = true;
            LeanTween.move(creature.gameObject, spot.Spot.position, creature.CrtData.checkEventsDelay);
            LeanTween.rotate(creature.gameObject, new Vector3(0f, GeneralAI.GetViewAngle(spot.InteractionTarget.position), 0f), creature.CrtData.checkEventsDelay);
            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

            if (spendFood)
            {
                amount = (creature.Inventory.StoredVal[0] + creature.Inventory.StoredVal[1]) / 2f;
                amountPerSecond = amount / duration;

                for (int i = 0; i < (int)duration; i++)
                {
                    creature.Satiety.Value += amountPerSecond;
                    yield return new WaitForSeconds(1f);
                }
                creature.Satiety.Value += amountPerSecond * (duration % 1f);
                creature.Inventory.ClearInventory();
            }
            else
            {
                amount = 1f - creature.Satiety.Value;
                amountPerSecond = amount / duration;

                for (int i = 0; i < (int)duration; i++)
                {
                    creature.Satiety.Value += amountPerSecond;
                    yield return new WaitForSeconds(1f);
                }
                creature.Satiety.Value += amountPerSecond * (duration % 1f);
            }

            spot.RemoveOccupation();
        }
    }
}
