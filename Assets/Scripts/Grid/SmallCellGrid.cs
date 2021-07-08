using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SmallCellGrid : MonoBehaviour
{
    public static int sizeX = CellMetrics.chunkSizeX * CellGrid.chunkCountX * 4 - 1;
    public static int sizeZ = CellMetrics.chunkSizeZ * CellGrid.chunkCountZ * 4 - 1;
    public static CellState[,] cellState;


    public void Awake()
    {
        Init();
    }


    public void Init()
    {
        cellState = new CellState[sizeX, sizeZ];
        CellStateDefaultInit();
        SetSlopes();
    }

    public void CellStateDefaultInit()
    {
        CellState.cellCounter = 0;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                cellState[i, j] = new CellState();
            }
        }
    }

    public void SetSlopes()
    {
        Cell cell1, cell2, cell3, cell4;
        bool coast;
        
        // Определение склонов вдоль оси х
        for (int i = 0; i < sizeX / 4; i++)
        {
            for (int j = 0; j < sizeZ / 4 + 1; j++)
            {
                cell1 = CCoord.GetCell(i, j);
                cell2 = CCoord.GetCell(i + 1, j);

                //Debug.Log($"{i} {j} and {i + 1} {j} : slope : {cell1.Elevation != cell2.Elevation}");
                if (cell1.Elevation != cell2.Elevation)
                {
                    coast = CheckCoastConditions(cell1, cell2);
                    for (int k = 1; k <= 3; k++)
                    {
                        cellState[4 * i + 3, 4 * j + 3 - k].slope = true;
                        cellState[4 * i + 3, 4 * j + 3 - k].coast = coast;
                    }
                    /*if (i != 0)
                    {
                        cellState[4 * i + 3, 4 * j + 3 - 4].slope = true;
                    }*/
                }
                
            }
        }

        // Определение склонов вдоль оси z
        for (int j = 0; j < sizeZ / 4; j++)
        {
            for (int i = 0; i < sizeX / 4 + 1; i++)
            {
                cell1 = CCoord.GetCell(i, j);
                cell2 = CCoord.GetCell(i, j + 1);

                //Debug.Log($"{i} {j} and {i + 1} {j} : slope : {cell1.Elevation == cell2.Elevation}");
                if (cell1.Elevation != cell2.Elevation)
                {
                    coast = CheckCoastConditions(cell1, cell2);
                    for (int k = 1; k <= 3; k++)
                    {
                        cellState[4 * i + 3 - k, 4 * j + 3].slope = true;
                        cellState[4 * i + 3 - k, 4 * j + 3].coast = coast;
                    }
                    /*if (j != 0)
                    {
                        cellState[4 * i + 3 - 4, 4 * j + 3].slope = true;
                    }*/
                }
            }
        }

        // Определение угловых склонов
        for (int i = 0; i < sizeX / 4; i++)
        {
            for (int j = 0; j < sizeZ / 4; j++)
            {
                cell1 = CCoord.GetCell(i, j);
                cell2 = CCoord.GetCell(i + 1, j);
                cell3 = CCoord.GetCell(i, j + 1);
                cell4 = CCoord.GetCell(i + 1, j + 1);

                //Debug.Log($"{i} {j} and {i + 1} {j} : slope : {cell1.Elevation == cell2.Elevation}");
                if (cell1.Elevation != cell2.Elevation || cell1.Elevation != cell3.Elevation || cell1.Elevation != cell4.Elevation)
                {
                    cellState[4 * i + 3, 4 * j + 3].slope = true;
                }
            }
        }
    }

    bool CheckCoastConditions(Cell cell1, Cell cell2)
    {
        return ((cell1.Elevation == 0 && cell2.Elevation == 1) || (cell1.Elevation == 1 && cell2.Elevation == 0));
    }

    public static bool CheckPlaceAvailability(Entity sender)
    {
        if (sender is Building) return CheckPlaceAvailabilityForBuilding(sender as Building);
        else if (sender is Nature) return CheckPlaceAvailabilityForNature(sender as Nature);
        else return false;
    }

    public static bool CheckPlaceAvailabilityForBuilding(Building building)
    {
        if (building.GridObject == null) return false;

        if (BuildingProperties.constructionMode == ConstructionMode.ORD)
        {
            bool enterAvialability = false;

            foreach (CellPointer pointer in building.GridObject.cellPointer)
            {
                if (pointer.EnterCellPointer)
                {
                    if (pointer.IsEnableForBuild) enterAvialability = true;
                }
                else
                {
                    if (!pointer.IsEnableForBuild)
                    {
                        Notification.Invoke(NotifType.PLACEBUILD);
                        return false;
                    }
                }
            }

            if (!enterAvialability)
            {
                Notification.Invoke(NotifType.PLACEBUILD);
                return false;
            }
        }

        return true;
    }

    public static bool CheckPlaceAvailabilityForNature(Nature nature)                                                
    {
        //SCCoord coord = SCCoord.FromPos(nature.transform.position);                                 
        NatureIndex index = nature.NtrData.Index;

        return CheckPlaceAvailabilityForNature(nature.GridObject.coordinates, index);
    }

    public static bool CheckPlaceAvailabilityForNature(SCCoord coord, NatureIndex index)                      // In future rewrite.
    {
        NatureIndex[] checkableIndex = GetResourcesFromNature(coord);

        if (cellState[coord.X, coord.Z].building || cellState[coord.X, coord.Z].enter) return false;
        if (checkableIndex.Length > 0 && !NatureData.CheckFriends(checkableIndex, index)) return false;

        return true;
    }

    public static void OccupyPlaceWithBuilding(SCCoord place, int[] size)
    {
        OccupyPlaceWithBuilding(place, size[0], size[1]);
    }

    public static void OccupyPlaceWithBuilding(SCCoord place, int sizeX = 1, int sizeZ = 1)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                cellState[place.X + i, place.Z + j].building = true;
                //cellState[place.X + i, place.Z + j].buildingRef =
            }
        }
    }

    public static void OccupyPlaceWithBuildingEnter(SCCoord place)
    {
        cellState[place.X, place.Z].enter = true;
    }

    public static void OccupyPlaceWithNature(SCCoord place, NatureIndex resource)
    {
        cellState[place.X, place.Z].resource.Add(resource);
    }

    public static void FreePlaceFromBuilding(SCCoord place)
    {
        cellState[place.X, place.Z].building = false;
        cellState[place.X, place.Z].buildingRef = null;
    }

    public static void FreePlaceFromBuildingEnter(SCCoord place)
    {
        cellState[place.X, place.Z].enter = false;
    }

    public static void FreePlaceFromNature(SCCoord place, NatureIndex resource)
    {
        cellState[place.X, place.Z].resource.Remove(resource);
    }

    public static bool CheckEmptyForBuilding(Vector3 pos)
    {
        SCCoord coord = SCCoord.FromPos(pos);
        return CheckEmptyForBuilding(coord);
    }

    public static bool CheckEmptyForBuilding(SCCoord place)
    {
        return !(cellState[place.X, place.Z].building || 
            cellState[place.X, place.Z].slope ||
            (cellState[place.X, place.Z].resource.Count > 0));
    }

    public static bool CheckEmptyForEnter(Vector3 pos)
    {
        SCCoord coord = SCCoord.FromPos(pos);
        return CheckEmptyForEnter(coord);
    }

    public static bool CheckEmptyForEnter(SCCoord place)
    {
        return !(cellState[place.X, place.Z].building ||
            cellState[place.X, place.Z].slope ||
            (cellState[place.X, place.Z].resource.Count > 0));
    }

    public static NatureIndex[] GetResourcesFromNature(SCCoord place)
    {
        return cellState[place.X, place.Z].resource.ToArray();
    }

    public static float GetHeightOfPoint(SCCoord coord)
    {
        Vector3 point = SCCoord.GetCenter(coord);
        return GetHeightOfPoint(point);
    }

    public static float GetHeightOfPoint(Vector3 point)
    {
        RaycastHit hit;
        if (Physics.Raycast(point + new Vector3(0f, 10f, 0f), -CellMetrics.Yaxis*100, out hit, 100) /*&& hit.collider.tag == "TerrainMesh"*/)
        {
            return hit.point.y;
        }
        else return 0f;
    }

    public static CellState GetNearestCoastCellState(Vector3 position)
    {
        CellState nearest = null;
        float sqrDistance, minSqrDistance = float.MaxValue;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                if (cellState[i, j].coast && (sqrDistance = Vector3.SqrMagnitude(SCCoord.GetCenter(cellState[i, j].coord) - position)) < minSqrDistance)
                {
                    nearest = cellState[i, j];
                    minSqrDistance = sqrDistance;
                }
            }
        }

        return nearest;
    }


    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                cellState[i, j].Save(writer);
            }
        }
    }

    public void Load(BinaryReader reader)
    {
        Init();

        // loading information about array "cellState" from savefile
        // But by design, it shouldn't store bool fields "building", "slope" etc. Firstly, info about natures at least (field "resource")
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                cellState[i, j].Load(reader);
            }
        }
    }
}

