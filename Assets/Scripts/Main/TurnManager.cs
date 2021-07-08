using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class TurnManager : MonoBehaviour
{
    // Эта функция должна активироваться кнопкой "следующий ход"
    public void TurnProcess()
    {
        foreach (Creature item in CreatureManager.Villagers)
        {
            if (item.Appointer.Home != null && (item.CrtProp.PlaceOfStay == null || item.CrtProp.PlaceOfStay.entity != item.Appointer.Home.entity))
            {
                Notification.Invoke(NotifType.EMPTYHOME);
                return;
            }
        }

        Connector.effectSoundManager.PlayNextTurnSound();

        Connector.sunlight.ChangeTurn();
        TimeEvents.TurnChanging();
        Connector.panelInvoker.SetTimeScale(1);

        //stat.turn++;

        //GetResourses();
        //EstablishHappiness();
        //Eat();
        //Loss();
        //Build();
        //RandomEvents();

        //
        //VillageData.CleanDeletedObjects();

        // 
        //if (!VillagerManager.villagersWereSpawned)
        //{
        //    //Debug.Log("TurnManager 27 :: spawning villagers...");
        //    Connector.villagerManager.SpawnAllVillagers();
        //}

        // Recover resource deposits
        foreach (Nature item in NatureManager.natures)
        {
            item.ResourceDeposit.Recover();
        }

        //
        RandomEvents();

        //
        SpawnRandomAnimals();

        //
        SatietyRefresh();

        //
        VillageData.Recalculate();

        //
        SetHappiness();

        //
        Connector.panelInvoker.CloseBuildingInfo();
        Connector.panelInvoker.CloseNatureInfo();
        Connector.panelInvoker.CloseCreatureInfo();
        InfoDisplay.Refresh();
    }

    void RandomEvents()
    {
        // New villagers
        float chance = Mathf.Clamp((VillageData.happiness - 0.3f) * 2f, 0f, 1f); // foodRatio = 2 => chance = 0; foodRatio = 6 => chance = 1;
        Debug.Log("Chance = " + chance);

        if (Random.Range(0f, 1f) < chance)
        {
            for (int i = 0; i < 2; i++)
            {
                Debug.Log("Spawning 2 villagers ...");
                Connector.creatureManager.SpawnRandomVillager();
            }
        }
    }

    void SpawnRandomAnimals()
    {
        int x, z;
        float y;
        Vector3 spawnPos;

        for (int i = 0; i < CreatureManager.villagerPopulation / 3; i++)
        {
            x = Random.Range(1, 40);
            z = Random.Range(5, 60);

            if (CCoord.GetCell(CCoord.FromPos(new Vector3(x, 0, z))).Elevation == 0 || SmallCellGrid.cellState[x, z].slope)
            {
                i--;
                continue;
            }

            y = SCCoord.GetHeight(new SCCoord(x, z));
            spawnPos = SCCoord.GetCenter(new SCCoord(x, z), y);
            Connector.creatureManager.SpawnRandomAnimal(spawnPos);
        }
    }

    void SatietyRefresh()
    {
        foreach (Creature item in CreatureManager.Villagers)
        {
            item.Satiety.Value -= 0.25f; 
        }
    }

    void SetHappiness()
    {
        float foodRatio = VillageData.foodAmount / CreatureManager.villagerPopulation;
        VillageData.foodRatio = foodRatio;

        if (foodRatio < 0.5f)
        {
            VillageData.foodServing = 0.2f;
            VillageData.happiness -= 0.10f;
        }
        else if (foodRatio < 1.0f)
        {
            VillageData.foodServing = 0.5f;
            VillageData.happiness -= 0.05f;
        }
        else if (foodRatio < 1.5f)
        {
            VillageData.foodServing = 1.0f;
            VillageData.happiness -= 0.0f;
        }
        else if (foodRatio < 2.0f)
        {
            VillageData.foodServing = 1.3f;
            VillageData.happiness += 0.03f;
        }
        else if (foodRatio < 3.0f)
        {
            VillageData.foodServing = 1.6f;
            VillageData.happiness += 0.06f;
        }
        else if (foodRatio > 3.0f)
        {
            VillageData.foodServing = 2.0f;
            VillageData.happiness += 0.10f;
        }

        VillageData.happiness = Mathf.Clamp(VillageData.happiness, 0.0f, 1.0f);
    }
}

