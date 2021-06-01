using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CellMapEditor : MonoBehaviour
{
	public CellGrid cellGrid;

	bool applyElevation = false;
	int activeElevation;
	int activeTerrainTypeIndex;
	NatureIndex applyNature;
	CreatureIndex applyCreature;

	void Awake()
	{
		SetTerrainTypeIndex(-1);
	}

    private void OnEnable()
    {
		//CellMetrics.colors = colors;
	}

    void Update()
	{
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit) && hit.collider.tag == "TerrainMesh")
		{
			//Debug.Log($"hit.point.y = {hit.point.y}, cell.position.y = {CellCoordinates.CellFromPosition(hit.point).transform.position.y}");
			if (hit.point.y - SCCoord.GetHeight(hit.point) < 0.01f)
			{
				EditCell(cellGrid.GetCell(hit.point));
				EditSmallCell(hit.point);
				EditCreatures(hit.point);
			}
		}
	}

	void EditCell(Cell cell)
	{
		if (activeTerrainTypeIndex >= 0)
		{
			cell.TerrainTypeIndex = activeTerrainTypeIndex;
		}
		if (applyElevation)
		{
			cell.Elevation = activeElevation;
		}
	}

	void EditSmallCell(Vector3 point)
    {
		if (applyNature != NatureIndex.NONE)
        {
			Connector.natureManager.AddItem(point, applyNature);
        }
    }

	void EditCreatures(Vector3 point)
    {
		if (applyCreature != CreatureIndex.NONE)
			Connector.creatureManager.SpawnRandomAnimal(point);
    }



	public void SetTerrainTypeIndex(int index)
	{
		activeTerrainTypeIndex = index;
	}

	public void SetApplyElevation(bool toggle)
	{
		applyElevation = toggle;
	}

	public void SetApplyAnimal(bool toggle)
    {
		if (toggle) applyCreature = CreatureIndex.DEER;
		else applyCreature = CreatureIndex.NONE;
    }

	public void SetElevation(float elevation)
	{
		activeElevation = (int)elevation;
	}

	public void SetResource(int index)
    {
		SetResource((NatureIndex)index);
    }

	public void SetResource(NatureIndex index)
    {
		applyNature = index;
    }
}