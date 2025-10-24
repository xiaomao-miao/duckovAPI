using System;
using System.Collections.Generic;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x02000127 RID: 295
[CreateAssetMenu]
public class SceneInfoCollection : ScriptableObject
{
	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x0002998C File Offset: 0x00027B8C
	internal static SceneInfoCollection Instance
	{
		get
		{
			GameplayDataSettings.SceneManagementData sceneManagement = GameplayDataSettings.SceneManagement;
			if (sceneManagement == null)
			{
				return null;
			}
			return sceneManagement.SceneInfoCollection;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x0002999E File Offset: 0x00027B9E
	public static List<SceneInfoEntry> Entries
	{
		get
		{
			if (SceneInfoCollection.Instance == null)
			{
				return null;
			}
			return SceneInfoCollection.Instance.entries;
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x000299BC File Offset: 0x00027BBC
	public SceneInfoEntry InstanceGetSceneInfo(string id)
	{
		return this.entries.Find((SceneInfoEntry e) => e.ID == id);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000299F0 File Offset: 0x00027BF0
	public string InstanceGetSceneID(int buildIndex)
	{
		SceneInfoEntry sceneInfoEntry = this.entries.Find((SceneInfoEntry e) => e != null && e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == buildIndex);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.ID;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00029A2D File Offset: 0x00027C2D
	internal string InstanceGetSceneID(SceneReference sceneRef)
	{
		if (sceneRef.UnsafeReason != SceneReferenceUnsafeReason.None)
		{
			return null;
		}
		return this.InstanceGetSceneID(sceneRef.BuildIndex);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00029A48 File Offset: 0x00027C48
	internal SceneReference InstanceGetSceneReferencce(string requireSceneID)
	{
		SceneInfoEntry sceneInfoEntry = this.InstanceGetSceneInfo(requireSceneID);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.SceneReference;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00029A68 File Offset: 0x00027C68
	public static SceneInfoEntry GetSceneInfo(string sceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneInfo(sceneID);
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00029A84 File Offset: 0x00027C84
	public static string GetSceneID(SceneReference sceneRef)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(sceneRef);
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00029AA0 File Offset: 0x00027CA0
	public static string GetSceneID(int buildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(buildIndex);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00029ABC File Offset: 0x00027CBC
	internal static int GetBuildIndex(string overrideSceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return -1;
		}
		SceneInfoEntry sceneInfoEntry = SceneInfoCollection.Instance.InstanceGetSceneInfo(overrideSceneID);
		if (sceneInfoEntry == null)
		{
			return -1;
		}
		return sceneInfoEntry.BuildIndex;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00029AF0 File Offset: 0x00027CF0
	internal static SceneInfoEntry GetSceneInfo(int sceneBuildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.entries.Find((SceneInfoEntry e) => e.BuildIndex == sceneBuildIndex);
	}

	// Token: 0x0400086D RID: 2157
	public const string BaseSceneID = "Base";

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private List<SceneInfoEntry> entries;
}
