using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000272 RID: 626
	public class MiniMapCenter : MonoBehaviour
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x0004910C File Offset: 0x0004730C
		public float WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00049114 File Offset: 0x00047314
		private void OnEnable()
		{
			MiniMapCenter.activeMiniMapCenters.Add(this);
			if (MiniMapCenter.activeMiniMapCenters.Count > 1)
			{
				if (MiniMapCenter.activeMiniMapCenters.Find((MiniMapCenter e) => e != null && e != this && e.gameObject.scene.buildIndex == base.gameObject.scene.buildIndex))
				{
					Debug.LogError("场景 " + base.gameObject.scene.name + " 似乎存在两个MiniMapCenter！");
				}
				return;
			}
			this.CacheThisCenter();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00049184 File Offset: 0x00047384
		private void CacheThisCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return;
			}
			Vector3 position = base.transform.position;
			instance.Cache(this);
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000491B4 File Offset: 0x000473B4
		private void OnDisable()
		{
			MiniMapCenter.activeMiniMapCenters.Remove(this);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000491C4 File Offset: 0x000473C4
		internal static Vector3 GetCenterOfObjectScene(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			return MiniMapCenter.GetCenter(sceneBuildIndex);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00049208 File Offset: 0x00047408
		internal static string GetSceneID(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return null;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.Find((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry == null)
			{
				return null;
			}
			return mapEntry.sceneID;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00049288 File Offset: 0x00047488
		internal static Vector3 GetCenter(int sceneBuildIndex)
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry != null)
			{
				return mapEntry.mapWorldCenter;
			}
			return instance.combinedCenter;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000492DF File Offset: 0x000474DF
		internal static Vector3 GetCenter(string sceneID)
		{
			return MiniMapCenter.GetCenter(SceneInfoCollection.GetBuildIndex(sceneID));
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000492EC File Offset: 0x000474EC
		internal static Vector3 GetCombinedCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			return instance.combinedCenter;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00049314 File Offset: 0x00047514
		private void OnDrawGizmosSelected()
		{
			if (this.WorldSize < 0f)
			{
				return;
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.WorldSize, 1f, this.WorldSize));
		}

		// Token: 0x04000E93 RID: 3731
		private static List<MiniMapCenter> activeMiniMapCenters = new List<MiniMapCenter>();

		// Token: 0x04000E94 RID: 3732
		[SerializeField]
		private float worldSize = -1f;
	}
}
