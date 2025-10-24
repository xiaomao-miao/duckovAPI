using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033C RID: 828
	public class MapElementForTask : MonoBehaviour
	{
		// Token: 0x06001C87 RID: 7303 RVA: 0x00066B3D File Offset: 0x00064D3D
		public void SetVisibility(bool _visable)
		{
			if (this.visable == _visable)
			{
				return;
			}
			this.visable = _visable;
			if (MultiSceneCore.Instance == null)
			{
				LevelManager.OnLevelInitialized += this.OnLevelInitialized;
				return;
			}
			this.SyncVisibility();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00066B75 File Offset: 0x00064D75
		private void OnLevelInitialized()
		{
			this.SyncVisibility();
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00066B7D File Offset: 0x00064D7D
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00066B90 File Offset: 0x00064D90
		private void OnDisable()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00066BA3 File Offset: 0x00064DA3
		private void SyncVisibility()
		{
			if (this.visable)
			{
				if (this.pointsInstance != null && this.pointsInstance.Count > 0)
				{
					this.DespawnAll();
				}
				this.Spawn();
				return;
			}
			this.DespawnAll();
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00066BD8 File Offset: 0x00064DD8
		private void Spawn()
		{
			foreach (MultiSceneLocation location in this.locations)
			{
				this.SpawnOnePoint(location, this.name);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00066C34 File Offset: 0x00064E34
		private void SpawnOnePoint(MultiSceneLocation _location, string name)
		{
			if (this.pointsInstance == null)
			{
				this.pointsInstance = new List<SimplePointOfInterest>();
			}
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Vector3 vector;
			if (!_location.TryGetLocationPosition(out vector))
			{
				return;
			}
			SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement:" + name).AddComponent<SimplePointOfInterest>();
			Debug.Log("Spawning " + simplePointOfInterest.name + " for task", this);
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			if (this.range > 0f)
			{
				simplePointOfInterest.IsArea = true;
				simplePointOfInterest.AreaRadius = this.range;
			}
			simplePointOfInterest.Setup(this.icon, name, false, null);
			simplePointOfInterest.SetupMultiSceneLocation(_location, true);
			this.pointsInstance.Add(simplePointOfInterest);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00066D08 File Offset: 0x00064F08
		public void DespawnAll()
		{
			if (this.pointsInstance == null || this.pointsInstance.Count == 0)
			{
				return;
			}
			foreach (SimplePointOfInterest simplePointOfInterest in this.pointsInstance)
			{
				UnityEngine.Object.Destroy(simplePointOfInterest.gameObject);
			}
			this.pointsInstance.Clear();
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00066D80 File Offset: 0x00064F80
		public void DespawnPoint(SimplePointOfInterest point)
		{
			if (this.pointsInstance != null && this.pointsInstance.Contains(point))
			{
				this.pointsInstance.Remove(point);
			}
			UnityEngine.Object.Destroy(point.gameObject);
		}

		// Token: 0x040013D9 RID: 5081
		private bool visable;

		// Token: 0x040013DA RID: 5082
		public new string name;

		// Token: 0x040013DB RID: 5083
		public List<MultiSceneLocation> locations;

		// Token: 0x040013DC RID: 5084
		public float range;

		// Token: 0x040013DD RID: 5085
		private List<SimplePointOfInterest> pointsInstance;

		// Token: 0x040013DE RID: 5086
		public Sprite icon;

		// Token: 0x040013DF RID: 5087
		public Color iconColor = Color.white;

		// Token: 0x040013E0 RID: 5088
		public Color shadowColor = Color.white;

		// Token: 0x040013E1 RID: 5089
		public float shadowDistance;
	}
}
