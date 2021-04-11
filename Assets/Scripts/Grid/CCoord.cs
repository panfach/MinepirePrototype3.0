using UnityEngine;

[System.Serializable]
// Cell Coordinates
public struct CCoord
{
	public int X { get; private set; }
	public int Z { get; private set; }

	public CCoord(int x, int z)
	{
		X = x;
		Z = z;
	}

	public static CCoord FromOffsetCoord(int x, int z)
	{
		return new CCoord(x, z);
	}

	public static CCoord FromPos(Vector3 position)
	{
		int x = Mathf.RoundToInt( (position.x - CellMetrics.cellShift) / (CellMetrics.outerRadius * 2f) - 0.5f);
		int z = Mathf.RoundToInt( (position.z - CellMetrics.cellShift) / (CellMetrics.outerRadius * 2f) - 0.5f);

		x = Mathf.Clamp(x, 0, CellGrid.cellCountX - 1);
		z = Mathf.Clamp(z, 0, CellGrid.cellCountZ - 1);

		return new CCoord(x, z);
	}

	public static CCoord FromPos(SCCoord coord) // FromPosition
	{
		int x = coord.X / 4;
		int z = coord.Z / 4;

		return new CCoord(x, z);
	}

	public static Cell GetCell(Vector3 position) // CellFromPosition
	{
		int x = Mathf.RoundToInt((position.x - CellMetrics.cellShift) / (CellMetrics.outerRadius * 2f) - 0.5f);
		int z = Mathf.RoundToInt((position.z - CellMetrics.cellShift) / (CellMetrics.outerRadius * 2f) - 0.5f);

		x = Mathf.Clamp(x, 0, CellGrid.cellCountX - 1);
		z = Mathf.Clamp(z, 0, CellGrid.cellCountZ - 1);

		return Connector.cellGrid.cells[z * Connector.cellGrid.CellSizeX + x];
	}

	public static Cell GetCell(CCoord coord) // CellFromCoordinates
    {
		return Connector.cellGrid.cells[coord.Z * Connector.cellGrid.CellSizeX + coord.X];
    }

	public static Cell GetCell(int x, int z) // CellFromCoordinates
	{
		return Connector.cellGrid.cells[z * Connector.cellGrid.CellSizeX + x];
	}

	public override string ToString()
	{
		return "(" + X.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines()
	{
		return X.ToString() + "\n" + Z.ToString();
	}
}

[System.Serializable]
public struct SCCoord
{
	public int X { get; private set; }
	public int Z { get; private set; }

	public SCCoord(int x, int z)
	{
		X = x;
		Z = z;
	}

	public static SCCoord FromOffsetCoord(int x, int z)
	{
		return new SCCoord(x, z);
	}

	public static SCCoord FromPos(Vector3 position)
	{
		int x = Mathf.RoundToInt((position.x - CellMetrics.cellShift) / (CellMetrics.smallOuterRadius * 2f) - CellMetrics.smallOuterRadius);
		int z = Mathf.RoundToInt((position.z - CellMetrics.cellShift) / (CellMetrics.smallOuterRadius * 2f) - CellMetrics.smallOuterRadius);

		x = Mathf.Clamp(x, 0, SmallCellGrid.sizeX - 1);
		z = Mathf.Clamp(z, 0, SmallCellGrid.sizeZ - 1);

		return new SCCoord(x, z);
	}

	// returns left down corner (which x and z coordinate are the smallest)
	public static Vector3 GetCorner(SCCoord coord, float y = 0) // GetCornerOfSmallCell
    {
		Vector3 outp = new Vector3
		(
			(coord.X + 0.5f) * (CellMetrics.smallOuterRadius * 2f), y,
			(coord.Z + 0.5f) * (CellMetrics.smallOuterRadius * 2f)
		);

		return outp;
	}


	public static Vector3 GetCornerRotated(int angleIndex, SCCoord coord, float y = 0)
    {
		GetRotationShift(angleIndex, out int xShift, out int zShift);
		//Debug.Log("GetCornerRotated : angleIndex = " + angleIndex);

		Vector3 outp = new Vector3
		(
			(coord.X + 0.5f) * (CellMetrics.smallOuterRadius * 2f) + xShift, y,
			(coord.Z + 0.5f) * (CellMetrics.smallOuterRadius * 2f) + zShift
		);

		//Debug.Log("MAIN CORNER = " + outp);

		return outp;
	}

	public static Vector3[] GetCorners(SCCoord coord, float y = 0) // GetCornersOfSmallCell
	{
		Vector3 mainCorner = GetCorner(coord, y);

		Vector3[] outp = new Vector3[4]
		{
			mainCorner + new Vector3(0f, 0f, 2 * CellMetrics.smallOuterRadius),
			mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, 2 * CellMetrics.smallOuterRadius),
			mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, 0f),
			mainCorner
		};

