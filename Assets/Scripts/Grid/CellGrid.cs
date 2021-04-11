using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;
using System.Collections;

public class CellGrid: MonoBehaviour
{
	public static bool drawCoordinates = false;
	public static int chunkCountX = 10, chunkCountZ = 10;
	public static int cellCountX, cellCountZ;
	public static float xMax, zMax, xMin, zMin;
	public static PhysicMaterial physMat;

	public Cell[] cells;
	public Color defaultColor;
	public Cell cellPrefab;
	public Text cellLabelPrefab;
	public Texture2D noiseSource;
	public Color[] colors;
	public CellGridChunk chunkPrefab;

	CellGridChunk[] chunks;

	public int CellSizeX
    {
		get => cellCountX;
    }

	void Awake()
	{
		cellCountX = chunkCountX * CellMetrics.chunkSizeX;
		cellCountZ = chunkCountZ * CellMetrics.chunkSizeZ;
		xMax = chunkCountX * CellMetrics.chunkSizeX * (2f * CellMetrics.outerRadius);
		zMax = chunkCountZ * CellMetrics.chunkSizeZ * (2f * CellMetrics.outerRadius);
		xMin = CellMetrics.cellShift;
		zMin = CellMetrics.cellShift;
	}

    private void OnEnable()
    {
		CellMetrics.noiseSource = noiseSource;
		CellMetrics.colors = colors;
		CreateChunks();
		CreateCells();
	}

	private void Start()
	{
		//Connector.navMeshSurface.BuildNavMesh();
		//StartCoroutine(NavMeshRefresh(0.001f)); // Очень сомнительная строчка. Но только с ней навмеш работает
	}

	public static IEnumerator NavMeshRefresh(float delay)
    {
		yield return new WaitForSeconds(delay);
		Connector.navMeshSurface.BuildNavMesh();
	}

    void CreateChunks()
	{
		chunks = new CellGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++)
		{
			for (int x = 0; x < chunkCountX; x++)
			{
				CellGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells()
	{
		cells = new Cell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++)
		{
			for (int x = 0; x < cellCountX; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}

	public Cell GetCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		return CCoord.GetCell(position);
	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = x * 2 * CellMetrics.outerRadius + CellMetrics.outerRadius;
		position.y = 0f;
		position.z = z * 2 * CellMetrics.outerRadius + CellMetrics.outerRadius;

		Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
		//cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = new CCoord(x, z);
		cell.TerrainTypeIndex = 2;

		if (x > 0)
		{
			cell.SetNeighbor(CellDirection.W, cells[i - 1]);
		}
		if (z > 0)
        {
			cell.SetNeighbor(CellDirection.S, cells[i - cellCountX]);
        }
		if (x > 0 && z > 0)
        {
			cell.SetNeighbor(CellDirection.SW, cells[i - cellCountX - 1]);
        }
		if (x < cellCountX - 1 && z > 0)
		{
			cell.SetNeighbor(CellDirection.SE, cells[i - cellCountX + 1]);
		}

		if (drawCoordinates)
		{
			Text label = Instantiate<Text>(cellLabelPrefab);
			label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
			label.text = x.ToString() + "\n" + z.ToString();
			cell.uiRect = label.rectTransform;
		}
		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	void AddCellToChunk(int x, int z, Cell cell)
	{
		int chunkX = x / CellMetrics.chunkSizeX;
		int chunkZ = z / CellMetrics.chunkSizeZ;
		CellGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * CellMetrics.chunkSizeX;
		int localZ = z - chunkZ * CellMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * CellMetrics.chunkSizeX, cell);
	}

	public void Save(BinaryWriter writer)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			cells[i].Save(writer);
		}
	}

	public void Load(BinaryReader reader)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			cells[i].Load(reader);
		}
		for (int i = 0; i < chunks.Length; i++)
		{
			chunks[i].Refresh();
		}
		//StartCoroutine(NavMeshRefresh(0.001f));
	}
}
