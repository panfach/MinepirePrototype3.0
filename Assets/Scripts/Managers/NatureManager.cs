using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureManager : MonoBehaviour
{
    public static List<Nature> natures = new List<Nature>();

    public float treePerturbStrength, sizePerturbStrength, anglePerturbStrength;
    public Texture2D noiseSource;

    public void AddItem(Vector3 point, NatureIndex index)
    {
        SCCoord coord = SCCoord.FromPos(point);
        AddItem(coord, index);
    }

    public void AddItem(SCCoord coord, NatureIndex index)
    {
        GameObject item = Instantiate
        (
            DataList.GetNatureObj(index), 
            Perturb(SCCoord.GetCorner(coord, SCCoord.GetHeight(coord))),
            Quaternion.identity,
            Connector.environmentSpawnedObjects.transform
        );

        Nature itemScript = item.GetComponent<Nature>();
        itemScript.GridObject.coordinates = coord;

        if (!itemScript.GridObject.CheckPlaceAvailability())                                        // In future "AddItem" shouldn't contain checking availability
        {
            Destroy(item);
            return;
        }

        itemScript.NtrProp.mainModel.Rotate(CellMetrics.Yaxis, AnglePerturb(SCCoord.GetCenter(coord), 180f));
        //itemScript.mainModel.localScale = new Vector3(1f, SizePerturb(SCCoord.GetCenter(coord), 1.3f), 1f);

        itemScript.GridObject.OccupyPlace();
        //SmallCellGrid.OccupyPlaceWithResource(coord, index);
        natures.Add(itemScript);

        //Connector.dynamicGameCanvas.SpawnInfo(itemScript);
        //itemScript.SetSmallInfo();
        /*if (*//*true*//* itemScript.data.index == NatureIndex.POPLAR)
        {
            //Debug.Log("Added to static batching list");
            VillageData.staticBatchingObjects.Add(item);                       // Collecting objects for refreshing of static batching 
        }*/
    }

    public void RemoveItem(Nature item)
    {
        natures.Remove(item);
    }


    public void ClearEnvironment()
    {
        foreach (Nature nature in natures)
        {
            nature.Die();
        }
        natures.Clear();
    }

    Vector3 Perturb(Vector3 position)
    {
        Vector4 sample = CellMetrics.SampleNoise(position);
        position.x += (sample.x * 2f - 1f) * treePerturbStrength;
        position.z += (sample.z * 2f - 1f) * treePerturbStrength;
        return position;
    }

    float SizePerturb(Vector3 position, float y)
    {
        Vector4 sample = CellMetrics.SampleNoise(position);
        y += (sample.y * 2f - 1f) * sizePerturbStrength;
        return y;
    }

    float AnglePerturb(Vector3 position, float z)
    {
        Vector4 sample = CellMetrics.SampleNoise(position);
        z += (sample.z * 2f - 1f) * anglePerturbStrength;
        return z;
    }
}
