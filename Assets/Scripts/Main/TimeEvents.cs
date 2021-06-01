

public static class TimeEvents
{
    public static void StartOfTheDay()
    {
        StateManager.SetOrdinaryMode();
        CreatureManager.MorningBehaviourDefinition();
    }

    public static void EndOfTheDay()
    {
        CreatureManager.DefineVillagerBehaviours(3);
    }

    public static void TurnChanging()
    {
        StateManager.SetChangeTurnMode();
    }
}
