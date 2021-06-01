using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class Animal_old : MonoBehaviour
{
   /* public static GameObject selectionPlanePrefab;
    const float angleControlDelay = 0.1f;

    [Header("Settings")]
    public float maxWalkingDelayTime;

    [Header("Data (Scriptable Object)")]
    public AnimalData data;

    [Header("Links")]
    public NavMeshAgent agent;
    public SmallAnimalInfo smallInfo;

    [Header("Others")]
    public bool gender;
    public float age;
    [SerializeField] float health;
    public AnimalState state;
    public bool MouseOver { get; private set; }
    public bool deletionFlag;
    public GameObject[] droppedItems;
    public GameObject lastAttacker;

    static float villagerHeight = 0.4f;
    static Vector3 villagerHeightVector = new Vector3(0f, villagerHeight / 2, 0f);
    Vector3 homeDirection, dest, oldPos;
    GameObject selectionPlane;
    Transform _transform;

    // -------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        StartCoroutine(RandomWalk());
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        MouseOver = true;
        selectionPlane = Instantiate(selectionPlanePrefab, _transform);
        selectionPlane.transform.position += new Vector3(0f, -0.2f, 0f);
        selectionPlane.transform.localScale *= 4.0f;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //Connector.panelInvoker.OpenVillagerInfo(this);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        *//*if (silhouette == null)
            silhouette = Instantiate(silhouettePrefab, _transform.position, Quaternion.identity);
        if (!StateManager.VillagerDragging)
            Building.SetAllSmallInfo(true);
        StateManager.VillagerDragging = true;*//*
    }

    private void OnMouseUp()
    {
        *//*data.TryToSettleByDragging();
        Building.SetAllSmallInfo(false);
        Building.OpenSmallInfoByMouse();
        Destroy(silhouette);
        StateManager.VillagerDragging = false;*//*
    }

    private void OnMouseExit()
    {
        MouseOver = false;
        if (selectionPlane != null) Destroy(selectionPlane);
    }

    // -------------------------------------------------------------------------------------------------- //

    public void Init(bool _gender, float _age)
    {
        gender = _gender;
        age = _age;

        SetHealth = data.maxHealth;
    }

    public float GetHealth() => health;

    public float SetHealth
    {
        set
        {
            health = value;
            if (health <= 0f)
            {
                Die(lastAttacker);
            }
            if (health > data.maxHealth)
            {
                health = data.maxHealth;
            }
            if (smallInfo != null && smallInfo.enabled) smallInfo.Refresh();
        }
    }

    // -------------------------------------------------------------------------------------------------- //

    public void DefineBehaviour()
    {
        StopAllCoroutines();
        state = AnimalState.NONE;

        StartCoroutine(RandomWalk());
    }

    public void GetDamage(GameObject sender, float damage)
    {
        if (sender.tag == "Villager") lastAttacker = sender;
        SetHealth = health - damage;
    }

    void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);

        //DefineAngle(dest);
    }

    void DefineAngle(Vector3 dest)
    {
        //Debug.Log("DefineAngle(Vector3 dest) of dest = " + dest);
        float angle = 360f * Mathf.Atan(dest.x / dest.z) / (2 * Mathf.PI);
        if (!double.IsNaN(angle))
        {
            if (dest.z < 0)
            {
                angle += 180f;
            }

            //Debug.Log("DefineAngle.angle = " + angle);
            //_transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            LeanTween.rotateLocal(gameObject, new Vector3(0f, angle, 0f), angleControlDelay);
        }
    }

    public void SetSmallInfo(bool state) ///////////////////////////////
    {
        if (state)
        {
            smallInfo.gameObject.SetActive(true);
            smallInfo.Refresh();
        }
        else
        {
            smallInfo.gameObject.SetActive(false);
        }
    }

    // -------------------------------------------------------------------------------------------------- //

    IEnumerator AngleControl()
    {
        oldPos = _transform.position;
        Vector3 dest;

        while (true)
        {
            yield return new WaitForSeconds(angleControlDelay);

            dest = _transform.position - oldPos;

            if (dest.sqrMagnitude > 0.001f) DefineAngle(dest);
            oldPos = _transform.position;
        }
    }

    IEnumerator RandomWalk()
    {
        StartCoroutine(AngleControl());
        state = AnimalState.RNDWALK;

        while (true)
        {
            float delayTime = Random.Range(1f, maxWalkingDelayTime);
            yield return new WaitForSeconds(delayTime);

            dest = _transform.position;
            dest += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            dest.y = SCCoord.GetHeight(dest);
            SetDestination(dest);
        }
    }

    // -------------------------------------------------------------------------------------------------- //

    public void Die(GameObject killer)
    {
        StopAllCoroutines();
        deletionFlag = true;
        AnimalManager.population--;
        smallInfo.Delete();
        VillageData.deletedAnimalsQueue.Add(this);
        Connector.resourceManager.DropItems(gameObject, "Animal");
        killer.GetComponent<Villager>()?.SetFutureDestObj(droppedItems);

        agent.enabled = false;
        transform.position = CellMetrics.hidedObjects;
        gameObject.SetActive(false);
    }

    public void Die()
    {
        StopAllCoroutines();
        deletionFlag = true;
        AnimalManager.population--;
        smallInfo.Delete();
        VillageData.deletedAnimalsQueue.Add(this);

        agent.enabled = false;
        transform.position = CellMetrics.hidedObjects;
        gameObject.SetActive(false);
    }

    public void SelfDeletion() { Destroy(gameObject); }*/
}
