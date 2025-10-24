using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x0200032C RID: 812
	[Serializable]
	public class SubSceneEntry
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00064220 File Offset: 0x00062420
		public string AmbientSound
		{
			get
			{
				return this.overrideAmbientSound;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001B8C RID: 7052 RVA: 0x00064228 File Offset: 0x00062428
		public bool IsInDoor
		{
			get
			{
				return this.isInDoor;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001B8D RID: 7053 RVA: 0x00064230 File Offset: 0x00062430
		public SceneInfoEntry Info
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.sceneID);
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001B8E RID: 7054 RVA: 0x00064240 File Offset: 0x00062440
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					Debug.LogWarning("未找到场景" + this.sceneID + "的相关信息，获取SceneReference失败。");
					return null;
				}
				return info.SceneReference;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x0006427C File Offset: 0x0006247C
		public string DisplayName
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					return this.sceneID;
				}
				return info.DisplayName;
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000642A0 File Offset: 0x000624A0
		internal bool TryGetCachedPosition(string locationPath, out Vector3 result)
		{
			result = default(Vector3);
			if (this.cachedLocations == null)
			{
				return false;
			}
			SubSceneEntry.Location location = this.cachedLocations.Find((SubSceneEntry.Location e) => e.path == locationPath);
			if (location == null)
			{
				return false;
			}
			result = location.position;
			return true;
		}

		// Token: 0x04001387 RID: 4999
		[SceneID]
		public string sceneID;

		// Token: 0x04001388 RID: 5000
		[SerializeField]
		private string overrideAmbientSound = "Default";

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		private bool isInDoor;

		// Token: 0x0400138A RID: 5002
		public List<SubSceneEntry.Location> cachedLocations = new List<SubSceneEntry.Location>();

		// Token: 0x0400138B RID: 5003
		public List<SubSceneEntry.TeleporterInfo> cachedTeleporters = new List<SubSceneEntry.TeleporterInfo>();

		// Token: 0x020005DD RID: 1501
		[Serializable]
		public class Location
		{
			// Token: 0x1700078A RID: 1930
			// (get) Token: 0x0600293E RID: 10558 RVA: 0x0009900A File Offset: 0x0009720A
			public string DisplayName
			{
				get
				{
					return this.displayName;
				}
			}

			// Token: 0x1700078B RID: 1931
			// (get) Token: 0x0600293F RID: 10559 RVA: 0x00099012 File Offset: 0x00097212
			// (set) Token: 0x06002940 RID: 10560 RVA: 0x0009901A File Offset: 0x0009721A
			public string DisplayNameRaw
			{
				get
				{
					return this.displayName;
				}
				set
				{
					this.displayName = value;
				}
			}

			// Token: 0x040020E0 RID: 8416
			public string path;

			// Token: 0x040020E1 RID: 8417
			public Vector3 position;

			// Token: 0x040020E2 RID: 8418
			public bool showInMap;

			// Token: 0x040020E3 RID: 8419
			[SerializeField]
			private string displayName;
		}

		// Token: 0x020005DE RID: 1502
		[Serializable]
		public class TeleporterInfo
		{
			// Token: 0x040020E4 RID: 8420
			public Vector3 position;

			// Token: 0x040020E5 RID: 8421
			public MultiSceneLocation target;

			// Token: 0x040020E6 RID: 8422
			public Vector3 nearestTeleporterPositionToTarget;
		}
	}
}
