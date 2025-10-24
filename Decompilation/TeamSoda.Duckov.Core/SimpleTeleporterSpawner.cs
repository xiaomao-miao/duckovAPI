using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B0 RID: 176
[RequireComponent(typeof(Points))]
public class SimpleTeleporterSpawner : MonoBehaviour
{
	// Token: 0x060005CA RID: 1482 RVA: 0x00019D18 File Offset: 0x00017F18
	private void Start()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
			if (this.points == null)
			{
				return;
			}
		}
		this.scene = SceneManager.GetActiveScene().buildIndex;
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00019D80 File Offset: 0x00017F80
	private void OnValidate()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
		}
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00019D9C File Offset: 0x00017F9C
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00019DB0 File Offset: 0x00017FB0
	public void StartCreate()
	{
		this.scene = SceneManager.GetActiveScene().buildIndex;
		int key = this.GetKey();
		object obj;
		if (!MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
		{
			MultiSceneCore.Instance.inLevelData.Add(key, true);
			this.Create();
			return;
		}
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x00019E08 File Offset: 0x00018008
	private void Create()
	{
		List<Vector3> randomPoints = this.points.GetRandomPoints(this.pairCount * 2);
		for (int i = 0; i < this.pairCount; i++)
		{
			this.CreateAPair(randomPoints[i * 2], randomPoints[i * 2 + 1]);
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00019E54 File Offset: 0x00018054
	private void CreateAPair(Vector3 point1, Vector3 point2)
	{
		SimpleTeleporter simpleTeleporter = this.CreateATeleporter(point1);
		SimpleTeleporter simpleTeleporter2 = this.CreateATeleporter(point2);
		simpleTeleporter.target = simpleTeleporter2.TeleportPoint;
		simpleTeleporter2.target = simpleTeleporter.TeleportPoint;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00019E89 File Offset: 0x00018089
	private SimpleTeleporter CreateATeleporter(Vector3 point)
	{
		SimpleTeleporter simpleTeleporter = UnityEngine.Object.Instantiate<SimpleTeleporter>(this.simpleTeleporterPfb);
		MultiSceneCore.MoveToActiveWithScene(simpleTeleporter.gameObject, this.scene);
		simpleTeleporter.transform.position = point;
		return simpleTeleporter;
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00019EB4 File Offset: 0x000180B4
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("SimpTeles_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400054C RID: 1356
	private int scene = -1;

	// Token: 0x0400054D RID: 1357
	[SerializeField]
	private int pairCount = 3;

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private SimpleTeleporter simpleTeleporterPfb;

	// Token: 0x0400054F RID: 1359
	[SerializeField]
	private Points points;
}
