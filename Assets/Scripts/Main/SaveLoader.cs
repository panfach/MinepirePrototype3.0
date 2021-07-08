using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoader : MonoBehaviour
{
	//public static bool mapIsLoaded = true;
	public static SaveLoadState state = SaveLoadState.START;

	public CellGrid cellGrid;
	public SmallCellGrid smallCellGrid;

	[Header("General Game Saves")]
	public TextAsset save;

	public void Save(string filename)
	{
		filename = (filename == "") ? "noname.map" : filename + ".map";

		string path = Path.Combine(Application.persistentDataPath, filename);

		state = SaveLoadState.SAVING;
		using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
		{
			cellGrid.Save(writer);
			smallCellGrid.Save(writer);
			VillageData.Save(writer);
		}
		state = SaveLoadState.DEFAULT;
	}

	public void Load(string filename)
	{
		state = SaveLoadState.LOADING;
		using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
		{
			ClearGame();
			Sunlight.timeOfDay = 0.86f;
			Sunlight.theEndOfDay = false;
			cellGrid.Load(reader);
			//Connector.navMeshSurface.BuildNavMesh();
			smallCellGrid.Load(reader);
			Connector.navMeshSurface.BuildNavMesh();
			VillageData.Load(reader);
			InfoDisplay.Refresh();
			//StaticBatchingUtility.Combine(VillageData.staticBatchingObjects.ToArray(), Connector.environmentSpawnedObjects);
			//VillageData.staticBatchingObjects.Clear();
			//StartCoroutine(TurnOnStaticBatching());
			//mapIsLoaded = true;
			//StartCoroutine(CellGrid.NavMeshRefresh(0.001f));
		}
		state = SaveLoadState.DEFAULT;
	}

	public void LoadNewGame()
	{
		state = SaveLoadState.LOADING;
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
			//mapIsLoaded = true;
		}
		state = SaveLoadState.DEFAULT;
	}

	public static string[] GetLoadFilenames()
    {
		string[] names = Directory.GetFiles(Application.persistentDataPath);

		return names;
	}

	public void ClearGame()
    {
		Connector.natureManager.Clear();
		Connector.creatureManager.Clear();
		Connector.generalBuilder.Clear();
		Connector.creatureManager.Clear();
		Connector.statistics.Clear();
		VillageData.Clear();
	}


	IEnumerator TurnOnStaticBatching()
    {
		yield return new WaitForSeconds(1.0f);
		StaticBatchingUtility.Combine(VillageData.staticBatchingObjects.ToArray(), Connector.environmentSpawnedObjects);
		//VillageData.staticBatchingObjects.Clear();
	}


	void OnApplicationQuit()
    {
		state = SaveLoadState.EXITING;
	}
}

public enum SaveLoadState
{
	START,
	DEFAULT,
	LOADING,
	SAVING,
	EXITING
}
