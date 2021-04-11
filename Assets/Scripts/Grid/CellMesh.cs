using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] // Добавляет дополнительные скрипты объекту при нацеплении этого скрипта на объект
public class CellMesh : MonoBehaviour
{
    Mesh cellMesh;
	MeshCollider meshCollider;
    List<Vector3> vertices;
    List<int> triangles;
	List<Color> colors;

	void Awake()
	{
		GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		cellMesh.name = "Cell Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
	}

	public void Triangulate(Cell[] cells)
	{
		cellMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		colors.Clear();
		for (int i = 0; i < cells.Length; i++)
		{
			Triangulate(cells[i]);
		}
		cellMesh.vertices = vertices.ToArray();
		cellMesh.triangles = triangles.ToArray();
		cellMesh.colors = colors.ToArray();
		cellMesh.RecalculateNormals();

		meshCollider.sharedMesh = cellMesh;
		meshCollider.material = CellGrid.physMat;
	}

	void Triangulate(Cell cell)
	{
		for (CellDirection d = CellDirection.N; d <= CellDirection.W; d+=2)
		{
			Triangulate(d, cell);
		}
	}

	void Triangulate(CellDirection direction, Cell cell)
	{
		Vector3 center = cell.Position;

		EdgeVertices e = new EdgeVertices(
			center + CellMetrics.GetFirstSolidCorner(direction),
			center + CellMetrics.GetSecondSolidCorner(direction)
		);

		TriangulateEdgeFan(center, e, cell.Color);

		if (direction <= CellDirection.E)
		{
			TriangulateConnection(direction, cell, e);
		}
	}

	void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color)
	{
		AddTriangle(center, edge.v1, edge.v2);
		AddTriangleColor(color);
		AddTriangle(center, edge.v2, edge.v3);
		AddTriangleColor(color);
		AddTriangle(center, edge.v3, edge.v4);
		AddTriangleColor(color);
	}

	void TriangulateEdgeStrip(EdgeVertices e1, Color c1, EdgeVertices e2, Color c2)
	{
		AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
		AddQuadColor(c1, c2);
		AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
		AddQuadColor(c1, c2);
		AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
		AddQuadColor(c1, c2);
	}

	void TriangulateConnection(CellDirection direction, Cell cell, EdgeVertices e1)
	{
		Cell neighbor = cell.GetNeighbor(direction);
		if (neighbor == null)
		{
			return;
		}

		Vector3 bridge = CellMetrics.GetBridge(direction);
		bridge.y = neighbor.Position.y - cell.Position.y;
		EdgeVertices e2 = new EdgeVertices(
			e1.v1 + bridge,
			e1.v4 + bridge
		);

		TriangulateEdgeStrip(e1, cell.Color, e2, neighbor.Color);

		Cell prevPrevNeighbor = cell.GetNeighbor(direction.Prev().Prev()) ?? cell;
		Cell prevNeighbor = cell.GetNeighbor(direction.Prev());
		Cell nextNeighbor = cell.GetNeighbor(direction.Next());
		Cell nextNextNeighbor = cell.GetNeighbor(direction.Next().Next()) ?? cell;

		Vector3 v5;

		if (nextNeighbor != null)
		{
			v5 = cell.transform.localPosition + CellMetrics.GetSecondCorner(direction);
			v5.y = (cell.transform.localPosition.y + neighbor.transform.localPosition.y + 
				nextNeighbor.transform.localPosition.y + nextNextNeighbor.transform.localPosition.y) / 4;

			AddTriangle(e1.v4, e2.v4 , v5);
			AddTriangleColor(
				cell.Color, 
				neighbor.Color, 
				(cell.Color + neighbor.Color + nextNeighbor.Color + nextNextNeighbor.Color) / 4f
			);
		}

		if (prevNeighbor != null)
		{
			v5 = cell.transform.localPosition + CellMetrics.GetFirstCorner(direction);
			v5.y = (cell.transform.localPosition.y + neighbor.transform.localPosition.y +
				prevNeighbor.transform.localPosition.y + prevPrevNeighbor.transform.localPosition.y) / 4;

			AddTriangle(e1.v1, v5, e2.v1);
			AddTriangleColor(
				cell.Color,
				(cell.Color + neighbor.Color + prevNeighbor.Color + prevPrevNeighbor.Color) / 4f,
				neighbor.Color
			);
		}
	}

	void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(Perturb(v1));
		vertices.Add(Perturb(v2));
		vertices.Add(Perturb(v3));
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
		int vertexIndex = vertices.Count;
		vertices.Add(Perturb(v1));
		vertices.Add(Perturb(v2));
		vertices.Add(Perturb(v3));
		vertices.Add(Perturb(v4));
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	void AddTriangleColor(Color c1, Color c2, Color c3)
	{
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
	}

	void AddTriangleColor(Color c)
    {
		colors.Add(c);
		colors.Add(c);
		colors.Add(c);
	}

	void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
	{
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
	}

	void AddQuadColor(Color c1, Color c2)
    {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}

	public static Vector3 Perturb(Vector3 position)
    {
		Vector4 sample = CellMetrics.SampleNoise(position);
		position.x += (sample.x * 2f - 1f) * CellMetrics.cellPerturbStrength;
		//position.y += (sample.y * 2f - 1f) * CellMetrics.cellPerturbStrength;
		position.z += (sample.z * 2f - 1f) * CellMetrics.cellPerturbStrength;
		return position;
    }

	public static Vector3 ReversePerturb(Vector3 position)
	{
		Vector4 sample = CellMetrics.SampleNoise(position);
		position.x -= (sample.x * 2f - 1f) * CellMetrics.cellPerturbStrength;
		//position.y += (sample.y * 2f - 1f) * CellMetrics.cellPerturbStrength;
		position.z -= (sample.z * 2f - 1f) * CellMetrics.cellPerturbStrength;
		return position;
	}
}
