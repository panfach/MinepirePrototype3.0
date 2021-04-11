using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGridChunk : MonoBehaviour
{
	Cell[] cells;

	CellMesh cellMesh;
	Canvas gridCanvas;

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		cellMesh = GetComponentInChildren<CellMesh>();

		cells = new Cell[CellMetrics.chunkSizeX * CellMetrics.chunkSizeZ];
	}

    private void Start()
    {
		Refresh();
	}

    public void AddCell(int index, Cell cell)
	{
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		if (CellGrid.drawCoordinates) cell.uiRect.SetParent(gridCanvas.transform, false);
	}

	public void Refresh()
	{
		cellMesh.Triangulate(cells);
		/*enabled = true;*/
	}

	/*void LateUpdate()
	{
		cellMesh.Triangulate(cells);
		enabled = false;
	}*/
}
