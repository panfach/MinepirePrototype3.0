using UnityEngine;

public static class TimeEvents
{
    public static void StartOfTheDay()
    {
        StateManager.SetOrdinaryMode();
        CreatureManager.DefineVillagerBehaviours(3);
    }

    public static void EndOfTheDay()
    {
        Debug.Log("TimeEvents.EndOfTheDay");
        CreatureManager.DefineVillagerBehaviours(3);
    }

    public static void TurnChanging()
    {
        StateManager.SetChangeTurnMode();
    }
}
