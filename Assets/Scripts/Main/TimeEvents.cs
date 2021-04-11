

public static class TimeEvents
{
    public static void StartOfTheDay()
    {
        StateManager.SetOrdinaryMode();
        VillagerManager.MorningBehaviourDefinition();
    }

    public static void EndOfTheDay()
    {
        VillagerManager.DefineAllBehaviours(3);
    }

    public static void TurnChanging()
    {
        StateManager.SetChangeTurnMode();
    }
}