		return outp;
    }

	public static Vector3[] GetCornersRotated(int angleIndex, SCCoord coord, float y = 0) // GetCornersOfSmallCell
	{
		Vector3 mainCorner = GetCornerRotated(angleIndex, coord, y);

		Vector3[] rotShifts = GetRotationShifts(angleIndex);
		Vector3[] outp = new Vector3[4]
		{
			mainCorner + new Vector3(rotShifts[0].x * 2 * CellMetrics.smallOuterRadius, 0f, rotShifts[0].z * 2 * CellMetrics.smallOuterRadius),
			mainCorner + new Vector3(rotShifts[1].x * 2 * CellMetrics.smallOuterRadius, 0f, rotShifts[1].z * 2 * CellMetrics.smallOuterRadius),
			mainCorner + new Vector3(rotShifts[2].x * 2 * CellMetrics.smallOuterRadius, 0f, rotShifts[2].z * 2 * CellMetrics.smallOuterRadius),
			mainCorner + new Vector3(rotShifts[3].x * 2 * CellMetrics.smallOuterRadius, 0f, rotShifts[3].z * 2 * CellMetrics.smallOuterRadius)
		};

		/*
		switch (angleIndex)
        {
			case (0):
				outp = new Vector3[4]
				{
					mainCorner + new Vector3(0f, 0f, 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, 0f),
					mainCorner
				};
				break;
			case (1):
				outp = new Vector3[4]
				{
					mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, 0f),
					mainCorner + new Vector3(2 * CellMetrics.smallOuterRadius, 0f, - 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(0f, 0f, - 2 * CellMetrics.smallOuterRadius),
					mainCorner
				};
				break;
			case (2):
				outp = new Vector3[4]
				{
					mainCorner + new Vector3(0f, 0f, - 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(- 2 * CellMetrics.smallOuterRadius, 0f, - 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(- 2 * CellMetrics.smallOuterRadius, 0f, 0f),
					mainCorner
				};
				break;
			case (3):
				outp = new Vector3[4]
				{
					mainCorner + new Vector3(- 2 * CellMetrics.smallOuterRadius, 0f, 0f),
					mainCorner + new Vector3(- 2 * CellMetrics.smallOuterRadius, 0f, 2 * CellMetrics.smallOuterRadius),
					mainCorner + new Vector3(0f, 0f, 2 * CellMetrics.smallOuterRadius),
					mainCorner
				};
				break;
			default:
				outp = null;
				break;
		}
		*/

		return outp;
	}

	public static void GetRotationShift(int index, out int xShift, out int zShift)
	{
		xShift = 0;
		zShift = 0;
		switch (index)
		{
			case 1:
				zShift = 1;
				break;
			case 2:
				xShift = 1;
				zShift = 1;
				break;
			case 3:
				xShift = 1;
				break;
		}
	}

	public static Vector3[] GetRotationShifts(int index)
    {
		Vector3[] outp = new Vector3[4];
		switch (index)
        {
			case 0:
				outp[0].z = 1f;
				outp[1].x = 1f;
				outp[1].z = 1f;
				outp[2].x = 1f;
				break;
			case 1:
				outp[0].x = 1f;
				outp[1].x = 1f;
				outp[1].z = -1f;
				outp[2].z = -1f;
				break;
			case 2:
				outp[0].z = -1f;
				outp[1].x = -1f;
				outp[1].z = -1f;
				outp[2].x = -1f;
				break;
			case 3:
				outp[0].x = -1f;
				outp[1].x = -1f;
				outp[1].z = 1f;
				outp[2].z = 1f;
				break;
        }

		return outp;
	}

	public static int[] RotateCellSize(int[] size)
    {
		if (size.Length != 2) return null;

		int[] outp = new int[2] { size[0], size[1] };

		switch(GeneralBuilder.buildingAngle.Index)
        {
			case 1:
				outp[0] = size[1];
				outp[1] = size[0];
				break;
			case 2:
				outp[0] = size[0];
				outp[1] = size[1];
				break;
			case 3:
				outp[0] = size[1];
				outp[1] = size[0];
				break;
        }

		return outp;
    }

	public static Vector3 GetCenter(SCCoord coord, float y = 0) // GetCenterOfSmallCell
	{
		Vector3 outp = GetCorner(coord, y);
		outp += new Vector3(CellMetrics.cellShift, 0f, CellMetrics.cellShift);

		return outp;
    }

	public static float GetHeight(Vector2 position)
    {
		SCCoord coord = FromPos(new Vector3(position.x, 0, position.y));
		return GetHeight(coord);
	}

	public static float GetHeight(Vector3 position)
    {
		SCCoord coord = FromPos(position);
		return GetHeight(coord);
    }

	public static float GetHeight(SCCoord smallCoord)
    {
		CCoord coord = CCoord.FromPos(smallCoord);
		return CCoord.GetCell(coord).transform.position.y;
    }

	public override string ToString()
	{
		return "(" + X.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines()
	{
		return X.ToString() + "\n" + Z.ToString();
	}
}

public enum CellDirection
{
	N, NE, E, SE, S, SW, W, NW
}

// Расширение enum'a
public static class CellDirectionExtensions
{
	public static CellDirection Opposite(this CellDirection direction)
	{
		return (int)direction < 4 ? (direction + 4) : (direction - 4);
	}

	public static CellDirection Prev(this CellDirection direction)
	{
		return direction == CellDirection.N ? CellDirection.NW : (direction - 1);
	}

	public static CellDirection Next(this CellDirection direction)
    {
		return direction == CellDirection.NW ? CellDirection.N : (direction + 1);
    }
}
