using UnityEngine;
using System.IO;

public class Cell : MonoBehaviour
{
    public CCoord coordinates;
    public RectTransform uiRect;
    public CellGridChunk chunk;

    [SerializeField] int elevation = int.MinValue;
    [SerializeField] int terrainTypeIndex;
    [SerializeField] Cell[] neighbors = null;

    public Color Color
    {
        get
        {
            return CellMetrics.colors[terrainTypeIndex];
        }
    }

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            if (elevation == value)
            {
                return;
            }

            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * CellMetrics.elevationStep;
            position.y += (CellMetrics.SampleNoise(position).y * 2f - 1f) * CellMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            if (CellGrid.drawCoordinates)
            {
                Vector3 uiPosition = uiRect.localPosition;
                uiPosition.z = -(position.y + 0.001f);
                uiRect.localPosition = uiPosition;
            }

            Refresh();
            
        }
    }

    public int TerrainTypeIndex
    {
        get
        {
            return terrainTypeIndex;
        }
        set
        {
            if (terrainTypeIndex != value)
            {
                terrainTypeIndex = value;
                Refresh();
            }
        }
    }

    public Cell GetNeighbor(CellDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(CellDirection direction, Cell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                Cell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)terrainTypeIndex);
        writer.Write((byte)elevation);
    }

    public void Load(BinaryReader reader)
    {
        terrainTypeIndex = reader.ReadByte();
        Elevation = reader.ReadByte();
    }
}

public struct EdgeVertices
{
    public Vector3 v1, v2, v3, v4;

    public EdgeVertices(Vector3 Corner1, Vector3 Corner2)
    {
        v1 = Corner1;
        v2 = Vector3.Lerp(Corner1, Corner2, 1f / 3f);
        v3 = Vector3.Lerp(Corner1, Corner2, 2f / 3f);
        v4 = Corner2;
    }
}
