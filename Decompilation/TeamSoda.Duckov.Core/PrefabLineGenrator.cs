using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014E RID: 334
[RequireComponent(typeof(Points))]
[ExecuteInEditMode]
public class PrefabLineGenrator : MonoBehaviour, IOnPointsChanged
{
	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0002DA93 File Offset: 0x0002BC93
	private List<Vector3> originPoints
	{
		get
		{
			return this.points.points;
		}
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
	public void OnPointsChanged()
	{
	}

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private float prefabLength = 2f;

	// Token: 0x0400090F RID: 2319
	public PrefabLineGenrator.SapwnInfo spawnPrefab;

	// Token: 0x04000910 RID: 2320
	public PrefabLineGenrator.SapwnInfo startPrefab;

	// Token: 0x04000911 RID: 2321
	public PrefabLineGenrator.SapwnInfo endPrefab;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private Points points;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	[HideInInspector]
	private List<BoxCollider> colliderObjects;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private float updateTick = 0.5f;

	// Token: 0x04000915 RID: 2325
	private float lastModifyTime;

	// Token: 0x04000916 RID: 2326
	private bool dirty;

	// Token: 0x04000917 RID: 2327
	public List<Vector3> searchedPointsLocalSpace;

	// Token: 0x020004A6 RID: 1190
	[Serializable]
	public struct SapwnInfo
	{
		// Token: 0x060026BE RID: 9918 RVA: 0x0008B504 File Offset: 0x00089704
		public GameObject GetRandomPrefab()
		{
			if (this.prefabs.Count < 1)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < this.prefabs.Count; i++)
			{
				num += this.prefabs[i].weight;
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			for (int j = 0; j < this.prefabs.Count; j++)
			{
				if (num2 <= this.prefabs[j].weight)
				{
					return this.prefabs[j].prefab;
				}
				num2 -= this.prefabs[j].weight;
			}
			return this.prefabs[this.prefabs.Count - 1].prefab;
		}

		// Token: 0x04001C33 RID: 7219
		public List<PrefabLineGenrator.PrefabPair> prefabs;

		// Token: 0x04001C34 RID: 7220
		public float rotateOffset;

		// Token: 0x04001C35 RID: 7221
		[Range(0f, 1f)]
		public float flatten;

		// Token: 0x04001C36 RID: 7222
		public Vector3 posOffset;
	}

	// Token: 0x020004A7 RID: 1191
	[Serializable]
	public struct PrefabPair
	{
		// Token: 0x04001C37 RID: 7223
		public GameObject prefab;

		// Token: 0x04001C38 RID: 7224
		public float weight;
	}
}
