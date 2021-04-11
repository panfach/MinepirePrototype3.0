using UnityEngine;

public class GridObject : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool useBuildSet;
    [SerializeField] bool useCellPointers;
    public CellPointer[] cellPointer;                             // How can i make it private ?

    [Header("Data")]
    public SCCoord coordinates;                                       
    public BuildingAngle angle = new BuildingAngle(0);            // Should i make it private

    bool hasOccupiedPlace;

    //public SCCoord Coord { get => coordinates; set { coordinates = value; }}
    public CellPointer CellPointer(int ind) => cellPointer[ind];
    public int CellPointersAmount { get => cellPointer.Length; }


    private void Awake()
    {
        if (entity == null) entity = GetComponent<Entity>();
    }


    public Vector3 GetLocalCenter()
    {
        Vector3 pos;

        if (useBuildSet)
        {
            pos = entity.BuildSet.ModelPos;
            pos.y = transform.position.y;
            pos = transform.InverseTransformPoint(pos);
        }
        else
        {
            pos = entity.BldData.LocalCenter;
        }

        return pos;
    }

    public Vector3 GetCenter()
    {
        Vector3 pos;

        if (useBuildSet)
        {
            pos = entity.BuildSet.ModelPos;
            pos.y = transform.position.y;
        }
        else
        {
            pos = transform.position;
            Vector3 localCenter = entity.BldData.LocalCenter;
            
            switch (angle.Index)
            {
                case 0:
                    pos.x += localCenter.x;
                    pos.z += localCenter.z;
                    break;
                case 1:
                    pos.x += localCenter.z;
                    pos.z -= localCenter.x;
                    break;
                case 2:
                    pos.x -= localCenter.x;
                    pos.z -= localCenter.z;
                    break;
                case 3:
                    pos.x -= localCenter.z;
                    pos.z += localCenter.x;
                    break;
            }
        }

        return pos;
    }

    public void RefreshCellPointers()
    {
        if (!useCellPointers) return;

        for (int i = 0; i < cellPointer.Length; i++)
        {
            cellPointer[i].Refresh();
        }
    }

    public void HideCellPointers()
    {
        if (!useCellPointers) return;

        foreach (CellPointer item in entity.GridObject.cellPointer)
        {
            item.gameObject.SetActive(false);
        }
    }

    public bool CheckPlaceAvailability()
    {
        return SmallCellGrid.CheckPlaceAvailability(entity);
    }

    // //////////////////////////////////////////////////////////////// This is example, how to write checking function for availability
    //bool TryOccupyPlace()
    //{
    //    if (entity.GridObject == null) return false;

    //    if (BuildingProperties.constructionMode == ConstructionMode.ORD)
    //    {
    //        bool enterAvialability = false;
    //        foreach (CellPointer pointer in entity.GridObject.cellPointer)
    //        {
    //            if (pointer.enterCellPointer)
    //            {
    //                if (pointer.isEnableForBuild) enterAvialability = true;
    //            }
    //            else
    //            {
    //                if (!pointer.isEnableForBuild)
    //                {
    //                    Notification.Invoke(NotifType.PLACEBUILD);
    //                    return false;
    //                }
    //            }
    //        }

    //        if (!enterAvialability)
    //        {
    //            Notification.Invoke(NotifType.PLACEBUILD);
    //            return false;
    //        }
    //    }

    //    SCCoord coord = SCCoord.FromPos(GeneralBuilder.hitPointBuild + CellMetrics.smallCellCenterShift);
    //    int[] size = GeneralBuilder.buildModeObject.GetComponent<Building>().Data.Size;
    //    OccupyPlace();
    //    entity.GridObject.coordinates = coord;
    //    HideCellPointers();

    //    return true;
    //}

    public void OccupyPlace()                                     // Curve fuction
    {

        if (useCellPointers)
        {
            HideCellPointers();

            foreach (CellPointer item in cellPointer)
            {
                item.Occupy();                                                        // This fuction shouldn't check availability, In CellPointer fuction Free must define type (Building or Nature or ...)
            }
        }
        else if (entity is Nature)
        {
            SmallCellGrid.OccupyPlaceWithNature(coordinates, entity.NtrData.Index);
        }

        hasOccupiedPlace = true;
    }

    public void FreePlace()
    {
        if (!hasOccupiedPlace) return;

        if (useCellPointers)
        {
            foreach (CellPointer item in cellPointer)
            {
                item.Free();                                                          // In CellPointer fuction Free must define type (Building or Nature or ...)
            }
        }
        else
        {
            foreach (CellPointer item in cellPointer)                                 // Come up with method to ocupy place without cell pointers in future? (The main problem here is rotations)
            {
                item.Free();                                                       
            }
        }

        hasOccupiedPlace = false;
    }


    private void OnDisable()
    {
        FreePlace();
    }
}
