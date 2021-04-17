using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] string _name_rus;
    [SerializeField] BuildingIndex index;
    [SerializeField] int sizeX;
    [SerializeField] int sizeZ;
    [SerializeField] int constructionCost;
    [SerializeField] ResourceQuery resourceCost;
    //[SerializeField] int maxPeople;
    [SerializeField] BuildingType type;
    [SerializeField] Profession profession;

    public string Name { get => _name; }
    public string Name_rus { get => _name_rus; }
    public BuildingIndex Index { get => index; }
    public int SizeX { get => sizeX; }
    public int SizeZ { get => sizeZ; }
    public int ConstrCost { get => constructionCost; }
    public ResourceQuery ResourceCost { get => resourceCost; }
    //public int MaxPeople { get => maxPeople; }
    public BuildingType BldType { get => type; }
    public Profession Profession { get => profession; }

    public int[] Size
    {
        get
        {
            int[] size = new int[2];
            size[0] = sizeX; size[1] = sizeZ;
            return size;
        }
    }

    public Vector3 LocalCenter
    {
        get
        {
            return new Vector3(sizeX / 2f, 0f, sizeZ / 2f);
        }
    }
}
