using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт должен висеть на основном источнике света.
/// </summary>
public class Sunlight : MonoBehaviour
{
    public static float timeOfDay = 0.84f;
    public static bool theEndOfDay = false;

    /* * * * * * * * * *
     * 8:00       0.84 *
     * 12:00      0.00 *
     * 20:00      0.33 *
     * 00:00      0.50 *
     * * * * * * * * * */

    [Header("Options")]
    public float rotationDurationOfChangingTurn, daySpeed;
    public Vector3 rotationAxis;
    public DayNightChangeLightingPreset lightingPreset;

    bool isTurnChanging = false;
    float timeStart, currentAngle, lastAngle, smallRotationAngle;

    /*public static bool TheEndOfDay
    {
        get => theEndOfDay;
        set
        {
            theEndOfDay = value;
            if (value)
            {
                VillagerManager.DefineAllBehaviours();
            }
        }
    }*/

    private void Awake()
    {
        UpdateLighting(timeOfDay);

        // Начальная предустановка
        lastAngle = 360f * timeOfDay; 
        transform.Rotate(rotationAxis, -360f * (1 - timeOfDay), Space.World);
    }

    private void LateUpdate()
    {
        if (isTurnChanging)
        {
            UpdateLighting(timeOfDay);

            if (Time.time - timeStart > rotationDurationOfChangingTurn)
            {
                isTurnChanging = false;
                lastAngle = 360f * timeOfDay;

                TimeEvents.StartOfTheDay();

                //addGuardButton.SetActive(true);
                //sumManager.FillSummary();
                //turnManager.AddHappinessAfterBattle();
            }
        }
        else if (!theEndOfDay)
        {
            if (timeOfDay < 0.5f && timeOfDay > 0.33f)
            {
                theEndOfDay = true;
                TimeEvents.EndOfTheDay();
            }
            else
            {
                TimeUpdate();
                UpdateLighting(timeOfDay);
                DayRotate();
            }
        }
    }

    public void ChangeTurn()
    {
        if (!isTurnChanging && (StateManager.GeneralState == GameState.ORD))
        {
            theEndOfDay = false;

            // Выключение Building Info
            //if (stateManager.getShowInfoMode())
            //{
            //    trigger.TurnOff();
            //}

            // Скрыть кнопку
            //LeanTween.moveLocalY(button, HIDINGPOS, 0.2f);

            // Переключение режимов
            isTurnChanging = true;

            // Запоминаем время начала
            timeStart = Time.time;

            // Обработка сводки (убрать, очистить)
            //sumManager.HideSummary();
            //sumManager.ClearEvents();

            // Изменение timeOfDay и угла поворота освещения во время смены хода
            LeanTween.value(gameObject, LeanTweenUpdateTimeOfDay, timeOfDay, 0.84f, rotationDurationOfChangingTurn).setEaseInOutQuad();
            LeanTween.rotateAround(gameObject, rotationAxis, 180f, rotationDurationOfChangingTurn).setEaseInOutQuad();
        }
    }

    private void UpdateLighting(float time)
    {
        RenderSettings.ambientLight = lightingPreset.ambientColor.Evaluate(time);
        RenderSettings.fogColor = lightingPreset.fogColor.Evaluate(time);
        GetComponent<Light>().color = lightingPreset.directionalColor.Evaluate(time);
        GetComponent<Light>().intensity = lightingPreset.sunlightIntensity.Evaluate(time);
    }

    void LeanTweenUpdateTimeOfDay(float time)
    {
        timeOfDay = time;
    }

    void TimeUpdate()
    {
        timeOfDay += daySpeed * Time.deltaTime;
        if (timeOfDay >= 1f) timeOfDay -= 1f;
    }

    void DayRotate()
    {
        currentAngle = 360f * timeOfDay;
        smallRotationAngle = currentAngle - lastAngle;
        if (smallRotationAngle < 0) smallRotationAngle = 360f - smallRotationAngle;
        transform.Rotate(rotationAxis, smallRotationAngle, Space.World);

        lastAngle = currentAngle;
    }
}

/*public class NextTurn : MonoBehaviour
{
    private const float HIDINGPOS = 111f;
    private const float SHOWINGPOS = 7f;

    [Header("Game objects")]
    public GameObject gameManager;

    public GameObject button, cam, addGuardButton;
    TurnManager turnManager;
    public BuildingInfoTrigger trigger;
    public StateManager stateManager;
    public SummaryManager sumManager;
    public MusicManager musicManager;
    public SoundManager soundManager;

    public Transform dirLight;
    public float rotationDuration;
    static public bool fight;
    static public int enemies;
    public AnimationCurve sunCurve;

    private float intensity, rotationSpeed;
    static public bool isLightRotates = false;
    static double SinusMult;
    public float timeStart;

    [SerializeField] public Light directionalLight;
    [SerializeField] public LightingPreset preset;
    [SerializeField, Range(0, 1)] private float timeOfDay;

    // Дополнительно
    FightManager fightManager;

    private void Start()
    {
        turnManager = gameManager.GetComponent<TurnManager>();
        fightManager = gameManager.GetComponent<FightManager>();
    }

    public void ChangeTurn()
    {
        if (!isLightRotates && (stateManager.getOrdinaryMode() || stateManager.getShowInfoMode()))
        {
            // Выключение Building Info
            if (stateManager.getShowInfoMode())
            {
                trigger.TurnOff();
            }

            // Скрыть кнопку
            LeanTween.moveLocalY(button, HIDINGPOS, 0.2f);

            // Переключение режимов
            stateManager.OnChangeTurnMode();
            isLightRotates = true;
            fight = false;

            // Запоминаем время начала
            timeStart = Time.time;

            // Расчет скорости верчения (Во 2й версии скорость постоянна, а не связана с синусом)
            rotationSpeed = 360f / rotationDuration;

            // Обработка сводки (убрать, очистить)
            sumManager.HideSummary();
            sumManager.ClearEvents();

            // Обработка, собственно, хода (Должна возвращать int !№:""№:%?)
            turnManager.ProcessTurn();
        }
    }

    private void Update()
    {
        if (isLightRotates)
        {
            timeOfDay = (Time.time - timeStart) / rotationDuration;
            // Кручение солнца
            dirLight.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
            // Интенсивность солнца
            dirLight.GetComponent<Light>().intensity = sunCurve.Evaluate(timeOfDay);
            // Изменение цвета окружения
            UpdateLighting(timeOfDay);

            //Debug.Log(dirLight.eulerAngles.x);
            //Debug.Log(dirLight.eulerAngles.y);
            //Debug.Log(dirLight.eulerAngles.z);

            if (fight && Time.time - timeStart > rotationDuration / 2)
            {
                //dirLight.eulerAngles = new Vector3(50f, 0f, 0f);

                isLightRotates = false;
                fight = false;

                addGuardButton.SetActive(false);
                musicManager.StopPlaying();
                soundManager.StopPlaying();
                fightManager.SetBattle(enemies);
            }

            if (Time.time - timeStart > rotationDuration)
            {
                dirLight.eulerAngles = new Vector3(50f, 0f, 0f);

                isLightRotates = false;
                stateManager.OnOrdinaryMode();

                LeanTween.moveLocalY(button, SHOWINGPOS, 0.2f).setEaseInOutBack();

                addGuardButton.SetActive(true);
                sumManager.FillSummary();


                turnManager.AddHappinessAfterBattle();
            }
        }
    }

    private void UpdateLighting(float time)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(time);
        RenderSettings.fogColor = preset.fogColor.Evaluate(time);
        directionalLight.color = preset.directionalColor.Evaluate(time);
    }
}*/
