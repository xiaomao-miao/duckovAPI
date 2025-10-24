using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x0200032D RID: 813
	[Serializable]
	public struct MultiSceneLocation
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0006431E File Offset: 0x0006251E
		// (set) Token: 0x06001B93 RID: 7059 RVA: 0x00064326 File Offset: 0x00062526
		public Transform LocationTransform
		{
			get
			{
				return this.GetLocationTransform();
			}
			private set
			{
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x00064328 File Offset: 0x00062528
		// (set) Token: 0x06001B95 RID: 7061 RVA: 0x00064330 File Offset: 0x00062530
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
			set
			{
				this.sceneID = value;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x0006433C File Offset: 0x0006253C
		public SceneReference Scene
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00064360 File Offset: 0x00062560
		// (set) Token: 0x06001B98 RID: 7064 RVA: 0x00064368 File Offset: 0x00062568
		public string LocationName
		{
			get
			{
				return this.locationName;
			}
			set
			{
				this.locationName = value;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x00064371 File Offset: 0x00062571
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0006437E File Offset: 0x0006257E
		public Transform GetLocationTransform()
		{
			if (this.Scene == null)
			{
				return null;
			}
			if (this.Scene.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(this.Scene, this.locationName);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000643AC File Offset: 0x000625AC
		public bool TryGetLocationPosition(out Vector3 result)
		{
			result = default(Vector3);
			if (MultiSceneCore.Instance == null)
			{
				return false;
			}
			if (MultiSceneCore.Instance.TryGetCachedPosition(this.sceneID, this.locationName, out result))
			{
				return true;
			}
			Transform location = SceneLocationsProvider.GetLocation(this.sceneID, this.locationName);
			if (location != null)
			{
				result = location.transform.position;
				return true;
			}
			return false;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00064419 File Offset: 0x00062619
		internal string GetDisplayName()
		{
			return this.DisplayName;
		}

		// Token: 0x0400138C RID: 5004
		[SerializeField]
		private string sceneID;

		// Token: 0x0400138D RID: 5005
		[SerializeField]
		private string locationName;

		// Token: 0x0400138E RID: 5006
		[SerializeField]
		private string displayName;
	}
}