/*
using System.Collections.Generic;
using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class TurnManager : MonoBehaviour
{
    [Header("Game objects")]
    public GameObject canvas;

    [Header("Settings")]
    [Tooltip("Количество игровых клеток.")] public int SIZE = 100;
    [SerializeField] int MAX_STARVEADD = 30, MAX_THIRSTADD = 30;
    [SerializeField] float lackOfFoodLevel = 1.5f;

    [Header("Curves")]
    public int thirstAdditionUnit;
    public AnimationCurve lackOfFoodFunction;
    public AnimationCurve foodMultiplierFunctionA;
    public AnimationCurve starveAdditionFunction;  

    // Дополнительно
    GameData gameData;
    SummaryManager sumManager;
    RayCaster rayCaster;
    StateManager stateManager;
    FightManager fightManager;

    // Ячейка ( клетка ), хранящая данные о своем заполнении игровыми объектами
    public struct CellState 
    {
        public bool empty;
        public bool road;
    }

    

    // Основная структура с данными о поселении
    public struct Stat
    {
        public List<GameObject> objects;
        public int objCounter;

        public int homeless;
        public int population;

        public int houseAmount;
        public int constructionAmount;
        public int hutAmount;
        public int barnAmount;
        public int buildAmount;
        public int woodAmount;
        public int huntAmount;
        public int wellAmount;
        public int mineAmount;

        public int workless;
        public int builders;
        public int woodcutters;
        public int hunters;
        public int miners;
        public int guards;
        public int militia;

        public float wood;
        public float stone;
        public float meat;
        public float fruits;

        public int happiness;
        public float starveAddition;
        public float thirstAddition;
        public float warAddition;

        public int turn;
    }

    public Stat stat;
    public CellState[,] cellState;

    int[] resArr;




    public bool ChangePeople(string type, int amount)
    {
        if (type != "settlehut" && amount > 0 && stat.workless < amount)
        {
            return false;
        }
        if (type == "settlehut" && amount > 0 && stat.homeless < amount) {
            return false;
        }

        switch (type)
        {
            case "settlehut":
                stat.population += amount;
                stat.workless += amount;
                stat.homeless -= amount;
                break;
            case "buildhut":
                stat.builders += amount;
                stat.workless -= amount;
                break;
            case "woodshack":
                stat.woodcutters += amount;
                stat.workless -= amount;
                break;
            case "hunthut":
                stat.hunters += amount;
                stat.workless -= amount;
                break;
            case "mine":
                stat.miners += amount;
                stat.workless -= amount;
                break;
        }
        return true;
    }

    // Авто заполнение беженцами жилых хижин 
    public void AutoSettle() {
        if (stat.homeless == 0) return;
        if (stateManager.getFightMode() || stateManager.getChangeTurnMode()) return;
        foreach (GameObject item in stat.objects) {
            if (stat.homeless == 0) return;
            Building building = item.GetComponent<Building>();
            if (building.type == "settlehut" && building.state == "ready" && building.people < building.maxPeople) {
                int k = building.maxPeople - building.people;
                for (int i = 1; i <= k; i++) {
                    building.AddPerson();
                    if (stat.homeless == 0) return;
                }
            }
        }
        rayCaster.notification.TurnOn("livplace");
    }

    // Изменение числа караульных
    public void ChangeGuards(int amount) {
        if (stateManager.getFightMode() || stateManager.getChangeTurnMode()) return;
        if (!(stat.guards <= 0 && amount < 0) && !(stat.workless <= 0 && amount > 0)) {
            stat.guards += amount;
            stat.workless -= amount; 
        }
    }

    // Очистка места в хижине после смерти жителя
    public void HutPurification()
    {
        foreach (GameObject item in stat.objects)
        {
            if (item.GetComponent<Building>().type == "settlehut" && item.GetComponent<Building>().people > 0)
            {
                item.GetComponent<Building>().people--;
                break;
            }
        }
    }

    // Перерасчет жителей (Исправление бага)
    public void PeopleRecalculation() {
        Building building;
        int sum = 0;

        stat.builders = 0;
        stat.woodcutters = 0;
        stat.hunters = 0;
        stat.miners = 0;
        foreach (GameObject item in stat.objects) {
            building = item.GetComponent<Building>();
            switch (building.type)
            {
                case "buildhut":
                    stat.builders += building.people;
                    break;
                case "woodshack":
                    stat.woodcutters += building.people;
                    break;
                case "hunthut":
                    stat.hunters += building.people;
                    break;
                case "mine":
                    stat.miners += building.people;
                    break;
            }
        }
        foreach (GameObject item in stat.objects)
        {
            building = item.GetComponent<Building>();
            if (building.type == "settlehut") {
                sum += building.people;
            }
        }

        // Случай, если у ополченца сломали дом (Он продолжит сражаться, но в конце боя должен стать бездомным)
        if (sum < stat.workless + stat.builders + stat.woodcutters + stat.hunters + stat.miners) {
            stat.homeless += (stat.workless + stat.builders + stat.woodcutters + stat.hunters + stat.miners) - sum;
            stat.workless -= (stat.workless + stat.builders + stat.woodcutters + stat.hunters + stat.miners) - sum;
        }

        stat.population = stat.workless + stat.workless + stat.builders + stat.woodcutters + stat.hunters + stat.miners;
    }

    // Проверка: Достаточно ли ресурсов для постройки
    public bool CheckResForBuild(int mode)
    {
        int[] res = new int[2];
        int wood = 0, stone = 0;

        res = gameData.get_res(mode);
        wood = res[0];
        stone = res[1];

        if (stat.wood >= wood && stat.stone >= stone)
            return true;
        return false;
    }

    // Конец игры
    public void EndGame(int reason) {
        //GameObject.Find("Canvas").GetComponent<PanelInvoker>().SetEndGame(reason);
    }

    // --------------------------------------------- Функции для ProcessTurn ---------------------------------------------------- //
    // Добыча ресурсов за ход
    private int GetResourses()
    {
        float am;

        foreach (GameObject item in stat.objects) {
            Building building = item.GetComponent<Building>();
            if (building.type == "woodshack") { 
                for (int i = 0; i < building.people; i++) {
                    if (item.transform.Find("TriggerField_2").gameObject.GetComponent<TreeCutter>().CutTree())
                    {
                        stat.wood += 2;
                        sumManager.woodDiff += 2;
                        building.lackOfTrees = false;
                    }
                    else {
                        building.lackOfTrees = true;
                    }
                }
            }
        }

        am = stat.miners * 2;
        stat.stone += am;
        sumManager.stoneDiff += am;

        am = stat.hunters * 2;
        stat.meat += am;
        sumManager.meatDiff += am;

        am = stat.hunters;
        stat.fruits += am;
        sumManager.fruitsDiff += am;

        return 0;
    }

    // Установить начальное значение счастья
    private void EstablishHappiness()
    {
        float food = stat.meat + stat.fruits;
        float coef = food / stat.population;

        if (coef >= 10f)
        {
            stat.happiness = 60;
        }
        else
        {
            stat.happiness = (int)((coef / 10f) * 20 + 40); // От 40 до 60
        }
    }

    // Употребление пищи за ход
    private int Eat()
    {
        float food = stat.meat + stat.fruits;
        float coef = food / stat.population; // 
        float mult;
        float result;

        if (food < stat.population * lackOfFoodLevel)
            sumManager.MakeEvent(6); // Вызов события "Слишком мало еды"

        float meatShare = stat.meat / food;
        float fruitsShare = stat.fruits / food;

        if (coef >= 10)
        {
            mult = 2f;
        }
        else
        {
            mult = foodMultiplierFunctionA.Evaluate(coef);
        }

        result = stat.population * mult * meatShare;
        stat.meat -= result;
        sumManager.meatDiff -= result;

        result = stat.population * mult * fruitsShare;
        stat.fruits -= result;
        sumManager.fruitsDiff -= result;

        stat.starveAddition += starveAdditionFunction.Evaluate(coef);
        stat.starveAddition = Mathf.Clamp(stat.starveAddition, -MAX_STARVEADD - 10, MAX_STARVEADD);

        if (stat.wellAmount * 20 < stat.population) {
            stat.thirstAddition -= thirstAdditionUnit;
            sumManager.MakeEvent(8);
        }
        else if (stat.thirstAddition < 0) stat.thirstAddition += thirstAdditionUnit;
        stat.thirstAddition = Mathf.Clamp(stat.thirstAddition, -MAX_THIRSTADD, 0);

        stat.happiness += (int)(stat.starveAddition + stat.thirstAddition);
        stat.happiness = Mathf.Clamp(stat.happiness, 0, 100);

        return 0;
    }

    // Потеря ресурсов при отсутствии мест на складах
    private void Loss() {
        int warehouseSpace = 0;
        float resAmount, attit, constLossCoef = 0.5f, lossCoef, oldVal;

        foreach (GameObject item in stat.objects) {
            if (item.GetComponent<Building>().type == "barn") warehouseSpace += 100;
        }

        resAmount = (float)(stat.meat + stat.fruits + stat.wood + stat.stone);
        if (resAmount > warehouseSpace) {
            attit = warehouseSpace / resAmount;
            lossCoef = constLossCoef + (attit / 2);           // Минимальное значение 0.5, Максимальное значение 1

            oldVal = stat.meat;
            stat.meat *= lossCoef;
            sumManager.meatDiff -= oldVal - stat.meat;

            oldVal = stat.fruits;
            stat.fruits *= lossCoef;
            sumManager.fruitsDiff -= oldVal - stat.fruits;

            oldVal = stat.wood;
            stat.wood *= lossCoef;
            sumManager.woodDiff -= oldVal - stat.wood;

            oldVal = stat.stone;
            stat.stone *= lossCoef;
            sumManager.stoneDiff -= oldVal - stat.stone;

            stat.happiness -= 5;

            sumManager.MakeEvent(7);
        }
    }

    // Стройка за ход
    private void Build(int points)
    {
        GameObject obj;

        for (int i = 0; i <= stat.objects.Count - 1; i++)
        {
            Building building = stat.objects[i].GetComponent<Building>();
            if (building.state == "build")
            {
                while (points > 0)
                {
                    building.buildPoints--;
                    points--;
                    if (building.buildPoints <= 0)
                    {
                        obj = stat.objects[i].GetComponent<BuildTrigger>().SetHouse();
                        sumManager.MakeEvent(11, 0, stat.objects[i].GetComponent<Building>().type_Rus);
                        Destroy(stat.objects[i]);
                        stat.objects.Remove(stat.objects[i]);
                        stat.objects.Add(obj);
                        i--; // Cause tail of objects array shifts left when we delete an object
                        break;
                    }
                }
            }
        }
    }

    // Случайные события
    private void RandomEvents() {
        float happinessCoef = 0f, chance;
        int maxGrowth, newPeople, number, amount;
        float food = stat.meat + stat.fruits;
        float foodCoef = food / stat.population;
        System.Random rand = new System.Random();

        if (stat.happiness > 0f)                                          // happinessCoef больше 0f
            happinessCoef = (float)stat.happiness / (float)100;
        if (stat.happiness <= 0f)
            happinessCoef = 0f;

        // Приход беженцев
        newPeople = 0;
        maxGrowth = 3; //  раньше было (stat.population / 10) + 1
        chance = stat.happiness * 0.5f * ((50f - stat.turn) / 50f); // Макс. шанс = 50%. С каждым ходом шанс падает ЛИНЕЙНО.
        chance = Mathf.Clamp(chance, 0f, 100f);                           // На 50м ходе и дальше шанс = 0%
        if (rand.Next(100) < chance) {
            newPeople = (rand.Next(100) / (100 / maxGrowth)) + 1;         // newPeople равно значению от 1 до maxGrowth
        }
        stat.homeless += newPeople;
        if (newPeople > 0)
            sumManager.MakeEvent(0, newPeople);

        // Рождение людей
        newPeople = 0;
        maxGrowth = (stat.population / 20) + 1;
        if (foodCoef < 1.5) {
            chance = 0f;
        }
        else {
            chance = (foodCoef / 10f) * 75f + 25f;     // От 25% до 100%
        }
        if (rand.Next(100) < chance) {
            newPeople = (rand.Next(100) / (100 / maxGrowth)) + 1;
        }
        stat.homeless += newPeople;
        if (newPeople > 0)
            sumManager.MakeEvent(1, newPeople);

        // Смерть поселенцев от болезни
        newPeople = 0;
        maxGrowth = (stat.population / 15) + 1;
        chance = 100 * (70 - stat.happiness) / 40f;  // При 30, chance = 100%, При 70, chance = 0%
        chance = Mathf.Clamp(chance, 0f, 100f);
        if (rand.Next(100) < chance) {
            newPeople = (rand.Next(100) / (100 / maxGrowth)) + 1;
        }
        if (newPeople > 0)
            sumManager.MakeEvent(3, newPeople);
        for (int i = 0; i < newPeople; i++) {                             // Обработка смерти каждого человека
            number = rand.Next(1, stat.population);
            if (number <= stat.workless) {
                stat.workless--;
            }
            if (number > stat.workless && number <= (stat.workless + stat.guards)) {
                stat.guards--;
            }
            if (number > (stat.workless + stat.guards)) {
                amount = number - stat.workless - stat.guards;
                foreach (GameObject item in stat.objects) {
                    Building building = item.GetComponent<Building>();
                    if (building.people > 0 && (building.type == "hunthut" || building.type == "buildhut" || building.type == "woodshack" || building.type == "mine")) {
                        if (amount > building.people) {
                            amount -= building.people;
                        }
                        else {
                            building.RemovePerson();
                            stat.workless--;
                            break;
                        }
                    }
                }
            }
            stat.population--;
            HutPurification();
        }

        // Нападение кочевников
        if (stat.turn <= 3)        // Первые n ходов кочевники не нападают
            chance = 0f;
        else
            chance = (foodCoef / 10f) * 50f;  // Раньше было 50f * ((float)stat.turn / ((float)stat.turn + (1000f / stat.turn)));  Теперь: При 0 chance = 0%, При 10 chance = 50%
        //chance = 100f; stat.turn = 5;
        if (stat.turn - fightManager.turnOfLastAttack > 3 && rand.Next(100) < chance) { 
            newPeople = (int)(stat.turn / 3f);
            NextTurn.fight = true;
            NextTurn.enemies = newPeople;
            //gameObject.GetComponent<FightManager>().SetBattle(newPeople); // Это нужно вызвать из NextTurn, когда пройдет половина суток
        }
    }

    // Корректировка счетчика счастья после битвы. 
    // Эта функция вызывается из NextTurn после того, как завершился цикл смены хода
    public void AddHappinessAfterBattle() {
        if (stat.warAddition > 5) {
            stat.warAddition -= 5;
        }
        else if (stat.warAddition < -5) {
            stat.warAddition += 5;
        }
        else stat.warAddition = 0;

        if (stat.turn == sumManager.turnOfLastAttack) stat.warAddition -= (sumManager.guardDeaths + sumManager.militiaDeaths) * 10 * (1 / Mathf.Sqrt(stat.turn));
        if (sumManager.battleOutcome == "win") stat.warAddition += 20;
        if (sumManager.battleOutcome == "lose") stat.warAddition -= 20;

        stat.happiness += (int)stat.warAddition;
    }

    // --------------------------------------------- Основная функция TurnManager ---------------------------------------------------- //
    // Все события, которые происходят по нажатию большой кнопки "ХОД"
    public void ProcessTurn() {
        stat.turn++;

        GetResourses();
        EstablishHappiness();
        Eat();
        Loss();
        Build(stat.builders + 1);
        RandomEvents();

        if (stat.happiness <= 0)
        { 
            EndGame(0);
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------- //





    // Проверка: Пуста ли клетка
    public bool CheckEmpty(Vector3 pos) {
        if (cellState[(int)(pos.x / 2 + 0.01), (int)(pos.z / 2 + 0.01)].empty == true) return true;
        return false;
    }

    // Проверка: Является ли клетка дорогой
    public bool CheckIsRoad(Vector3 pos) {
        if (cellState[(int)(pos.x / 2), (int)(pos.z / 2)].road == true) return true;
        return false;
    }

    // Заполнение структуры stat
    private void FillStat(string mode) {
        switch (mode)
        {
            case "test1":
                stat.homeless = 0;
                stat.population = 16;

                stat.houseAmount = 10;
                stat.constructionAmount = 0;
                stat.hutAmount = 4; // 16 p
                stat.barnAmount = 1;
                stat.buildAmount = 1; // 4 p
                stat.woodAmount = 1; // 2 p
                stat.huntAmount = 1; // 3 p
                stat.wellAmount = 1;
                stat.mineAmount = 1; // 2 p

                stat.workless = 16;
                stat.builders = 0;
                stat.woodcutters = 0;
                stat.hunters = 0;
                stat.miners = 0;
                stat.guards = 0;

                stat.wood = 10;
                stat.stone = 10;
                stat.meat = 20;
                stat.fruits = 20;

                stat.happiness = 75;
                stat.starveAddition = 0;

                stat.turn = 1;
                break;
        }
    }

    // --------------------------------------------------- Функция начала --------------------------------------------------- //
    //                                                                                                                        //
    public void BeginTurnManager() {
        fightManager = GetComponent<FightManager>();
        gameData = GetComponent<GameData>();
        rayCaster = GetComponent<RayCaster>();
        stateManager = GetComponent<StateManager>();
        sumManager = canvas.transform.Find("TurnSummaryBox").Find("TurnSummary").GetComponent<SummaryManager>();

        // Создание и инициализирование массива CellState нулями
        cellState = new CellState[SIZE,SIZE];
        for (int i = 0; i < SIZE; i++) {
            for (int j = 0; j < SIZE; j++) {
                cellState[i, j].empty = true;
                cellState[i, j].road = false;
            }
        }
        
        // Создание структуры Stat
        stat = new Stat();
        stat.objects = new List<GameObject>();
        stat.objCounter = 100;

        // Заполнение структур и массивов данными начального поселения
        GameObject[] startObjects;
        startObjects = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject item in startObjects) {
            stat.objects.Add(item);
            cellState[(int)item.transform.position.x / 2, (int)item.transform.position.z / 2].empty = false;
            cellState[(int)item.transform.position.x / 2, (int)item.transform.position.z / 2].road = true;
        }

        // Превращение buildset в constructions и готовые здания
        startObjects = GameObject.FindGameObjectsWithTag("BuildSet");
        foreach (GameObject item in startObjects) {
            item.GetComponent<BuildsetScript>().RememberPlace();
            item.GetComponent<BuildsetScript>().MakeBuilding(item.transform.position, true);
            Destroy(item);
        }

        // Превращение construction в готовые здания
        startObjects = GameObject.FindGameObjectsWithTag("Construction");
        foreach (GameObject item in startObjects) {
            GameObject obj = item.GetComponent<BuildTrigger>().SetHouse();
            stat.objects.Remove(item);
            stat.objects.Add(obj);
            Destroy(item);
        }

        // Заполнение всех пустых жилых хижин
        startObjects = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject item in startObjects) { 
            if (item.GetComponent<Building>().type == "settlehut") {
                item.GetComponent<Building>().people = item.GetComponent<Building>().maxPeople;
            }
        }

        GameObject.Find("Canvas").GetComponent<BuildingInfoTrigger>().TurnOffTF2Color();

        FillStat("test1");
    }
}
*/
