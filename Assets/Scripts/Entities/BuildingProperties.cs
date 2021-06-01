using System.Collections.Generic;
using UnityEngine;

public class BuildingProperties : MonoBehaviour                                  
{
    public static int maxUniqueIndex = 1;
    public static string[] tags = { "BuildSet", "ConstructionSet", "Building" };
    public static ConstructionMode constructionMode = ConstructionMode.ORD;
    public static BuildingType[] livingBuildings = { BuildingType.LIVING };
    public static BuildingType[] workBuildings = { BuildingType.HUNT };

    [Header("Entity")]
    public Entity entity;

    [Header("Other info")]
    public bool deletionFlag;                                                        // Maybe it's not necessary, or it should be in Entity
    [SerializeField] int uniqueIndex;

    public int UniqueIndex { get => uniqueIndex; }


    public void AssignUniqueIndex(int index = 0)                                     // Maybe create separate class "UniqueIndex" with methods "Get", "Assign"
    {
        uniqueIndex = (index == 0) ? ++maxUniqueIndex : index;
    }

    public override string ToString()
    {
        return $"{entity.BldData.Name} Building";
    }
}
