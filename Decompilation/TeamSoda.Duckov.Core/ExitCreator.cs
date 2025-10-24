using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000101 RID: 257
public class ExitCreator : MonoBehaviour
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000877 RID: 2167 RVA: 0x000259A1 File Offset: 0x00023BA1
	private int minExitCount
	{
		get
		{
			return LevelConfig.MinExitCount;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x000259A8 File Offset: 0x00023BA8
	private int maxExitCount
	{
		get
		{
			return LevelConfig.MaxExitCount;
		}
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x000259B0 File Offset: 0x00023BB0
	public void Spawn()
	{
		int num = UnityEngine.Random.Range(this.minExitCount, this.maxExitCount + 1);
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		List<ValueTuple<string, SubSceneEntry.Location>> list = new List<ValueTuple<string, SubSceneEntry.Location>>();
		foreach (SubSceneEntry subSceneEntry in MultiSceneCore.Instance.SubScenes)
		{
			foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
			{
				if (this.IsPathCompitable(location))
				{
					list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry.sceneID, location));
				}
			}
		}
		list.Sort(new Comparison<ValueTuple<string, SubSceneEntry.Location>>(this.compareExit));
		if (num > list.Count)
		{
			num = list.Count;
		}
		Vector3 vector;
		MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out vector);
		int num2 = Mathf.RoundToInt((float)list.Count * 0.8f);
		if (num > num2)
		{
			num2 = num;
		}
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(0, num2);
			num2--;
			ValueTuple<string, SubSceneEntry.Location> valueTuple = list[index];
			list.RemoveAt(index);
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(valueTuple.Item1);
			this.CreateExit(valueTuple.Item2.position, sceneInfo.BuildIndex, i);
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00025B38 File Offset: 0x00023D38
	private int compareExit([TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> a, [TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> b)
	{
		Vector3 a2;
		if (!MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out a2))
		{
			return -1;
		}
		Vector3 b2;
		if (!MiniMapSettings.TryGetMinimapPosition(a.Item2.position, a.Item1, out b2))
		{
			return -1;
		}
		Vector3 b3;
		if (!MiniMapSettings.TryGetMinimapPosition(b.Item2.position, b.Item1, out b3))
		{
			return -1;
		}
		float num = Vector3.Distance(a2, b2);
		float num2 = Vector3.Distance(a2, b3);
		if (num > num2)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00025BB4 File Offset: 0x00023DB4
	private bool IsPathCompitable(SubSceneEntry.Location location)
	{
		string path = location.path;
		int num = path.IndexOf('/');
		return num != -1 && path.Substring(0, num) == "Exits";
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00025BEC File Offset: 0x00023DEC
	private void CreateExit(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.exitPrefab, position, Quaternion.identity);
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.MoveToActiveWithScene(go, sceneBuildIndex);
		}
		this.SpawnMapElement(position, sceneBuildIndex, debugIndex);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00025C28 File Offset: 0x00023E28
	private void SpawnMapElement(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement").AddComponent<SimplePointOfInterest>();
		simplePointOfInterest.transform.position = position;
		if (MultiSceneCore.Instance != null)
		{
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			simplePointOfInterest.IsArea = false;
			simplePointOfInterest.ScaleFactor = 1f;
			string sceneID = SceneInfoCollection.GetSceneID(sceneBuildIndex);
			simplePointOfInterest.Setup(this.icon, this.exitNameKey, false, sceneID);
			SceneManager.MoveGameObjectToScene(simplePointOfInterest.gameObject, MultiSceneCore.MainScene.Value);
		}
	}

	// Token: 0x040007AB RID: 1963
	public GameObject exitPrefab;

	// Token: 0x040007AC RID: 1964
	[LocalizationKey("Default")]
	public string exitNameKey;

	// Token: 0x040007AD RID: 1965
	[SerializeField]
	private Sprite icon;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	private Color iconColor = Color.white;

	// Token: 0x040007AF RID: 1967
	[SerializeField]
	private Color shadowColor = Color.white;

	// Token: 0x040007B0 RID: 1968
	[SerializeField]
	private float shadowDistance;
}
