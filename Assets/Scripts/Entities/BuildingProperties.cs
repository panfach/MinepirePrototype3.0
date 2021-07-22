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
    [SerializeField] int uniqueIndex;

    public int UniqueIndex { get => uniqueIndex; }


    public void AssignUniqueIndex(int index = 0)                                     // Maybe create separate class "UniqueIndex" with methods "Get", "Assign"
    {
        uniqueIndex = (index == 0) ? ++maxUniqueIndex : index;
        VillageData.UniqIndexDict.Add(uniqueIndex, entity as Building);
    }

    public override string ToString()
    {
        return $"{entity.BldData.Name} Building";
    }
}