public class CellState
{
    public static int cellCounter = 0;

    public SCCoord coord;
    public bool building;
    public bool slope;
    public bool enter;
    public bool coast;
    public List<NatureIndex> resource;
    public GameObject buildingRef;

    public CellState()
    {    
        coord = new SCCoord(cellCounter / SmallCellGrid.sizeX, cellCounter % SmallCellGrid.sizeX);
        building = false;
        slope = false;
        enter = false;
        coast = false;
        resource = new List<NatureIndex>();
        buildingRef = null;

        cellCounter++;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(building);                                    // Transform theese bool values into ~boolarrays (examine c#)
        writer.Write(slope);                                       // There are detailed comments in method Load() ...

        writer.Write((byte)resource.Count);
        for (int i = 0; i < resource.Count; i++)
        {
            writer.Write((byte)resource[i]);
        }
    }

    public void Load(BinaryReader reader)
    {
        NatureIndex res_read;

        building = reader.ReadBoolean();                             // It's no need to store this data. While instant building field "building" turns to true
        reader.ReadBoolean();                                        // ??? it is for slopes? This code must be deleted

        byte resourceCount = reader.ReadByte();                      // It remains
        for (int i = 0; i < resourceCount; i++)                      
        {
            res_read = (NatureIndex)reader.ReadByte();
            Connector.natureManager.AddItem(coord, res_read);
            //Debug.Log($"SmallCellGrid.Load() resource[{i}] is loaded.");
        }
    }
}
