using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoader : MonoBehaviour
{
	public static bool mapIsLoaded = true;

	public CellGrid cellGrid;
	public SmallCellGrid smallCellGrid;

	[Header("General Game Saves")]
	public TextAsset save;

	public void Save(string filename)
	{
		filename = (filename == "") ? "noname.map" : filename + ".map";

		string path = Path.Combine(Application.persistentDataPath, filename);

		using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
		{
			cellGrid.Save(writer);
			smallCellGrid.Save(writer);
			VillageData.Save(writer);
		}


	}

	public void Load(string filename)
	{
		using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
		{
			ClearGame();
			Sunlight.timeOfDay = 0.86f;
			Sunlight.theEndOfDay = false;
			cellGrid.Load(reader);
			Connector.navMeshSurface.BuildNavMesh();
			smallCellGrid.Load(reader);
			VillageData.Load(reader);
			InfoDisplay.Refresh();
			//StaticBatchingUtility.Combine(VillageData.staticBatchingObjects.ToArray(), Connector.environmentSpawnedObjects);
			//VillageData.staticBatchingObjects.Clear();
			//StartCoroutine(TurnOnStaticBatching());
			mapIsLoaded = true;
			//StartCoroutine(CellGrid.NavMeshRefresh(0.001f));
		}
	}

	public void LoadNewGame()
	{
		//using (BinaryReader reader = new BinaryReader(new MemoryStream(save.bytes)))
		using (BinaryReader reader = new BinaryReader(new MemoryStream(save.bytes)))
		{
			ClearGame();
			Sunlight.timeOfDay = 0.86f;
			Sunlight.theEndOfDay = false;
			cellGrid.Load(reader);
			Connector.navMeshSurface.BuildNavMesh();
			smallCellGrid.Load(reader);
			VillageData.Load(reader);
			InfoDisplay.Refresh();
			StartCoroutine(TurnOnStaticBatching());
			mapIsLoaded = true;
		}
	}

	public static string[] GetLoadFilenames()
    {
		string[] names = Directory.GetFiles(Application.persistentDataPath);

		return names;
	}

	public void ClearGame()
    {
		Connector.natureManager.ClearEnvironment();
		Connector.creatureManager.Clear();
		Connector.generalBuilder.Clear();
		Connector.creatureManager.Clear();
		VillageData.Clear();
	}


	IEnumerator TurnOnStaticBatching()
    {
		yield return new WaitForSeconds(1.0f);
		StaticBatchingUtility.Combine(VillageData.staticBatchingObjects.ToArray(), Connector.environmentSpawnedObjects);
		//VillageData.staticBatchingObjects.Clear();
	}
}
